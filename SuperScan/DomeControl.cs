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
            DeviceControl.UnparkDome();
            return true;
        }

        public static bool OpenDome()
        {
            //Method for opening the TSX dome
            //Method to open dome
            //Save mount and dome connect states
            bool mountedState = DeviceControl.IsMountConnected();
            bool domeState = DeviceControl.IsDomeConnected();
            //Make sure dome decoupled
            DeviceControl.IsDomeCoupled = false;
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
            //Slew dome to home position
            //ReliableGoToDomeAz(Convert.ToDouble(cfg.DomeHomeAz));
            //Park Dome, if not already parked
            System.Threading.Thread.Sleep(1000);
            DeviceControl.FindDomeHome();
            System.Threading.Thread.Sleep(1000);
            while (!DeviceControl.IsFindDomeHomeComplete())
                System.Threading.Thread.Sleep(1000);
            System.Threading.Thread.Sleep(1000);
            DeviceControl.ParkDome();
            while (!DeviceControl.IsParkComplete())
                System.Threading.Thread.Sleep(1000);
            System.Threading.Thread.Sleep(1000);
            //Open Slit
            DeviceControl.OpenDomeSlit();
            //Give a wait to get goint
            System.Threading.Thread.Sleep(5000);
            while (!DeviceControl.IsOpenComplete())
                System.Threading.Thread.Sleep(1000); //one second wait loop
            //Unpark the dome so it can chase the mount
            DeviceControl.UnparkDome();
            while (!DeviceControl.IsUnparkComplete())
                System.Threading.Thread.Sleep(1000);
            //Enable mount chasing
            DeviceControl.IsDomeCoupled = true;

            //Reset device states
            if (domeState)
                DeviceControl.ConnectDome();
            if (mountedState)
                DeviceControl.ConnectMount();
            return true;

        }

        public static bool CloseDome()
        {
            //Method for closing the TSX dome
            //Save mount and dome connect states
            //Note that if close dome fails, the mount is not reconnected nor the dome recoupled
            //  in case it is chasing a target below horizon
            bool mountedState = DeviceControl.IsMountConnected();
            bool domeState = DeviceControl.IsDomeConnected();
            //Disconnect the mount
            DeviceControl.DisconnectMount();
            //Decouple the dome
            DeviceControl.IsDomeCoupled = false;
            //Connect dome and decouple the dome from the mount position, if it fails, reset the connection states
            DeviceControl.ConnectDome();
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            if (!DeviceControl.AbortDome())
                return false;
            //Goto home position using goto rather than home
            //Configuration cfg = new Configuration();
            //ReliableGoToDomeAz(Convert.ToDouble(cfg.DomeHomeAz));
            System.Threading.Thread.Sleep(1000);
            DeviceControl.FindDomeHome();
            System.Threading.Thread.Sleep(1000);
            while (!DeviceControl.IsFindDomeHomeComplete())
                System.Threading.Thread.Sleep(1000);
            System.Threading.Thread.Sleep(1000);
            DeviceControl.ParkDome();
            while (!DeviceControl.IsParkComplete())
                System.Threading.Thread.Sleep(1000);
            System.Threading.Thread.Sleep(1000);
            //Close slit
            DeviceControl.CloseDomeSlit();
            // Release task thread so TSX can start Close Slit -- Command in Progress exception otherwise
            System.Threading.Thread.Sleep(5000);
            // Wait for close slit competion or receive timout -- meaning that the battery has failed, probably
            try
            {
                while (!DeviceControl.IsCloseComplete())
                    System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                return false;
            }
            //Reset device states
            if (domeState) 
                DeviceControl.ConnectDome();
            if (mountedState)
                DeviceControl.ConnectMount();
            return true;
        }

        public static bool ReliableGoToDomeAz(double az)
        {
            //Slews dome to azimuth while avoiding lockup if already there
            //  Mount will be disconnect upon return
            Configuration cfg = new Configuration();
            //Make sure the mount is disconnected
            DeviceControl.DisconnectMount();
            //Abort any other dome operations
            DeviceControl.AbortDome();
            //Decouple the dome
            DeviceControl.IsDomeCoupled = false;
            double currentAz = Convert.ToDouble(cfg.DomeHomeAz);
            if (currentAz - az > 1)
            {
                DeviceControl.GoToDomeAzm(az);
                System.Threading.Thread.Sleep(5000);
                while (!DeviceControl.IsGoToAzmComplete())
                    System.Threading.Thread.Sleep(1000);
            }
            return true;
        }
    }
}


