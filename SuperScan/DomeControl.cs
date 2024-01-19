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
            LogEntry("Connecting Dome");
            try { tsxd.Connect(); }
            catch { return false; }
            //If a connection is set, then make sure the dome is coupled to the telescope slews
            LogEntry("Coupling Dome");
            DeviceControl.IsDomeCoupled = true;
            LogEntry("Unparking Dome, if needed");
            System.Threading.Thread.Sleep(5000);
            DeviceControl.UnparkDome();
            System.Threading.Thread.Sleep(5000);
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
            LogEntry("Uncoupling Dome from mount (although this doen't work for tracking, yet");
            DeviceControl.IsDomeCoupled = false;
            //Disconnect the mount so the dome won't chase it
            LogEntry("Disconnecting Mount");
            DeviceControl.DisconnectMount();
            //Connect the dome, assuming it might be disconnected for some reason, if it fails, reset the connection states
            LogEntry("Connecting Dome");
            if (!DeviceControl.ConnectDome())
            {
                if (DeviceControl.IsMountConnected())
                    return false;
            }
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            LogEntry("Aborting Dome Commands");
            if (!DeviceControl.AbortDome())
            {
                if (DeviceControl.IsMountConnected())
                    return false;
            }
            //Park Dome
            LogEntry("Bringing dome to park position");
            DeviceControl.DomeParkReliably();
            //Open Slit
            LogEntry("Initiating opening dome slit");
            //Keep trying to close if failed the first time -- this is important
            while (!DeviceControl.OpenDomeSlit())
            {
                System.Threading.Thread.Sleep(5000);
                LogEntry("Attempt to start open failed.  Trying again.");
            }
            LogEntry("Dome slit open underway");
            //Give a wait to get goint
            System.Threading.Thread.Sleep(5000);
            while (!DeviceControl.IsOpenComplete())
                System.Threading.Thread.Sleep(5000);
            //Unpark the dome so it can chase the mount
            System.Threading.Thread.Sleep(5000);
            LogEntry("Unparking dome, if parked");
            DeviceControl.UnparkDome();
            System.Threading.Thread.Sleep(5000);
            while (!DeviceControl.IsUnparkComplete())
                System.Threading.Thread.Sleep(1000);
            //Enable mount chasing
            System.Threading.Thread.Sleep(5000);
            LogEntry("Coupling dome to mount");
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
            LogEntry("Disconnecting mount");
            DeviceControl.DisconnectMount();
            //Decouple the dome
            LogEntry("Uncoupling dome to mount -- except for tracking as of now");
            DeviceControl.IsDomeCoupled = false;
            //Connect dome and decouple the dome from the mount position, if it fails, reset the connection states
            LogEntry("Connecting dome, if needed");
            DeviceControl.ConnectDome();
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            LogEntry("Aborting any outstanding dome commandes");
            if (!DeviceControl.AbortDome())
                return false;
            //Park Dome
            LogEntry("Bringing dome to home/park positing and unparking there");
            DeviceControl.DomeParkReliably();
            //Close slit
            System.Threading.Thread.Sleep(5000);
            LogEntry("Initiating closing dome slit");
            //Keep trying to close if failed the first time -- this is important
            while (!DeviceControl.CloseDomeSlit())
            {
                System.Threading.Thread.Sleep(5000);
                LogEntry("Attempt to start close failed.  Trying again.");
            }
            LogEntry("Dome slit close underway");
            // Release task thread so TSX can start Close Slit -- Command in Progress exception otherwise
            System.Threading.Thread.Sleep(5000);
            // Wait for close slit competion or receive timout -- meaning that the battery has failed, probably
            while (!DeviceControl.IsCloseComplete())
                System.Threading.Thread.Sleep(5000);
            //Reset device states
            if (domeState)
                DeviceControl.ConnectDome();
            if (mountedState)
                DeviceControl.ConnectMount();
            LogEntry("Dome closing complete");
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

        private static void LogEntry(string upd)
        //Method for projecting log entry to the SuperScan Main Form
        {
            Logger lg = new Logger();
            lg.PostLogEntry(upd);
            return;
        }

    }
}


