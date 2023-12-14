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
    public static class DomeControl
    {
        public static bool DomeStartUp()
        {
            //Method for connecting and initializing the TSX dome, if any
            // use exception handlers to check for dome commands, opt out if none
            //  couple the dome to telescope if everything works out
            sky6Dome tsxd = new sky6Dome();
            try { tsxd.Connect(); }
            catch { return false; }
            //If a connection is set, then make sure the dome is coupled to the telescope slews
            DeviceControl.IsDomeCoupled = true;
            return true;
        }

        public static bool OpenDome()
        {
            //Method for opening the TSX dome
            //Method to open dome
            Configuration cfg = new Configuration();
            //Save mount and dome connect states
            bool coupledState = DeviceControl.IsDomeCoupled;
            bool mountedState = DeviceControl.IsMountConnected();
            bool domeState = DeviceControl.IsDomeConnected();
            //Disconnect the mount so the dome won't chase it
            DeviceControl.DisconnectMount();
            //Connect the dome, assuming it might be disconnected for some reason, if it fails, reset the connection states
            if (!DeviceControl.ConnectDome())
            {
                if (DeviceControl.IsMountConnected())
                    return false;
            }
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            if (!DeviceControl.AbortDome())
            {
                if (DeviceControl.IsMountConnected())
                    return false;
            }
            //Make sure dome decoupled
            DeviceControl.IsDomeCoupled = false;
            //Slew dome to home position
            ReliableGoToDomeAz(Convert.ToDouble(cfg.DomeHomeAz));
            //Open Slit
            InitiateOpenSlit();
            System.Threading.Thread.Sleep(10);  //Workaround for race condition in TSX
            while (!DeviceControl.IsOpenComplete())
            { System.Threading.Thread.Sleep(1000); } //one second wait loop
            DeviceControl.IsDomeCoupled = true;
            //Reset device states
            if (domeState)
                DeviceControl.ConnectDome();
            if (coupledState)
                DeviceControl.IsDomeCoupled = true;
            if (mountedState)
                DeviceControl.ConnectMount();
            return true;
  
        }

        public static bool ReliableGoToDomeAz(double az)
        {
            //Slews dome to azimuth while avoiding lockup if already there
            Configuration cfg = new Configuration();
            //Disconnect the mount
            DeviceControl.DisconnectMount();
            //Abort any other dome operations
            DeviceControl.AbortDome();
            //Decouple the dome
            DeviceControl.IsDomeCoupled = false;
            double currentAz = Convert.ToDouble(cfg.DomeHomeAz);
            if (currentAz - az > 1)
            {
                InitiateDomeGoTo(az);
                System.Threading.Thread.Sleep(5000);
                while (!DeviceControl.IsGoToAzmComplete())
                    System.Threading.Thread.Sleep(1000);
            }
            return true;
        }

        public static bool CloseDome()
        {
            //Method for closing the TSX dome
            //Save mount and dome connect states
            bool coupledState = DeviceControl.IsDomeCoupled;
            bool mountedState = DeviceControl.IsMountConnected();
            bool domeState = DeviceControl.IsDomeConnected();
            //Disconnect the mount
            DeviceControl.DisconnectMount();
            //Connect dome and decouple the dome from the mount position, if it fails, reset the connection states
            if (!DeviceControl.IsDomeConnected())
            {
                if (mountedState) DeviceControl.ConnectMount();
                return false;
            }
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            if (!DeviceControl.AbortDome())
            {
                if (mountedState) DeviceControl.ConnectMount();
                return false;
            }
            //Make sure dome decoupled
            DeviceControl.IsDomeCoupled = false;
            //Goto home position using goto rather than home
            Configuration cfg = new Configuration();
            ReliableGoToDomeAz(Convert.ToDouble(cfg.DomeHomeAz));
            //Close slit
            InitiateCloseSlit();
            // Release task thread so TSX can start Close Slit -- Command in Progress exception otherwise
            System.Threading.Thread.Sleep(5000);
            // Wait for close slit competion
            while (!DeviceControl.IsCloseComplete())
                System.Threading.Thread.Sleep(1000);
            //Reset device states
            if (domeState) DeviceControl.ConnectDome();
            if (coupledState) DeviceControl.IsDomeCoupled = true;
            if (mountedState) DeviceControl.ConnectMount();
            return true;
        }

        private static void InitiateDomeGoTo(double az)
        {
            //Operation in progress == 0
            int sleepOver = 1000;
            bool failed = true;

            while (failed)
            {
                try
                {
                    failed = !DeviceControl.GoToDomeAzm(az);
                }
                catch
                {
                    //Assume goto in progress error, wait until Goto is complete
                    System.Threading.Thread.Sleep(sleepOver);
                }
            }
            return;
        }

        private static void InitiateOpenSlit()
        {
            //Operation in progress == 0
            int sleepOver = 1000;
            bool failed = true;

            while (failed)
            {
                try
                {
                    failed = !DeviceControl.OpenDomeSlit();
                }
                catch
                {
                    //Assume goto in progress error, wait until Goto is complete
                    System.Threading.Thread.Sleep(sleepOver);
                }
            }
            return;
        }

        private static void InitiateCloseSlit()
        {
            //Operation in progress == 0
            int sleepOver = 1000;
            bool failed = true;

            while (failed)
            {
                try
                {
                    failed = !DeviceControl.CloseDomeSlit();
                }
                catch
                {
                    //Assume goto in progress error, wait until Goto is complete
                    System.Threading.Thread.Sleep(sleepOver);
                }
            }
            return;
        }

    }
}


