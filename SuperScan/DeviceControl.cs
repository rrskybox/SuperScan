// --------------------------------------------------------------------------------
// ExoScan module
//
// Description:	
//
// Environment:  Windows 10 executable, 32 and 64 bit
//
// Usage:        TBD
//
// Author:		(REM) Rick McAlister, rrskybox@yahoo.com
//
// Edit Log:     Rev 1.0 Initial Version
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 
// ---------------------------------------------------------------------------------
//

using System;
using System.Data;
using System.Threading;
using TheSky64Lib;

namespace SuperScan


{
    internal static class DeviceControl
    {
        const int sleepover = 1000;

        #region mount
        public static bool MountConnect()
        {
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            tsxt.Connect();
            return Convert.ToBoolean(tsxt.IsConnected);
        }

        public static bool MountDisconnect()
        {
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            tsxt.Disconnect();
            return Convert.ToBoolean(tsxt.IsConnected);
        }

        public static bool IsMountConnected()
        {
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            return Convert.ToBoolean(tsxt.IsConnected);
        }

        public static void MountReliableSlew(double RA, double Dec, string name, bool hasDome)
        {
            //
            //Checks for dome tracking underway, waits half second if so -- doesn't solve race condition, but may avoid 
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            if (hasDome)
            {
                while (IsDomeTrackingUnderway())
                    System.Threading.Thread.Sleep(500);
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

        public static int MountReliableCLS(double RA, double Dec, string name, bool hasDome, string reductionType)
        {
            //Tries to perform CLS without running into dome tracking race condition
            //
            //First set camera for image reduction
            //Then slew to the target ra/dec and pull the dome with you
            //  while waiting for the dome tracking to catch up.
            //Then decouple the dome and run the CLS which should
            //  be very close to the initial slew.

            ccdsoftCamera tsxc = new ccdsoftCamera();
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            switch (reductionType)
            {
                case "None":
                    { tsxc.ImageReduction = ccdsoftImageReduction.cdNone; break; }
                case "2":
                    { tsxc.ImageReduction = ccdsoftImageReduction.cdAutoDark; break; }
                case "3":
                    { tsxc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat; break; }
            }

            MountReliableSlew(RA, Dec, name, hasDome);
            int clsStatus = 123;
            //If dome, Turn off tracking
            if (hasDome)
            {
                AllDomeCouplingOff();
                while (clsStatus == 123)
                {
                    try { clsStatus = tsx_cl.exec(); }
                    catch (Exception ex)
                    {
                        clsStatus = ex.HResult - 1000;
                    };
                    if (clsStatus == 123) System.Threading.Thread.Sleep(500);
                }
                AllDomeCouplingOn();
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

        public static bool MountFindHome()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            try { tsxm.FindHome(); }
            catch { return false; };
            return true;
        }

        public static bool MountPark()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            try { tsxm.Park(); }
            catch { return false; };
            return true;
        }

        public static bool MountUnpark()
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            tsxm.Connect();
            try { tsxm.Unpark(); }
            catch { return false; };
            return true;
        }

        public static bool MountStartUp()
        {
            //Method for connecting and unparking the TSX mount,
            // leaving it connected
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            if (tsxm.IsConnected == 0)
                tsxm.Connect();
            //If parked, try to unpark, if fails return false
            try
            { if (tsxm.IsParked()) tsxm.Unpark(); }
            catch { return false; }
            //Otherwise return true;
            return true;
        }

        public static void MountPrePosition(string side)
        {
            // Directs the mount to point either to the "East" or "West" side of the 
            // meridian at a location of 80 degrees altitude.  Used for autofocus routine
            // and for starting off the target search
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            tsxm.Asynchronous = 0;
            tsxm.Connect();
            if (side == "East")
            {
                tsxm.SlewToAzAlt(90.0, 80.0, "");
                while (tsxm.IsSlewComplete == 0)
                    System.Threading.Thread.Sleep(1000);
            }
            else
            {
                tsxm.SlewToAzAlt(270.0, 80.0, "");
                while (tsxm.IsSlewComplete == 0)
                    System.Threading.Thread.Sleep(1000);
            }
            return;
        }

        public static bool MountShutDown()
        //Method for connecting and parking and disconnecting the TSX mount
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            if (tsxm.IsConnected == 0)
                tsxm.Connect();
            try
            {
                tsxm.Park();
                tsxm.Disconnect();
            }
            catch
            { return false; }
            return true;
        }

        public static int ReliableClosedLoopSlew(double RA, double Dec, string name, bool hasDome, string reductionType)
        {
            //Tries to perform CLS without running into dome tracking race condition
            //
            //First set camera for image reduction
            //Then slew to the target ra/dec and pull the dome with you
            //  while waiting for the dome tracking to catch up.
            //Then decouple the dome and run the CLS which should
            //  be very close to the initial slew.

            ccdsoftCamera tsxc = new ccdsoftCamera();
            ClosedLoopSlew tsx_cl = new ClosedLoopSlew();
            switch (reductionType)
            {
                case "None":
                    { tsxc.ImageReduction = ccdsoftImageReduction.cdNone; break; }
                case "2":
                    { tsxc.ImageReduction = ccdsoftImageReduction.cdAutoDark; break; }
                case "3":
                    { tsxc.ImageReduction = ccdsoftImageReduction.cdBiasDarkFlat; break; }
            }

            ReliableRADecSlew(RA, Dec, name, hasDome);
            int clsStatus = 123;
            //If dome, Turn off tracking
            if (hasDome)
            {
                AllDomeCouplingOff();
                while (clsStatus == 123)
                {
                    try { clsStatus = tsx_cl.exec(); }
                    catch (Exception ex)
                    {
                        clsStatus = ex.HResult - 1000;
                    };
                    if (clsStatus == 123) System.Threading.Thread.Sleep(500);
                }
                AllDomeCouplingOn();
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

        public static void ReliableRADecSlew(double RA, double Dec, string name, bool hasDome)
        {
            //
            //Checks for dome tracking underway, waits half second if so -- doesn't solve race condition, but may avoid 
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            if (hasDome)
            {
                while (IsDomeTrackingUnderway())
                    System.Threading.Thread.Sleep(500);
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


        #endregion

        #region focuser

        public static bool FocuserStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try { tsxc.focConnect(); }
            catch { return false; }
            return true;
        }

        #endregion

        #region rotator

        public static bool RotatorStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try { tsxc.rotatorConnect(); }
            catch { return false; }
            return true;
        }

        public static bool HasRotator()
        {
            //Returns true if a rotator is connected, else false
            ccdsoftCamera tsxc = new ccdsoftCamera();
            if (tsxc.rotatorIsConnected() == 1) //rotator is present and powered up
                return true;
            else return false;
        }

        #endregion

        #region camera

        public static bool CameraStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try
            { tsxc.Disconnect(); }
            catch
            { return false; }
            try
            { tsxc.Connect(); }
            catch
            { return false; }
            return true;
        }

        public static bool CameraConnect()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try
            { tsxc.Connect(); }
            catch
            { return false; }
            return true;
        }

        public static bool CameraDisconnect()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try
            { tsxc.Disconnect(); }
            catch
            { return false; }
            return true;
        }

        public static double CameraTemperature
        {
            //Camera temperature property for TSX
            get
            {
                ccdsoftCamera tsxc = new ccdsoftCamera();
                return tsxc.Temperature;
            }
            set
            {
                ccdsoftCamera tsxc = new ccdsoftCamera();
                tsxc.TemperatureSetPoint = value;
                tsxc.RegulateTemperature = 1;
            }
        }

        public static void CameraSetAsynchronous(bool state)
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Asynchronous = Convert.ToInt32(state);
        }

        public static void CameraSetExposure(double exp)
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.ExposureTime = exp;
        }

        public static void CameraSetDelay(double delay)
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Delay = delay;
        }

        public static void CameraSetFrame(ccdsoftImageFrame frame)
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Frame = frame;
        }

        public static void CameraSetFilter(int filter)
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.FilterIndexZeroBased = filter;
        }

        public static void CameraSetReductionType(ccdsoftImageReduction reductionType)
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.ImageReduction = reductionType;
        }

        public static void CameraInitialize(bool async, double exp, double delay, ccdsoftImageFrame frameType, ccdsoftImageReduction reductionType)
        {
            ccdsoftCamera tsx_cc = new ccdsoftCamera()
            {
                Asynchronous = Convert.ToInt32(async),
                ExposureTime = exp,
                Delay = delay,
                Frame = frameType,
                ImageReduction = reductionType
            };
            return;
        }

        public static double? ImageCurrentPA()
        {
            //Will return Image PA of most recent image link
            double? currentImagePA = null;
            if (!HasRotator())
                return currentImagePA;
            ImageLinkResults ilr = new ImageLinkResults();
            try
            { currentImagePA = ilr.imagePositionAngle; }
            catch
            { return currentImagePA; }
            return currentImagePA;
        }

        //public static bool ImageRotateToPA(double imagePA)
        //{
        //    //Move the rotator to a position that gives an image position angle of tImagePA
        //    //  Assumes that the rotator position angle variables are current
        //    //Returns false if failure, true if good

        //    if (!HasRotator())
        //        return false;
        //    ccdsoftCamera tsxc = new ccdsoftCamera();
        //    //target rotation PA = current image PA + current rotator PA - target image PA 

        //    double rotatorPA = tsxc.rotatorPositionAngle();
        //    double destRotationPA = ((double)ImageCurrentPA() - destinationIPA) + rotatorPA;
        //    double destRotationPAnormalized = AstroMath.Transform.NormalizeDegreeRange(destRotationPA);
        //    tsxc.rotatorGotoPositionAngle(destRotationPAnormalized);
        //    return true;
        //}
        #endregion

        #region dome

        public static bool DomeConnect()
        {
            sky6Dome tsxd = new sky6Dome();
            tsxd.Connect();
            return Convert.ToBoolean(tsxd.IsConnected);
        }

        public static bool DomeDisconnect()
        {
            sky6Dome tsxd = new sky6Dome();
            tsxd.Disconnect();
            return Convert.ToBoolean(tsxd.IsConnected);
        }

        public static bool IsDomeConnected()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsConnected);
        }

        public static bool DomeAbort()
        {
            sky6Dome tsxd = new sky6Dome();
            tsxd.Abort();
            Thread.Sleep(1000);
            return true;
        }

        private static bool IsDomeTrackingUnderway()
        {
            //Test to see if a dome tracking operation is underway.
            // If so, doing a IsGotoComplete will throw an Error 212.
            // return true
            // otherwise return false

            sky6Dome tsxd = new sky6Dome();
            int testDomeTrack;
            try { testDomeTrack = tsxd.isCoupledToMountTracking(); }
            catch { return true; }
            if (testDomeTrack == 0) return true;
            else return false;
        }

        public static void WaitIfDomeRotating()
        {
            //Returns true if the dome isn't "motionless"
            //  This is a workaround for catching dome tracking activity to
            //  avoid an Error 123.
            sky6Dome tsxd = new sky6Dome();
            int lastAzLocation = -1;
            int currentAzLocation = 0;
            while (currentAzLocation != lastAzLocation)
            {
                Thread.Sleep(sleepover);
                lastAzLocation = currentAzLocation;
                tsxd.GetAzEl();
                currentAzLocation = (int)tsxd.dAz;
            };
        }

        public static void AllDomeCouplingOn()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)

            sky6Dome tsxd = new sky6Dome();
            tsxd.setIsCoupledToMountTracking(Convert.ToInt32(true));
            tsxd.IsCoupled = Convert.ToInt32(true);
            System.Threading.Thread.Sleep(500);
            while (IsDomeTrackingUnderway())
                System.Threading.Thread.Sleep(1000);
            return;
        }

        public static void AllDomeCouplingOff()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            tsxd.setIsCoupledToMountTracking(Convert.ToInt32(false));
            tsxd.IsCoupled = Convert.ToInt32(false);
        }

        public static bool IsDomeCoupledAtAll
        {
            get
            {
                sky6Dome tsxd = new sky6Dome();
                return (Convert.ToBoolean(tsxd.IsCoupled) || Convert.ToBoolean(tsxd.isCoupledToMountTracking()));
            }
            set
            {
                sky6Dome tsxd = new sky6Dome();
                tsxd.IsCoupled = Convert.ToInt32(value);
                tsxd.setIsCoupledToMountTracking(Convert.ToInt32(value));
                return;
            }
        }

        public static bool DomeAbortCommand()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.Abort();
                System.Threading.Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            //Wait for abort command to clear
            return true;
        }

        public static bool DomeGoTo(double az)
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.GotoAzEl(az, 0);
                System.Threading.Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool DomeIsGoToComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsGotoComplete);
        }

        public static bool DomeOpen()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.OpenSlit();
                System.Threading.Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsDomeOpenComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsOpenComplete);
        }

        public static bool DomeClose()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.CloseSlit();
                System.Threading.Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            return true;

        }

        public static bool IsDomeCloseComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsCloseComplete);
        }

        public static bool DomeFindHome()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.FindHome();
                System.Threading.Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool? IsDomeFindHomeComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            int fhStatus = 0;
            try { fhStatus = tsxd.IsFindHomeComplete; }
            catch { return null; }
            return Convert.ToBoolean(fhStatus);
        }

        public static bool DomePark()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.Park();
                Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsDomeParkComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsParkComplete);
        }

        public static bool DomeReliablePark()
        {
            if (!DomePark())
                //try one more time
                DomePark();
            //wait for complete
            try
            {
                while (!IsDomeParkComplete())
                    Thread.Sleep(1000);
            }
            catch { return false; }
            return true;
        }

        public static bool DomeUnpark()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.Unpark();
                System.Threading.Thread.Sleep(sleepover);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsDomeUnparkComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsUnparkComplete);
        }

        public static void DomeParkReliably()
        {
            const int domeBackUp = 20;

            sky6Dome tsxd = new sky6Dome();
            DomeCommandStatus dc = new DomeCommandStatus();
            AllDomeCouplingOff();
            tsxd.Abort();
            DomeDriverPropagationTimeout();
            tsxd.GetAzEl();
            tsxd.GotoAzEl(tsxd.dAz - domeBackUp, 0);
            while (!(bool)new DomeCommandStatus().GoToDone)
                DomeDriverWaitTimeout();
            DomeDriverPropagationTimeout();
            try { tsxd.FindHome(); } catch { }
            DomeDriverPropagationTimeout();
            while (!(bool)new DomeCommandStatus().HomeDone)
                DomeDriverWaitTimeout();

            DomeDriverPropagationTimeout();
            dc = new DomeCommandStatus();
            tsxd.GetAzEl();
            tsxd.GotoAzEl(tsxd.dAz - domeBackUp, 0);
            DomeDriverPropagationTimeout();
            //while (!Convert.ToBoolean(tsxd.IsGotoComplete))
            while (!(bool)new DomeCommandStatus().GoToDone)
                DomeDriverWaitTimeout();
            //Park
            DomeDriverPropagationTimeout();
            tsxd.Park();
            DomeDriverPropagationTimeout();
            dc = new DomeCommandStatus();
            while (!(bool)new DomeCommandStatus().ParkDone)
            {
                DomeDriverWaitTimeout();
                dc = new DomeCommandStatus();
            }

        }

        public static void DomeHomeReliably()
        {
            const int domeBackUp = 20;

            sky6Dome tsxd = new sky6Dome();
            DomeCommandStatus dc = new DomeCommandStatus();
            AllDomeCouplingOff();
            tsxd.Abort();
            DomeDriverPropagationTimeout();
            tsxd.GetAzEl();
            tsxd.GotoAzEl(tsxd.dAz - domeBackUp, 0);
            while (!(bool)new DomeCommandStatus().GoToDone)
                DomeDriverWaitTimeout();
            DomeDriverPropagationTimeout();
            try { tsxd.FindHome(); } catch { }
            DomeDriverPropagationTimeout();
            while (!(bool)new DomeCommandStatus().HomeDone)
                DomeDriverWaitTimeout();
            DomeDriverPropagationTimeout();
        }

        private static void DomeDriverPropagationTimeout() => System.Threading.Thread.Sleep(5000);

        private static void DomeDriverWaitTimeout() => System.Threading.Thread.Sleep(1000);

        public class DomeCommandStatus
        {
            public bool? GoToDone = null;
            public bool? OpenDone = null;
            public bool? CloseDone = null;
            public bool? ParkDone = null;
            public bool? UnparkDone = null;
            public bool? HomeDone = null;

            public DomeCommandStatus()
            {
                sky6Dome tsxd = new sky6Dome();
                try { GoToDone = Convert.ToBoolean(tsxd.IsGotoComplete); }
                catch { }
                try { OpenDone = Convert.ToBoolean(tsxd.IsOpenComplete); }
                catch { }
                try { CloseDone = Convert.ToBoolean(tsxd.IsCloseComplete); }
                catch { }
                try { ParkDone = Convert.ToBoolean(tsxd.IsParkComplete); }
                catch { }
                try { UnparkDone = Convert.ToBoolean(tsxd.IsUnparkComplete); }
                catch { }
                try { HomeDone = Convert.ToBoolean(tsxd.IsFindHomeComplete); }
                catch { }
            }
        }

    }

    #endregion

}


