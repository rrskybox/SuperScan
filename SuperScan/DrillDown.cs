
/// DrillDown carries methods and properties for making and reading
/// a long (10 minute) exposure when a suspect is detected.
///
/// Instantiation of the class opens the FollowUp subdirectory and
/// creates a new "MM-DD-YY" subdirectory, based on the current date,
/// if one doesn//t exist already.  The method "Path" returns the path
/// to that subdirectory.
/// 
/// The method "Shoot(galaxyName, exposureTime) takes an image and stores
/// it in the subdirectory under the "galaxyName.fit"
/// 
/// The method "Display(galaxyName, RA, Dec) opens the file galaxyName.fit
/// in TSX.  The image is plate solved for location and light sources.
///  
/// The target object is set as the closest object to 
/// an RA/Dec position input.  Reference objects are selected as the 
/// brightest, un-anti-bloom//ed (under 1/2 max pixel value) sources
///
/// The mean and deviation to mean is calculated for the reference source
/// instrument magnitude and catalog magnitude.
///
/// These values are then used to calculate a relative magnitude with
/// error deviation for the target object.
/// 
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using AstroImage;

using TheSky64Lib;



namespace SuperScan
{
    public class DrillDown
    {
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

        private ccdsoftImage tsxim { get; set; }
        public string FollowUpImageFilePath { get; set; }
        public string CurrentImageFilePath { get; set; }
        public string LastImageFilePath { get; set; }
        public string TargetName { get; set; }
        public double TargetRAHrs { get; set; }
        public double TargetDecDeg { get; set; }
        public double TargetX { get; set; }
        public double TargetY { get; set; }
        public double ImagePA { get; set; }
        public string TargetImageDir { get; set; }

        public AstroImage.FitsFile TargetFits;

        public string FitsFilePath { get; set; }

        public DrillDown(string galaxyName, DateTime edate)
        {
            TargetName = galaxyName;
            Configuration ss_cfg = new Configuration();
            string followUpDirPath = ss_cfg.FollowUpFolder + "\\" + edate.ToString("MM-dd-yyyy");
            if (!System.IO.Directory.Exists(followUpDirPath))
                System.IO.Directory.CreateDirectory(followUpDirPath);
            FollowUpImageFilePath = followUpDirPath + "\\" + TargetName + ".fit";

            TargetImageDir = ss_cfg.ImageBankFolder + "\\" + TargetName;
            if (!System.IO.Directory.Exists(TargetImageDir))
                System.IO.Directory.CreateDirectory(TargetImageDir);
            (CurrentImageFilePath, LastImageFilePath) = GetTwoMostRecentImages();

            tsxim = new ccdsoftImage();
            return;
        }

        public void ShootFollowUpImage(string gName)
        {
            //Take a fresh image at configured seconds
            Configuration sscfg = new Configuration();
            double expTime = Convert.ToDouble(sscfg.FollowUpExposure);

            tsxim.Path = FollowUpImageFilePath;
            ccdsoftCamera tsx_cc = new ccdsoftCamera()
            {
                //Note: No filter change
                AutoSaveOn = 0,    //Autosave Off
                ExposureTime = expTime,
                Frame = ccdsoftImageFrame.cdLight,
                Asynchronous = 1     //Asynchronous off
            };
            switch (sscfg.ImageReductionType)
            {
                case "None":
                    {
                        tsx_cc.ImageReduction = ccdsoftImageReduction.cdNone;
                        break;
                    }
                case "Auto":
                    {
                        tsx_cc.ImageReduction = ccdsoftImageReduction.cdAutoDark;
                        break;
                    }
                case "Full":
                    {
                        tsx_cc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat;
                        break;
                    }
            }
            tsx_cc.TakeImage();
            //Wait for completion
            while (tsx_cc.State != ccdsoftCameraState.cdStateNone)
            {
                System.Threading.Thread.Sleep(1000);
                System.Windows.Forms.Application.DoEvents();
            }
            tsxim.AttachToActiveImager();
            tsxim.Save();
        }

        public string LoadFollowUpImage(double suspectRAhrs, double suspectDecdeg)
        {
            TargetRAHrs = suspectRAhrs;
            TargetDecDeg = suspectDecdeg;

            //Show suspect in astroimage form, if PlateSolve2 is installed
            // if not, then an exception will be thrown
            TargetFits = new FitsFile(FollowUpImageFilePath, true);
            double pixSize = 1;
            if (TargetFits.FocalLength != 0)
                pixSize = (206.265 / TargetFits.FocalLength) * TargetFits.XpixSz;
            //
            tsxim.Path = FollowUpImageFilePath;
            tsxim.Open();
            tsxim.ScaleInArcsecondsPerPixel = pixSize; //Must be set
            //Try to image link with an InsertWCS -- need to get PA.
            //If not successful, probably too few stars
            //  if so, just return out of this;
            int wcsresult;
            try { wcsresult = tsxim.InsertWCS(false); }
            catch (Exception ex)
            {
                return ("Image Link Error: " + ex.Message);
            }
            //Show pic on scrren
            tsxim.Visible = 0;
            //Save changed file
            tsxim.Save();
            ImageLinkResults tsxilr = new ImageLinkResults();
            int rlt = tsxilr.succeeded;
            string rltText = tsxilr.errorText;
            ImagePA = tsxilr.imagePositionAngle;
            int txyResult = tsxim.RADecToXY(TargetRAHrs, TargetDecDeg);
            TargetX = tsxim.RADecToXYResultX();
            TargetY = tsxim.RADecToXYResultY();
            return null;
        }

        public string VerifySuspect()
        {
            //Assumes that a WCS has been run on current image
            const int sampleSize = 20;
            try
            {
                tsxim.ShowInventory();
            }
            catch (Exception ex)
            {
                string evx = ex.Message;
                return (evx);
            }

            //Success -- light source at target location.  Get magnitude and X,Y coordinates for all light sources
            var rMagArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryMagnitude);
            var rXArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryX);
            var rYArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryY);

            //Look for a light source within 10 pixels of the target RA/Dec
            //The developer is picking an arbitrary 10 pixel square box as "near"
            const int pDistance = 10;
            //Search for a "near" light source to the location SRA, SDec and returns it//s index
            //if not, then -1 is returned

            tsxim.RADecToXY(TargetRAHrs, TargetDecDeg);
            double tLocX = tsxim.RADecToXYResultX();
            double tLocY = tsxim.RADecToXYResultY();

            double tLhighX = tLocX + pDistance;
            double tLlowX = tLocX - pDistance;
            double tLhighY = tLocY + pDistance;
            double tLlowY = tLocY - pDistance;

            int iLS = rXArr.Length;
            for (int iR = 0; iR < rXArr.Length; iR++)
            {
                if ((rXArr[iR] < tLhighX)
                    && (rXArr[iR] > tLlowX)
                    && (rYArr[iR] < tLhighY)
                    && (rYArr[iR] > tLlowY))
                {
                    iLS = iR;
                }
            }
            if (iLS == rXArr.Length)
                return ("No light source found at suspect location\r\n  **Aborting check**");

            double rMag;
            string cName;
            int starIndex = 0;
            int starCount = 0;
            int minStarCount = sampleSize;
            double tMag = rMagArr[iLS];
            //Collect a set of catalog stars that have positions that match up
            //  with reference light sources of similar intensity to the target
            //  light source.
            //
            //  first, create data arrays for reference magnitudes and catalog magnitudes
            double[] refMag = new double[sampleSize];
            double[] catMag = new double[sampleSize];
            double[] difMag = new double[sampleSize];
            //double[] meanDev = new double[sampleSize];
            //  second, create TSX objects for the star chart and object information
            sky6StarChart tsxsc = new sky6StarChart();
            sky6ObjectInformation tsxoi = new sky6ObjectInformation();
            //loop over all the stars in the light source magnitude array, 
            // or until a sufficient number of stars are found that match  up
            do
            {
                //Compare the reference light source magnitude to the target light source magnitude
                //  if within 1 magnitude of each other then look up the associated star, if any
                rMag = (double)rMagArr[starIndex];
                if (Math.Abs(tMag - rMag) <= 1.0)
                {
                    //Get the RA/Dec location of the reference light source
                    tsxim.XYToRADec(rXArr[starIndex], rYArr[starIndex]);
                    double rRA = tsxim.XYToRADecResultRA();
                    double rDec = tsxim.XYToRADecResultDec();
                    //Center the star chart on the RA/Dec coordinates
                    tsxsc.RightAscension = rRA;
                    tsxsc.Declination = rDec;
                    int Xcen = tsxsc.WidthInPixels / 2;
                    int Ycen = tsxsc.HeightInPixels / 2;
                    //find the star at the center of the chart
                    tsxsc.ClickFind(Xcen, Ycen);
                    //get the name of the star, if any
                    tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_SOURCE_CATALOG);
                    string cSrc = tsxoi.ObjInfoPropOut;
                    //if there is a name, then get its properties
                    if (cSrc != "")
                    {
                        //Get the name of the star
                        tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
                        cName = tsxoi.ObjInfoPropOut;
                        //Get the catalog magnitude of the star
                        tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAG);
                        catMag[starCount] = tsxoi.ObjInfoPropOut;
                        refMag[starCount] = rMag;
                        starCount++;
                    }
                }
                starIndex++;
            } while ((starCount < minStarCount) && (starIndex < rMagArr.Length));

            //Compute the difference bewtween reference magnitude and catalog magnitude
            for (int i = 0; i < starCount; i++)
            { difMag[i] = refMag[i] - catMag[i]; }
            //Compute the average difference
            double avgMagDif = 0;
            for (int i = 0; i < starCount; i++)
            { avgMagDif += difMag[i]; }
            avgMagDif = avgMagDif / starCount;
            //Compute the mean square of the deviation
            double avgMagDev = 0;
            for (int i = 0; i < starCount; i++)
            { avgMagDev += Math.Pow((difMag[i] - avgMagDif), 2); }
            avgMagDev = Math.Sqrt(avgMagDev / starCount);

            //Compute the adjusted magnitude for target
            double avgTgtAdjMag = tMag - avgMagDif;
            //Compute the adjusted magnitude error
            double meanDevTgtAdjMag = avgMagDev;

            //Return center of starchart to target location
            //Set the center of view to the suspect//s RA/Dec and light up the target icon
            //
            //Recenter the star chart on the RA/Dec coordinates
            tsxsc.RightAscension = TargetRAHrs;
            tsxsc.Declination = TargetDecDeg;
            int Xtcen = tsxsc.WidthInPixels / 2;
            int Ytcen = tsxsc.HeightInPixels / 2;
            //find the star at the center of the chart
            tsxsc.ClickFind(Xtcen, Ytcen);
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            string starName = tsxoi.ObjInfoPropOut;
            //Open astrodisplay form of follow up image

            tsxim.Close();
            //Report on computed apparant magnitude
            return ("Nearest star: " + starName + "\r\n" +
                                                   "Adjusted apparent magnitude = " +
                                                   avgTgtAdjMag.ToString() + "  +/- " +
                                                   meanDevTgtAdjMag.ToString());
        }

        public Image GetFollowUpImage()
        {
            AstroDisplay ad = new AstroDisplay(ref TargetFits);
            Point target = new Point((int)TargetX, (int)TargetY);
            ad.AddCrossHair(target, 80, 6);
            return ad.PixImage;
        }

        public Image GetCurrentSampleImage(int zoom)
        {
            FitsFile targetFits;
            targetFits = new FitsFile(CurrentImageFilePath, true);
            AstroDisplay ad = new AstroDisplay(ref targetFits);
            Point target = new Point((int)TargetX, (int)TargetY);
            ad.AddCrossHair(target, 80, 6);
            Size subSize = new Size(ad.PixImage.Width / zoom, ad.PixImage.Height / zoom);
            return ad.Zoom(zoom);
        }

        public Image GetPriorSampleImage(int zoom)
        {
            FitsFile targetFits;
            targetFits = new FitsFile(LastImageFilePath, true);
            AstroDisplay ad = new AstroDisplay(ref targetFits);
            Point target = new Point((int)TargetX, (int)TargetY);
            ad.AddCrossHair(target, 80, 6);
            Size subSize = new Size(ad.PixImage.Width / zoom, ad.PixImage.Height / zoom);
            return ad.Zoom(zoom);
        }

        public (string, string) GetTwoMostRecentImages()
        {
            //string imageDir = ImageBankPath + "\\" + TargetName;
            DirectoryInfo dinfo = new DirectoryInfo(TargetImageDir);
            var allFiles = dinfo.GetFiles("NGC*.fit").OrderByDescending(p => p.CreationTime).ToArray();
            if (allFiles.Count() == 0)
                return (null, null);
            else if (allFiles.Count() == 1)
                return (allFiles[0].FullName, null);
            else
                return (allFiles[0].FullName, allFiles[1].FullName);
        }



    }
}
