/// DrillDown carries methods and properties for making and reading
/// a long (10 minute) exposure when a suspect is detected.
///
/// Instantiation of the class opens the FollowUp subdirectory and
/// creates a new "MM-DD-YY" subdirectory, based on the current date,
/// if one doesn't exist already.  The method "Path" returns the path
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
/// brightest, un-anti-bloom'ed (under 1/2 max pixel value) sources
///
/// The mean and deviation to mean is calculated for the reference source
/// instrument magnitude and catalog magnitude.
///
/// These values are then used to calculate a relative magnitude with
/// error deviation for the target object.
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheSkyXLib;

namespace SuperScan
{
    class DrillDown
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

        private string followUpPath;

        public DrillDown(DateTime edate)
        {
            Configuration ss_cfg = new Configuration();
            string followUpDirPath = ss_cfg.FollowUpFolder;
            followUpPath = followUpDirPath + "\\" + edate.ToString("MM-dd-yyyy");
            if (!System.IO.Directory.Exists(followUpPath))
            {
                System.IO.Directory.CreateDirectory(followUpPath);
            }
            return;
        }

        public void Shoot(string gName, double expTime)
        {
            //Take a fresh image at 600 seconds
            //That will be placed in the 
            ccdsoftImage tsx_im = new ccdsoftImage();
            ccdsoftCamera tsx_cc = new ccdsoftCamera();
            tsx_im.Path = followUpPath + "\\" + gName + ".fit";
            tsx_cc.AutoSaveOn = 0;          //Autosave Off
            //No filter change
            tsx_cc.ExposureTime = expTime;
            tsx_cc.Frame = ccdsoftImageFrame.cdLight;
            tsx_cc.ImageReduction = ccdsoftImageReduction.cdAutoDark;
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

        public void Display(string galaxyName, double tRA, double tDec)
        {
            const int sampleSize = 20;

            ccdsoftImage tsxim = new ccdsoftImage();
            tsxim.Path = followUpPath + "\\" + galaxyName + ".fit";
            tsxim.Open();
            //Try to image link.  If not successful, probably too few stars
            //  if so, just return out of this;
            ImageLink tsxil = new ImageLink();
            tsxil.pathToFITS = tsxim.Path;
            try { tsxil.execute(); }
            catch (Exception ex)
            {
                MessageBox.Show("Image Link Error: " + ex.Message);
                return;
            }
            
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
                System.Windows.Forms.MessageBox.Show(evx);
                return;
            }

             //Look for a light source within 10 pixels of the target RA/Dec
            //The developer is picking an arbitrary 10 pixel square box as "near"
            int iLS = FindClosestLightSource(tsxim, tRA, tDec, 10);
            if (iLS == -1)
            {
                System.Windows.Forms.MessageBox.Show("No light source found at suspect location\r\n  **Aborting check**");
                GC.Collect();
                return;
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
            for (int i =0;i<starCount;i++)
            { avgMagDev += Math.Pow((difMag[i] - avgMagDif),2); }
            avgMagDev = Math.Sqrt(avgMagDev / starCount);

            //Compute the adjusted magnitude for target
            double avgTgtAdjMag = tMag - avgMagDif;
            //Compute the adjusted magnitude error
            double meanDevTgtAdjMag = avgMagDev;
            
            //Return center of starchart to target location
            //Set the center of view to the suspect's RA/Dec and light up the target icon
            //
            //Recenter the star chart on the RA/Dec coordinates
            tsxsc.RightAscension = tRA;
            tsxsc.Declination = tDec;
            int Xtcen = tsxsc.WidthInPixels / 2;
            int Ytcen = tsxsc.HeightInPixels / 2;
            //find the star at the center of the chart
            tsxsc.ClickFind(Xtcen, Ytcen);
            tsxoi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
            string starName = tsxoi.ObjInfoPropOut;
            //tsxsc.Find(tRA.ToString() + ", " + tDec.ToString());

            //Report on computed apparant magnitude
            System.Windows.Forms.MessageBox.Show("Nearest star: " + starName + "\r\n" +
                                                   "Adjusted apparent magnitude = " +
                                                   avgTgtAdjMag.ToString() + "  +/- " +
                                                   meanDevTgtAdjMag.ToString());

            tsxim = null;
            tsxoi = null;
            tsxsc = null;
            GC.Collect();
            return;

        }

        private int FindClosestLightSource(ccdsoftImage tsxim, double sRA, double sDec, int pDistance)
        //Searches for a "near" light source to the location SRA, SDec and returns it's index
        //if not, then -1 is returned

        {

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
    }

}
