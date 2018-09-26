/// FreshImage Class
///
/// ------------------------------------------------------------------------
/// Module Name: FreshImage
/// Purpose: Encapsulates data and methods for acquiring new image of galaxy
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description:See SuperScanIPS.doc, Sec xx
/// 
/// ------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using TheSkyXLib;

namespace SuperScan
{
    public class FreshImage
    {
        //statements for linking logging method LogUpdate to the main form logging routine.
        public delegate void LogEventHandler(string LogText);
        public event LogEventHandler LogUpdate;

        private string freshImagePath = "";
        private string freshImageName = "";
        private double freshImageRA = 0;
        private double freshImageDec = 0;
        private double freshImagePA = 0;
        //private double freshImageScale = 0;

        public FreshImage()
        {
            return;
        }

        public bool Acquire(string fsn, double exposure)
        //Creates a new instance of a FreshImage and sets the filepath for storing it
        {
            //SuperScanConfiguration sscf = new SuperScanConfiguration();
            //freshImagePath = sscf.FreshImagePath;
            freshImageName = fsn;
            if (!SeekGalaxy())
            {
                return false;
            }
            ShootGalaxy();
            return true;
        }

        private bool SeekGalaxy()
        //Find the coordinates of the object galaxyName and perform a slew, then CLS to it.
        {
            sky6StarChart tsx_sc = new sky6StarChart();
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            sky6RASCOMTele tsx_mt = new sky6RASCOMTele();
            sky6Raven tsx_rv = new sky6Raven();
            sky6ObjectInformation tsx_obj = new sky6ObjectInformation();
            sky6Dome tsx_dm = new sky6Dome();

            LogEntry("Finding coordinates for " + freshImageName);
            tsx_sc.Find(freshImageName);

            // Perform slew to new location before starting CLS -- TSX does not wait for dome rotation.
            tsx_obj.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
            double tRA = tsx_obj.ObjInfoPropOut;
            tsx_obj.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
            double tDec = tsx_obj.ObjInfoPropOut; ;
            //Make sure that the mount commands are synchronous
            tsx_mt.Asynchronous = 0;
            LogEntry("Initial slew to target");
            //Slew the mount and dome should follow before completion...
            tsx_mt.SlewToRaDec(tRA, tDec, freshImageName);
            //But wait for a second, anyway
            Task.Delay(5000);
            LogEntry("Precision slew (CLS) to target");
            //Now try the CLS
            try
            {
                tsx_cl.exec();
            }
            catch
            {
                LogEntry("CLS failed");
                return false;
            };
            LogEntry("CLS successful");
            return true;
        }

        private void ShootGalaxy()
        //Take an image via TSX.  Set the autosave path to the FreshImage path;
        //  Set exposureTime, Light Frame, AutoDark, No Autosave, Asynchronous
        //  then TakeImage 
        //  Wait for completion status, then return
        {
            Configuration sscf = new Configuration();
            ccdsoftImage tsx_im = new ccdsoftImage
            {
                Path = sscf.FreshImagePath
            };
            ccdsoftCamera tsx_cc = new ccdsoftCamera
            {
                AutoSaveOn = 0,          //Autosave Off
                FilterIndexZeroBased = Convert.ToInt32(sscf.Filter),
                ExposureTime = Convert.ToDouble(sscf.Exposure),
                Subframe = 0,
                Frame = ccdsoftImageFrame.cdLight,
                ImageReduction = ccdsoftImageReduction.cdAutoDark,
                Asynchronous = 1        //Asynchronous on
            };
            LogEntry("Imaging target for " + sscf.Exposure + " secs");
            tsx_cc.TakeImage();
            //Wait for completion
            while (tsx_cc.State != ccdsoftCameraState.cdStateNone)
            {
                //System.Threading.Thread.Sleep(1000);
                Task.Delay(1000);
                System.Windows.Forms.Application.DoEvents();
            }
            tsx_im.AttachToActiveImager();
            tsx_im.Save();
            freshImagePath = sscf.FreshImagePath;
            LogEntry("Imaging Complete");
            return;
        }

        public string ImagePath => freshImagePath;

        public double RA => freshImageRA;

        public double Dec => freshImageDec;

        public double PA => freshImagePA;

        //Method to link to SuperScan main form for logging progress.
        private void LogEntry(string upd)
        //Method for projecting log entry to the SuperScan Main Form
        {
            LogUpdate(upd);
            return;
        }

    }
}
