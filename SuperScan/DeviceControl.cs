// Device Control Class
///
/// ------------------------------------------------------------------------
/// Module Name: DeviceControl
/// Purpose: Encapsulate data and methods for managing TSX for a SuperScan run
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description:  this class is intended to provide hardware specific management.
/// One method initializes the scope.  The other method initializes the camera.
/// 
/// ------------------------------------------------------------------------
using System;
#if ISTSX32
using TheSkyXLib;
#endif
#if ISTSX64
using TheSky64Lib;
#endif

namespace SuperScan
{
    class DeviceControl
    {
        public bool TelescopeStartUp()
        {
            //Method for connecting and initializing the TSX mount
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            if (tsxm.IsConnected == 0) tsxm.Connect();
            //If parked, try to unpark, if fails return false
            try { if (tsxm.IsParked()) tsxm.Unpark(); }
            catch (Exception ex) { return false; }
            //Otherwise return true;
            return true;
        }

        /// <summary>
        /// TelescopePrePosition(side)
        /// Directs the mount to point either to the "East" or "West" side of the 
        /// meridian at a location of 80 degrees altitude.  Used for autofocus routine
        /// and for starting off the galaxy search
        /// </summary>
        /// <param name="side"></param>
        public void TelescopePrePosition(string side)
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            //DeviceControl dctl = new DeviceControl();
            tsxm.Asynchronous = 0;
            tsxm.Connect();
            //dctl.DomeTrackingOff();
            if (side == "East")
            {
                tsxm.SlewToAzAlt(90.0, 80.0, "");
            }
            else
            {
                tsxm.SlewToAzAlt(270.0, 80.0, "");
            }
            //dctl.DomeTrackingOn();
            return;
        }

        public bool TelescopeShutDown()
        //Method for connecting and parking the TSX mount
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            if (tsxm.IsConnected == 0) { tsxm.Connect(); }
            try { tsxm.Park(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool CameraStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try { tsxc.Connect(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public void SetCameraTemperature(double settemp)
        {
            //Method for setting TSX camera temp
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.TemperatureSetPoint = settemp;
            tsxc.RegulateTemperature = 1;
            while (!TTUtility.CloseEnough(tsxc.Temperature, settemp, 90))
            {
                System.Threading.Thread.Sleep(1);
            };
            return;
        }

        public void SetCLSSettings()
        {
            //Sets reduction and scale for image linking, CLS or T-Point
            ccdsoftCamera tsx_cc = new ccdsoftCamera();
            AutomatedImageLinkSettings ails = new AutomatedImageLinkSettings();
            Configuration sscf = new Configuration();
            //Full reduction is not an option because the automated image link settings binning cannot be read
            switch (sscf.CLSReductionType)
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
                //case "Full":
                //    {
                //        Reduction calLib = new Reduction();
                //        string binning = Binning.GetBinning();
                //        calLib.SetReductionGroup(tsx_cc.FilterIndexZeroBased, ails.exposureTimeAILS , (int)tsx_cc.TemperatureSetPoint, binning);
                //        tsx_cc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat;
                //        break;
                //    }
            }
            return;
        }

        public void ReliableRADecSlew(double RA, double Dec, string name, bool hasDome)
        {
            //
            //Checks for dome tracking underway, waits half second if so -- doesn//t solve race condition, but may avoid 
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            if (hasDome)
            {
                while (IsDomeTrackingUnderway()) System.Threading.Thread.Sleep(500);
                int result = -1;
                while (result != 0)
                {
                    result = 0;
                    try { tsxt.SlewToRaDec(RA, Dec, name); }
                    catch (Exception ex) { result = ex.HResult - 1000; }
                }
            }
            else tsxt.SlewToRaDec(RA, Dec, name);
            return;
        }

        public int ReliableClosedLoopSlew(double RA, double Dec, string name, bool hasDome)
        {
            //Tries to perform CLS without running into dome tracking race condition
            //
            ImageLink tsxil = new ImageLink();
            //Locate target using a standard slew first to avoid "Dome Command In Progress" error from TSX
            ReliableRADecSlew(RA, Dec, name, hasDome);

            SetCLSSettings();

            //Now do a CLS
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            int clsStatus = 123;
            //If dome, Turn off tracking
            if (hasDome)
            {
                DomeCouplingOff();
                while (clsStatus == 123)
                {
                    try { clsStatus = tsx_cl.exec(); }
                    catch (Exception ex)
                    {
                        clsStatus = ex.HResult - 1000;
                    };
                    if (clsStatus == 123) System.Threading.Thread.Sleep(500);
                }
                DomeCouplingOn();
            }
            else
            {
                try { clsStatus = tsx_cl.exec(); }
                catch (Exception ex)
                {
                    clsStatus = ex.HResult - 1000;
                };
            }
            return clsStatus;
        }

        private bool IsDomeTrackingUnderway()
        {
            //Test to see if a dome tracking operation is underway.
            // If so, doing a IsGotoComplete will throw an Error 212.
            // return true
            // otherwise return false
            sky6Dome tsxd = new sky6Dome();
            int testDomeTrack;
            try { testDomeTrack = tsxd.IsGotoComplete; }
            catch { return true; }
            if (testDomeTrack == 0) return true;
            else return false;
        }

        public void ToggleDomeCoupling()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            tsxd.IsCoupled = 0;
            System.Threading.Thread.Sleep(1000);
            tsxd.IsCoupled = 1;
            //Wait for all dome activity to stop
            while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            return;
        }

        public void DomeCouplingOn()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)

            sky6Dome tsxd = new sky6Dome();
            tsxd.IsCoupled = 1;
            System.Threading.Thread.Sleep(500);
            while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            return;
        }

        public void DomeCouplingOff()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            tsxd.IsCoupled = 0;
            System.Threading.Thread.Sleep(500);
            while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            return;
        }

    }
}
