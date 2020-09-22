/// NovaDetection Class
///
/// ------------------------------------------------------------------------
/// Module Name: NovaDetection 
/// Purpose: Load and compare galactic images for incremental light sources
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description: TBD
/// 
/// ------------------------------------------------------------------------
using System;
using TheSkyXLib;

namespace SuperScan
{
    class NovaDetection
    {
        private double DRILLDOWN_EXPOSURE = 600.00;

        enum ccdsoftInventoryIndex
        {
            cdInventoryX,
            cdInventoryY,
            cdInventoryMagnitude,
            cdInventoryClass,
            cdInventoryFWHM,
            cdInventoryMajorAxis,
            cdInventoryMinorAxis,
            cdInventoryTheta,
            cdInventoryEllipticity
        };

        //Private Variables
        string galaxyName;
        string curImagePath;
        string refImagePath;
        double galaxyRA;
        double galaxyDec;
        double grandeWidth;
        double grandeHeight;

        //private Logger ss_log = new Logger();

        //statements for linking logging method LogUpdate to the main form logging routine.
        public delegate void LogEventHandler(string LogText);
        public event LogEventHandler LogUpdate;

        public NovaDetection()
        {
            return;
        }

        public bool Detect(string gname, double subFrameSizeInArcSec, double gRA, double gDec, string freshImagePath, string storedImagePath, string workingImageFolder)
        {
            //Store calling parameters
            galaxyName = gname;
            curImagePath = freshImagePath;
            refImagePath = storedImagePath;
            galaxyRA = gRA;
            galaxyDec = gDec;

            //Create Image Array for reference image
            ImageArray refImage = new ImageArray(refImagePath);
            //Create Image Array for current image
            ImageArray curImage = new ImageArray(curImagePath);
            //Plate solve for x,y positions of galactic center
            RADecToXY refGal = new RADecToXY(refImagePath, gRA, gDec);
            if (!refGal.IsPlateSolved)
            {
                LogEntry("Reference image plate solve failure.");
                return false;
            }
            else
            {
                LogEntry("Reference Image plate solve successful.");
            }
            RADecToXY curGal = new RADecToXY(curImagePath, gRA, gDec);
            if (!curGal.IsPlateSolved)
            {
                LogEntry("Current image plate solve failure.");
                return false;
            }
            else
            {
                LogEntry("Current Image plate solve successful.");
            }
            //Save the image//s height and width for possible future use
            grandeHeight = curGal.Height;
            grandeWidth = curGal.Width;

            //Set x,y boundaries for galaxy subframe
            //   using x,y coordinates of RA,Dec of galaxy in image
            //   offset by 2x the maximum axis size, in both x and y.
            int subframeSize = Convert.ToInt32(subFrameSizeInArcSec / refGal.Scale);

            //Compute rotation angle in radians
            double rotation = (curGal.PA - refGal.PA) * Math.PI / 180;

            //Open a new difference image in TSX of the galaxy subframe size
            ImageArray sdifImage = new ImageArray(subframeSize, subframeSize);

            //Open a new reference image in TSX of the galaxy subframe size            
            ImageArray srefImage = new ImageArray(subframeSize, subframeSize);

            //Open a new current image in TSX of the galaxy subframe size
            ImageArray scurImage = new ImageArray(subframeSize, subframeSize);

            int highpix = -32000;
            int lowpix = 32000;
            double curavg = 0;
            double refavg = 0;

            //First time through: create subframed and translated current and reference images registered to the galaxy center
            // get the average pixel value for the current and reference images and compute relative intensity
            //  between the two images.  
            for (int iXp = 0; iXp < subframeSize; iXp++)
            {
                for (int iYp = 0; iYp < subframeSize; iYp++)
                {
                    int iXd = iXp - subframeSize / 2;
                    int iYd = iYp - subframeSize / 2;
                    int iXc = curGal.X + iXd;
                    int iYc = curGal.Y + iYd;
                    int iXr = refGal.X + TransformX(iXd, -iYd, rotation);
                    int iYr = refGal.Y - TransformY(iXd, -iYd, rotation);
                    //Subtract adjusted reference image pixel intensity from current image pixel intensity
                    //  and put it in the subframe array
                    int curPix = curImage.GetPixel(iXc, iYc);
                    scurImage.SetPixel(iXp, iYp, curPix);
                    int refPix = refImage.GetPixel(iXr, iYr);
                    srefImage.SetPixel(iXp, iYp, refPix);

                    int difPix = Math.Abs(curPix - refPix);

                    //Determine average pixel values for the current and reference images or at least, get the sum for now
                    curavg = curavg + curPix;
                    refavg = refavg + refPix;
                    //Keep track of high, low pixel values for the difference image 
                    if (difPix > highpix)
                    { highpix = difPix; }
                    if (difPix < lowpix)
                    { lowpix = difPix; }
                }

            }

            curavg = curavg / (subframeSize * subframeSize);
            refavg = refavg / (subframeSize * subframeSize);
            double difintensity = curavg / refavg;

            //Second time through:  Create the difference image with difference between 
            //  current image and reference images
            for (int iXp = 0; iXp < subframeSize; iXp++)
            {
                for (int iYp = 0; iYp < subframeSize; iYp++)
                {
                    //Subtract adjusted reference image pixel intensity from current image pixel intensity
                    int curPix = scurImage.GetPixel(iXp, iYp);
                    int refPix = srefImage.GetPixel(iXp, iYp);
                    int difPix = Convert.ToInt32(Math.Abs((curPix - (refPix * difintensity))));
                    //Pedistal out the current image average -- that is, all star candidates must be above the average pixel value
                    difPix = Convert.ToInt32((difPix - curavg) * Math.Sign(difPix - curavg));

                    // Normalize the pixel value between the high and low values
                    difPix = Convert.ToInt32((difPix - lowpix) * (highpix - lowpix) / (256 * 256));
                    //  OR
                    //difPix = Convert.ToInt32((difPix - lowpix) * (highpix - lowpix));

                    //Update the difference image with this pixel
                    sdifImage.SetPixel(iXp, iYp, difPix);
                }
            }
            //Normalize pixDiff to 256 bit intensity until TSX DataArray gets fixed
            //Normalize the array between the highs and lows, and flatten between 0 and 256

            //Save the difference image into the Galaxy Image Bank
            System.IO.FileInfo rfi = new System.IO.FileInfo(workingImageFolder);
            string rdir = rfi.DirectoryName;
            string dFilePath = rdir + "\\" + "DifferenceImage.fit";
            sdifImage.Store(dFilePath);
            string rFilePath = rdir + "\\" + "ReferenceImage.fit";
            srefImage.Store(rFilePath);
            string cFilePath = rdir + "\\" + "CurrentImage.fit";
            scurImage.Store(cFilePath);
            rfi = null;
            sdifImage = null;
            srefImage = null;
            scurImage = null;
            GC.Collect();

            //Check for anomolies using TSX Sextractor (e.g. ShowInventory)
            if (NewStar(gname, dFilePath, rFilePath))
            {
                //If a suspect is found, take and store a five minute image for later analysis
                //  Can//t go longer than 5 minutes as Image Link seems to fail a lot with too many stars.
                //  This is going to take 10 minutes for the shot and dark, plus another dark
                //  will have to be taken for the regular scan upon resumption.
                LogEntry("Potential transient identified.  Taking follow up image.");
                DrillDown ss_dd = new DrillDown(DateTime.Now);
                ss_dd.Shoot(gname, DRILLDOWN_EXPOSURE);
                LogEntry("Follow up image acquired and stored.");
                return true;
            }
            return false;
        }

        public bool NewStar(string gname, string dfilepath, string rfilepath)
        {
            //Look for star-like reminant in difference image using the TSX "ShowInventory" (Sextractor) function
            //   return true if star-like object found, false if not.
            //

            //Open a new TSX Image object, then load and open difference image
            ccdsoftImage tsx_dif = new ccdsoftImage();
            tsx_dif.Path = dfilepath;
            tsx_dif.Open();
            //Prepare TSX to close the image window when closing the image
            tsx_dif.DetachOnClose = 0;
            //Look for stars using Sextractor
            //  If fails, close the object and image
            try
            {
                int tdstat = tsx_dif.ShowInventory();
            }
            catch
            {
                tsx_dif.Close();
                LogEntry("No suspects:  Sextractor failed on Difference Image.");
                return false;
            }
            // Sextractor didnt// run into any problems, but may have found nothing
            //   Check how many objects found, if none then close up and return false
            //   If something is found, then leave the window open and return "true"
            var dMagArr = tsx_dif.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryMagnitude);
            //double[] FWHMArr = Convert.ToDouble(tDiff.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryFWHM));
            var dXPosArr = tsx_dif.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryX);
            var dYPosArr = tsx_dif.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryY);
            //double[] ElpArr = Convert.ToDouble(tDiff.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryEllipticity));
            var dClsArr = tsx_dif.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryClass);

            if (dMagArr.Length == 0)
            {
                tsx_dif.Close();
                LogEntry("No suspects:  No light sources found on Difference Image.");
                return false;
            }
            else
            {

                //We have at least one hit in the box.  Now look to see if any don//t match up with
                //  light sources found in the reduced reference image.
                //Open a new TSX Image object, then load and open difference image
                ccdsoftImage tsx_ref = new ccdsoftImage();
                tsx_ref.Path = rfilepath;
                tsx_ref.Open();
                //Prepare TSX to close the image window when closing the image
                tsx_ref.DetachOnClose = 0;
                //Look for stars using Sextractor
                //  If fails, close the object and image
                try
                {
                    int cdstat = tsx_ref.ShowInventory();
                }
                catch
                {
                    tsx_ref.Close();
                    tsx_dif.Close();
                    LogEntry("No suspects:  Sextractor failed on Reference Image.");
                    return false;
                }
                // Sextractor didnt// run into any problems, but may have found nothing
                //   Check how many objects found, if none then close up and return false
                //   If something is found, then leave the window open and return "true"
                var rMagArr = tsx_ref.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryMagnitude);
                //double[] FWHMArr = Convert.ToDouble(tDiff.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryFWHM));
                var rXPosArr = tsx_ref.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryX);
                var rYPosArr = tsx_ref.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryY);
                //double[] ElpArr = Convert.ToDouble(tDiff.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryEllipticity));
                var rClsArr = tsx_ref.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryClass);
                //So let//s make being within 20 pixels (40 arcsec) of each other sufficient for calling two light sources the same.
                //Proceed through the light source array looking for matches, there shouldn//t be many
                int proximity = 20;
                Boolean nomatch;
                for (int di = 0; di < dXPosArr.Length; di++)
                {
                    double dXpos = Convert.ToDouble(dXPosArr[di]);
                    double dYpos = Convert.ToDouble(dYPosArr[di]);
                    double dCls = Convert.ToDouble(dClsArr[di]);
                    nomatch = true;
                    if (dCls >= 0.8)
                    {
                        for (int ri = 0; ri < rXPosArr.Length; ri++)
                        {
                            double rXpos = Convert.ToDouble(rXPosArr[ri]);
                            double rYpos = Convert.ToDouble(rYPosArr[ri]);

                            if ((Math.Abs(dXpos - rXpos) <= proximity) && (Math.Abs(dYpos - rYpos) <= proximity))
                            {
                                nomatch = false;
                                break;
                            }
                        }
                        if (nomatch)
                        {
                            // A suspect has been found. Log it//s x,y location on the Difference image, then
                            //  Get its location (RA,Dec) by using XYToRADec method that runs yet another Image Link
                            //  on the current image file (not the cropped one), extrapolating the cropped difference image x,y
                            //  to the uncropped current image, then using TSX to convert the extrapolated X,Y to an RA,Dec.
                            //
                            //  Add to Suspect List then, log, close, collect and return

                            //  We may do more later...
                            LogEntry("Suspect without alibi in " + dfilepath + " at X= " + Convert.ToInt32(dXpos) + "   Y= " + Convert.ToInt32(dYpos));

                            double xCurPos = ((dXpos - (tsx_dif.WidthInPixels) / 2)) + (grandeWidth / 2);
                            double yCurPos = ((dYpos - (tsx_dif.HeightInPixels) / 2)) + (grandeHeight / 2);
                            XYToRADec ss_perp = new XYToRADec(curImagePath, xCurPos, yCurPos);
                            double pRA = ss_perp.RA;
                            double pDec = ss_perp.Dec;
                            LogEntry("Suspect//s coordinates (RA,Dec) = " + pRA.ToString() + " Hrs, " + pDec.ToString() + " Deg");

                            //Now create a new entry and save the suspect in the suspect file
                            Suspect ss_sus = new Suspect();
                            ss_sus.GalaxyName = gname;
                            ss_sus.Event = DateTime.Now;
                            ss_sus.SuspectRA = pRA;
                            ss_sus.SuspectDec = pDec;
                            ss_sus.SuspectCurrentLocationX = dXpos;
                            ss_sus.SuspectCurrentLocationY = dYpos;
                            ss_sus.Store();

                            tsx_dif.Close();
                            tsx_dif = null;
                            tsx_ref.Close();
                            tsx_ref = null;
                            ss_perp = null;
                            ss_sus = null;
                            GC.Collect();
                            return true;
                        }
                    }
                }
                //If no suspects found, log, close, collect and return;
                LogEntry("All suspects have alibis from Reference image.");
                tsx_dif.Close();
                tsx_dif = null;
                tsx_ref.Close();
                tsx_ref = null;
                return false;
            }
        }

        private int TransformX(double X, double Y, double angleR)
        //Computes X coordinate of a rotation on X,Y through a rotation of angle degrees
        //X in pixels, Y in pixels, angle in radians
        //x// = x cos deltaPA - y sin deltaPA
        {
            // double angleR = Math.PI * angleD / 180.0;
            // angleR = Math.PI * -90.001 / 180.0;
            double dX = (X * Math.Cos(angleR)) - (Y * Math.Sin(angleR));
            return (Convert.ToInt32(dX));
        }

        private int TransformY(double X, double Y, double angleR)
        //Computes Y coordinate of a rotation on X,Y through a rotation of angle degrees
        //X in pixels, Y in pixels, angle in radians
        //y// = y cos deltaPA + x sin deltaPA
        {
            //double angleR = Math.PI * angleD / 180.0;
            //angleR = Math.PI * -90.001 / 180.0;
            double dY = (Y * Math.Cos(angleR)) + (X * Math.Sin(angleR));
            return (Convert.ToInt32(dY));
        }

        //Classes for converting XY to RADec and RADec to XY (using Image Link and TSX)

        private class RADecToXY
        //Class to get the J2000 RA,Dec location of a specific pixel x,y in an image
        //tRA is RA location in hours, tDec is Dec location in degrees
        {
            private bool plateSolveResult;
            private int targetX;
            private int targetY;
            private double northangle;
            private double imagescale;
            private double imagewidth;
            private double imageheight;

            public RADecToXY(string wcsfilepath, double tRA, double tDec)
            {
                //Open reference image in TSX.  Plate solve.
                ccdsoftImage tsx_img = new ccdsoftImage();

                tsx_img.Path = wcsfilepath;
                tsx_img.Open();
                tsx_img.DetachOnClose = 0;
                try
                {
                    tsx_img.InsertWCS(true);
                }
                catch
                {
                    tsx_img.Close();
                    plateSolveResult = false;
                    return;
                }
                tsx_img.RADecToXY(tRA, tDec);
                targetX = Convert.ToInt32(tsx_img.RADecToXYResultX());
                targetY = Convert.ToInt32(tsx_img.RADecToXYResultY());
                northangle = tsx_img.NorthAngle;
                imagescale = tsx_img.ScaleInArcsecondsPerPixel;
                imagewidth = tsx_img.WidthInPixels;
                imageheight = tsx_img.HeightInPixels;
                //Check for some catastrophic problem with the image link result
                if ((targetX < 0) || (targetX > imagewidth) || (targetY < 0) || (targetY > imageheight))
                { plateSolveResult = false; }
                else
                { plateSolveResult = true; };

                tsx_img.Close();
                return;
            }

            //Properties for RADecToXY class -- nothing complex.  should be self-explanatory.
            public bool IsPlateSolved
            {
                get { return plateSolveResult; }
            }

            public int X
            {
                get { return targetX; }
            }

            public int Y
            {
                get { return targetY; }
            }

            public double Scale
            {
                get { return imagescale; }
            }

            public double PA
            {
                get { return northangle; }
            }

            public double Width
            {
                get { return imagewidth; }
            }

            public double Height
            {
                get { return imageheight; }
            }
        }

        private class XYToRADec
        //Class to get the specific pixel x,y based on a J2000 RA,Dec location of a in a Plate Solved image
        //Input tRA will be RA location in hours, tDec will be Dec location in degrees
        //Output properties X and Y are in pixels
        {
            private bool plateSolve;
            private double targetRA;
            private double targetDec;
            private double northangle;
            private double imagescale;
            private double imagewidth;
            private double imageheight;

            public XYToRADec(string wcsfilepath, double Xpos, double Ypos)
            {
                //Open reference image in TSX.  Plate solve.
                ccdsoftImage tsx_img = new ccdsoftImage();

                tsx_img.Path = wcsfilepath;
                tsx_img.Open();
                tsx_img.DetachOnClose = 0;
                try
                {
                    tsx_img.InsertWCS(true);
                }
                catch
                {
                    tsx_img.Close();
                    plateSolve = false;
                    return;
                }
                tsx_img.XYToRADec(Xpos, Ypos);
                targetRA = tsx_img.XYToRADecResultRA();
                targetDec = tsx_img.XYToRADecResultDec();
                northangle = tsx_img.NorthAngle;
                imagescale = tsx_img.ScaleInArcsecondsPerPixel;
                imagewidth = tsx_img.WidthInPixels;
                imageheight = tsx_img.HeightInPixels;
                plateSolve = true;
                tsx_img.Close();
                return;
            }

            public bool PlateSolve
            {
                get { return plateSolve; }
            }

            public double RA
            {
                get { return targetRA; }
            }

            public double Dec
            {
                get { return targetDec; }
            }

            public double Scale
            {
                get { return imagescale; }
            }

            public double PA
            {
                get { return northangle; }
            }
            public double Width
            {
                get { return imagewidth; }
            }

            public double Height
            {
                get { return imageheight; }
            }
        }

        //Method to link to SuperScan main form for logging progress.
        private void LogEntry(string upd)
        //Method for projecting log entry to the SuperScan Main Form
        {
            LogUpdate(upd);
            return;
        }

    }
}

