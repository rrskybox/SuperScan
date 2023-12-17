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
using TheSky64Lib;

namespace SuperScan

{
    internal static class DeviceControl
    {
        public static bool TelescopeStartUp()
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

        public static void TelescopePrePosition(string side)
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

        public static bool TelescopeShutDown()
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

        public static bool CameraStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try
            { tsxc.Connect(); }
            catch
            { return false; }
            return true;
        }

        public static bool FocuserStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            try { tsxc.focConnect(); }
            catch { return false; }
            return true;
        }

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

        public static void SetCameraTemperature(double settemp)
        {
            //Method for setting TSX camera temp
            const int temperatureSettlingRange = 5; //in percent
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.TemperatureSetPoint = settemp;
            tsxc.RegulateTemperature = 1;
            while (!TTUtility.CloseEnough(tsxc.Temperature, settemp, temperatureSettlingRange))
            {
                System.Threading.Thread.Sleep(1000);
            };
            return;
        }

        public static void ReliableRADecSlew(double RA, double Dec, string name, bool hasDome)
        {
            //
            //Checks for dome tracking underway, waits half second if so -- doesn't solve race condition, but may avoid 
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

        public static int ReliableClosedLoopSlew(double RA, double Dec, string name, bool hasDome, string reductionType)
        {
            //Tries to perform CLS without running into dome tracking race condition
            //
            //First set camera for image reduction
            ccdsoftCamera tsxc = new ccdsoftCamera();
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

        private static bool IsDomeTrackingUnderway()
        {
            //Test to see if a dome tracking operation is underway.
            // If so, doing a IsGotoComplete will throw an Error 212.
            // return true
            // otherwise return false
            return false;

            //sky6Dome tsxd = new sky6Dome();
            //int testDomeTrack;
            //try { testDomeTrack = tsxd.IsGotoComplete; }
            //catch { return true; }
            //if (testDomeTrack == 0) return true;
            //else return false;
        }

        public static void ToggleDomeCoupling()
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

        public static void DomeCouplingOn()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)

            sky6Dome tsxd = new sky6Dome();
            tsxd.IsCoupled = 1;
            System.Threading.Thread.Sleep(500);
            while (IsDomeTrackingUnderway())
            { System.Threading.Thread.Sleep(1000); }
            return;
        }

        public static void DomeCouplingOff()
        {
            //Uncouple dome tracking, then recouple dome tracking (synchronously)
            sky6Dome tsxd = new sky6Dome();
            tsxd.IsCoupled = 0;
            System.Threading.Thread.Sleep(500);
            while (IsDomeTrackingUnderway()) { System.Threading.Thread.Sleep(1000); }
            return;
        }

        public static double? CurrentIPA()
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

        public static bool RotateToImagePA(double destinationIPA)
        {
            //Move the rotator to a position that gives an image position angle of tImagePA
            //  Assumes that the rotator position angle variables are current
            //Returns false if failure, true if good

            if (!HasRotator())
                return false;
            ccdsoftCamera tsxc = new ccdsoftCamera();
            //target rotation PA = current image PA + current rotator PA - target image PA 

            double rotatorPA = tsxc.rotatorPositionAngle();
            double destRotationPA = ((double)CurrentIPA() - destinationIPA) + rotatorPA;
            double destRotationPAnormalized = AstroMath.Transform.NormalizeDegreeRange(destRotationPA);
            tsxc.rotatorGotoPositionAngle(destRotationPAnormalized);
            return true;
        }

        public static bool ConnectMount()
        {
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            tsxt.Connect();
            return Convert.ToBoolean(tsxt.IsConnected);
        }

        public static bool ConnectDome()
        {
            sky6Dome tsxd = new sky6Dome();
            tsxd.Connect();
            return Convert.ToBoolean(tsxd.IsConnected);
        }

        public static bool DisconnectMount()
        {
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            tsxt.Disconnect();
            return Convert.ToBoolean(tsxt.IsConnected);
        }

        public static bool DiscnnectDome()
        {
            sky6Dome tsxd = new sky6Dome();
            tsxd.Disconnect();
            return Convert.ToBoolean(tsxd.IsConnected);
        }

        public static bool IsMountConnected()
        {
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            return Convert.ToBoolean(tsxt.IsConnected);
        }

        public static bool IsDomeConnected()
        {
            sky6Dome tsxd = new sky6Dome();
            return Convert.ToBoolean(tsxd.IsConnected);
        }

        public static bool IsDomeCoupled
        {
            get
            {
                sky6Dome tsxd = new sky6Dome();
                try { tsxd.Connect(); }
                catch { return false; }
                int cState = tsxd.IsCoupled;
                if (cState == 0) return false;
                else return (true); ;
            }
            set
            {
                sky6Dome tsxd = new sky6Dome();
                try { tsxd.Connect(); }
                catch { return; }
                //If a connection is set, then make sure the dome is coupled to the telescope slews
                tsxd.IsCoupled = Convert.ToInt32(true);
                return;
            }
        }

        public static bool AbortDome()
        {
            sky6Dome tsxd = new sky6Dome();
            try { tsxd.Abort(); }
            catch { return false; }
            System.Threading.Thread.Sleep(1000);  //Wait for abort command to clear
            return true;
        }

        public static bool GoToDomeAzm(double az)
        {
            sky6Dome tsxd = new sky6Dome();
            try
            { tsxd.GotoAzEl(az, 0); }
            catch
            { return false; }
            return true;
        }

        public static bool IsGoToAzmComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsGotoComplete == 1)
                return true;
            else
                return false;
        }

        public static bool OpenDomeSlit()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.OpenSlit();
                System.Threading.Thread.Sleep(10);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsOpenComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsOpenComplete == 1)
                return true;
            else
                return false;
         }

        public static bool CloseDomeSlit()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.CloseSlit();
                System.Threading.Thread.Sleep(10);
            }
            catch
            {
                return false;
            }
            return true;

        }

        public static bool IsCloseComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsCloseComplete == 1)
                return true;
            else
                return false;
        }

        public static bool ParkDome()
        {
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.Park();
                System.Threading.Thread.Sleep(10);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsParkComplete()
        {
            sky6Dome tsxd = new sky6Dome();
            if (tsxd.IsParkComplete == 1)
                return true;
            else
                return false;
        }

    }
}

