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

using TheSky64Lib;


namespace SuperScan
{
    public class FreshImage
    {
        ////statements for linking logging method LogUpdate to the main form logging routine.

        private string freshImagePath = "";
        private string freshImageName = "";
        private double freshImageRA = 0;
        private double freshImageDec = 0;
        private double freshImagePA = 0;

        public FreshImage()
        {
            return;
        }

        //Creates a new instance of a FreshImage and sets the filepath for storing it
        public bool Acquire(string fsn, double exposure)
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

        //Find the coordinates of the object galaxyName and perform a slew, then CLS to it.
        private bool SeekGalaxy()
        {
            sky6StarChart tsx_sc = new sky6StarChart();
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            sky6RASCOMTele tsx_mt = new sky6RASCOMTele();
            sky6Raven tsx_rv = new sky6Raven();
            sky6ObjectInformation tsx_obj = new sky6ObjectInformation();

            //Clear any camera set up stuff that might be hanging around
            //  and there has been some on occasion
            //Removed subframe on request for cameras with long download times
            ccdsoftCamera tsx_cc = new ccdsoftCamera();

            LogEntry("Finding coordinates for " + freshImageName);
            tsx_sc.Find(freshImageName);

            // Perform slew to new location before starting CLS -- TSX does not wait for dome rotation.
            tsx_obj.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
            double tRA = tsx_obj.ObjInfoPropOut;
            tsx_obj.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
            double tDec = tsx_obj.ObjInfoPropOut; ;
            //Make sure that the mount commands are synchronous
            tsx_mt.Asynchronous = 0;

            ////Slew the mount and dome should follow before completion...
            //Test to see if a dome tracking operation is underway.

            //If using dome, toggle dome coupling:  this appears to clear most Error 123 problems
            Configuration ss_cfg = new Configuration();
            bool hasDome = Convert.ToBoolean(ss_cfg.UsesDome);
            if (hasDome)
            {
                sky6Dome tsx_dm = new sky6Dome();
                tsx_dm.IsCoupled = 0;
                System.Threading.Thread.Sleep(1000);
                tsx_dm.IsCoupled = 1;
            }

            LogEntry("Precision slew (CLS) to target");
            Configuration cfg = new Configuration();
            int clsStatus = DeviceControl.ReliableClosedLoopSlew(tRA, tDec, freshImageName, hasDome, cfg.CLSReductionType );
            LogEntry("Precision Slew Complete:  ");
            if (clsStatus == 0)
            {
                LogEntry("    CLS successful");
                return true;
            }
            else
            {
                LogEntry("    CLS unsucessful: Error: " + clsStatus.ToString());
                return false;
            }
        }

        //Take an image via TSX.  Set the autosave path to the FreshImage path;
        //  Set exposureTime, Light Frame, AutoDark, No Autosave, Asynchronous
        //  then TakeImage 
        //  Wait for completion status, then return
        //
        //  Removed subframe setting on request for cameras with long download times

        private void ShootGalaxy()
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
                Frame = ccdsoftImageFrame.cdLight,
                Asynchronous = 1        //Asynchronous on
            };
            switch (sscf.ImageReductionType)
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
                        Reduction calLib = new Reduction();
                        string binning = Binning.GetBinning();
                        calLib.SetReductionGroup(tsx_cc.FilterIndexZeroBased, tsx_cc.ExposureTime, (int)tsx_cc.TemperatureSetPoint, binning);
                        tsx_cc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat;
                        break;
                    }
            }
            LogEntry("Imaging target for " + sscf.Exposure + " secs");
            tsx_cc.TakeImage();
            //Wait for completion
            while (tsx_cc.State != ccdsoftCameraState.cdStateNone)
            {
                System.Threading.Thread.Sleep(1000);
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
            Logger.LogToFileAndScreen(upd);
            return;
        }

    }
}
