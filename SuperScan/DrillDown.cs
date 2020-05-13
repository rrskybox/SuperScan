
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using TheSkyXLib;
using AstroImage;

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

        public string FollowUpPath { get; set; }
        public string ImageBankPath { get; set; }

        public string TargetName { get; set; }
        public double TargetRAhrs { get; set; }
        public double TargetDecdeg { get; set; }
        public string TargetImageDir { get; set; }
        public AstroImage.FitsFile TargetFits;

        public string FitsFilePath { get; set; }

        public DrillDown(DateTime edate)
        {
            Configuration ss_cfg = new Configuration();
            string followUpDirPath = ss_cfg.FollowUpFolder;
            FollowUpPath = followUpDirPath + "\\" + edate.ToString("MM-dd-yyyy");
            if (!System.IO.Directory.Exists(FollowUpPath))
            {
                System.IO.Directory.CreateDirectory(FollowUpPath);
            }
            ImageBankPath = ss_cfg.ImageBankFolder;
            if (!System.IO.Directory.Exists(ImageBankPath))
            {
                System.IO.Directory.CreateDirectory(ImageBankPath);
            }

            return;
        }

        public void Shoot(string gName, double expTime)
        {
            //Take a fresh image at 600 seconds
            //That will be placed in the 
            Configuration sscfg = new Configuration();
            ccdsoftImage tsx_im = new ccdsoftImage();
            ccdsoftCamera tsx_cc = new ccdsoftCamera();
            tsx_im.Path = FollowUpPath + "\\" + gName + ".fit";
            tsx_cc.AutoSaveOn = 0;          //Autosave Off
            //No filter change
            tsx_cc.ExposureTime = expTime;
            tsx_cc.Frame = ccdsoftImageFrame.cdLight;
            tsx_cc.ImageReduction = ccdsoftImageReduction.cdAutoDark;
            switch (sscfg.CalibrationType)
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
            tsx_cc.Asynchronous = 0;        //Asynchronous on

            tsx_cc.TakeImage();
            //Wait for completion
            while (tsx_cc.State != ccdsoftCameraState.cdStateNone)
            {
                System.Threading.Thread.Sleep(1000);
                System.Windows.Forms.Application.DoEvents();
            }
            tsx_im.AttachToActiveImager();
            tsx_im.Save();
        }

        public string Display(string galaxyName, double suspectRAhrs, double suspectDecdeg)
        {
            const int sampleSize = 20;

            //Test code for PlateSolve2 Wrapper
            //CancellationToken cToken;
            TargetName = galaxyName;
            TargetRAhrs = suspectRAhrs;
            TargetDecdeg = suspectDecdeg;
            string followUpfileName = FollowUpPath + "\\" + TargetName + ".fit";
            TargetImageDir = ImageBankPath + "\\" + TargetName;
            //Show suspect in astroimage form, if PlateSolve2 is installed
            // if not, then an exception will be thrown

            //
            //test code
            //fileName = "C:\\Users\\Rick McAlister\\Documents\\SuperScan\\Image Bank\\NGC 1023\\NGC 1023_2019-10-31-2138.fit";
            //fileName = "C:\\Users\\Rick McAlister\\Documents\\SuperScan\\Image Bank\\NGC 1023\\CurrentImage.fit";
            //

            TargetFits = new FitsFile(followUpfileName, true);
            double pixSize = 1;
            if (TargetFits.FocalLength != 0) pixSize = (206.265 / TargetFits.FocalLength) * TargetFits.XpixSz;
            //
            ccdsoftImage tsxim = new ccdsoftImage();
            tsxim.Path = FollowUpPath + "\\" + galaxyName + ".fit";
            tsxim.Open();
            //Try to image link.  If not successful, probably too few stars
            //  if so, just return out of this;
            ImageLink tsxil = new ImageLink();
            tsxil.pathToFITS = tsxim.Path;
            try { tsxil.execute(); }
            catch (Exception ex)
            {
                return ("Image Link Error: " + ex.Message);
            }
            //Show pic on scrren
            tsxim.Visible = 0;

            ImageLinkResults tsxilr = new ImageLinkResults();
            int rlt = tsxilr.succeeded;
            string rltText = tsxilr.errorText;

            try
            {
                //tsxim.InsertWCS(true);
                tsxim.ShowInventory();
            }
            catch (Exception ex)
            {
                string evx = ex.Message;
                return (evx);
            }

            //Look for a light source within 10 pixels of the target RA/Dec
            //The developer is picking an arbitrary 10 pixel square box as "near"
            int iLS = FindClosestLightSource(tsxim, TargetRAhrs, TargetDecdeg, 10);
            if (iLS == -1)
            {
                return ("No light source found at suspect location\r\n  **Aborting check**");
            }

            //Success -- light source at target location.  Get magnitude and X,Y coordinates for all light sources
            var rMagArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryMagnitude);
            var rXArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryX);
            var rYArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryY);

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
            tsxsc.RightAscension = TargetRAhrs;
            tsxsc.Declination = TargetDecdeg;
            int Xtcen = tsxsc.WidthInPixels / 2;
            int Ytcen = tsxsc.HeightInPixels / 2;
            //find the star at the center of the chart
            tsxsc.ClickFind(Xtcen, Ytcen);
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            string starName = tsxoi.ObjInfoPropOut;
            //Open astrodisplay form of follow up image

            tsxim = null;
            tsxoi = null;
            tsxsc = null;
            //Report on computed apparant magnitude
            return ("Nearest star: " + starName + "\r\n" +
                                                   "Adjusted apparent magnitude = " +
                                                   avgTgtAdjMag.ToString() + "  +/- " +
                                                   meanDevTgtAdjMag.ToString());
        }

        private int FindClosestLightSource(ccdsoftImage tsxim, double sRA, double sDec, int pDistance)
        {
            //Searches for a "near" light source to the location SRA, SDec and returns it//s index
            //if not, then -1 is returned

            tsxim.RADecToXY(sRA, sDec);
            double tLocX = tsxim.RADecToXYResultX();
            double tLocY = tsxim.RADecToXYResultY();

            double tLhighX = tLocX + pDistance;
            double tLlowX = tLocX - pDistance;
            double tLhighY = tLocY + pDistance;
            double tLlowY = tLocY - pDistance;

            var rXArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryX);
            var rYArr = tsxim.InventoryArray((int)ccdsoftInventoryIndex.cdInventoryY);
            for (int iR = 0; iR < rXArr.Length; iR++)
            {
                if ((rXArr[iR] < tLhighX)
                    && (rXArr[iR] > tLlowX)
                    && (rYArr[iR] < tLhighY)
                    && (rYArr[iR] > tLlowY))
                {
                    return (iR);

                }
            }
            return (-1);  //Error return
        }

        public Image GetFollowUpImage(int zoom) => AstroImage.AstroDisplay.FitsToTargetImage(TargetFits, TargetRAhrs, TargetDecdeg, zoom);

        public Image[] GetBlinkImages(int zoom)
        {
            string imageDir = ImageBankPath + "\\" + TargetName;
            DirectoryInfo dinfo = new DirectoryInfo(imageDir);
            var allFiles = dinfo.GetFiles("NGC*.fit").OrderByDescending(p => p.CreationTime).ToArray();
            string[] imageFiles = { allFiles[0].FullName, allFiles[1].FullName };
            return AstroImage.AstroDisplay.FitsFilesToTargetImages(imageFiles, TargetRAhrs, TargetDecdeg, zoom);
        }

        public void ReBlinker(string targetName, double tgtRA, double tgtDec, int zoom, string firstFile, string secondFile)
        {
            //Creates a new process to run the AstroDisplay independently
            string toolName = "Blinker.appref-ms";
            string ttdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\TSXToolkit\\TSXToolkit";
            //string blinkerPath = Path.Combine(ttdir, toolName);

            string blinkerPath = @"C:\Users\Rick McAlister\OneDrive\CS Projects\Blinker\Blinker\bin\Debug\Blinker.exe";
            //string blinkerPath = @"C:\Users\Rick McAlister\AppData\Local\Apps\2.0\CEVNCHMT.VDQ\0JZ09JAL.1TN\blin..tion_429a632034a792cd_0001.0000_0ac1f94652a855a2\Blinker.exe";
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = blinkerPath;
            proc.StartInfo.Arguments =
                TargetName + "," +
                MathHelpers.HoursToRad(tgtRA).ToString("0.00000", CultureInfo.InvariantCulture) + "," +
                MathHelpers.DegToRad(tgtDec).ToString("0.00000", CultureInfo.InvariantCulture) + "," +
                zoom.ToString() + "," +
                firstFile + "," +
                secondFile;
            proc.Start();

            while (!proc.HasExited) { Thread.Sleep(1000); }
            proc.Dispose();
            return;
        }
    }

}
