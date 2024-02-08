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
            DeviceControl.AllDomeCouplingOn();
            LogEntry("Unparking Dome, if needed");
            System.Threading.Thread.Sleep(5000);
            DeviceControl.DomeUnpark();
            System.Threading.Thread.Sleep(5000);
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
            LogEntry("Uncoupling Dome from mount ");
            DeviceControl.AllDomeCouplingOff();
            //Disconnect the mount so the dome won't chase it
            LogEntry("Disconnecting Mount");
            DeviceControl.MountDisconnect();
            //Connect the dome, assuming it might be disconnected for some reason, if it fails, reset the connection states
            LogEntry("Connecting Dome");
            if (!DeviceControl.DomeConnect())
            {
                if (DeviceControl.IsMountConnected())
                    return false;
            }
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            LogEntry("Aborting Dome Commands");
            if (!DeviceControl.DomeAbort())
            {
                if (DeviceControl.IsMountConnected())
                    return false;
            }
            //Home Dome
            LogEntry("Bringing dome to home position");
            DeviceControl.DomeHomeReliably();
            //Open Slit
            LogEntry("Initiating opening dome slit");
            //Keep trying to close if failed the first time -- this is important
            while (!DeviceControl.DomeOpen())
            {
                System.Threading.Thread.Sleep(5000);
                LogEntry("Attempt to start open failed.  Trying again.");
            }
            LogEntry("Dome slit open underway");
            //Give a wait to get goint
            System.Threading.Thread.Sleep(5000);
            while (!DeviceControl.IsDomeOpenComplete())
                System.Threading.Thread.Sleep(5000);
            //Unpark the dome so it can chase the mount
            System.Threading.Thread.Sleep(5000);
            LogEntry("Unparking dome, if parked");
            DeviceControl.DomeUnpark();
            System.Threading.Thread.Sleep(5000);
            while (!DeviceControl.IsDomeUnparkComplete())
                System.Threading.Thread.Sleep(1000);
            //Enable mount chasing
            System.Threading.Thread.Sleep(5000);
            LogEntry("Coupling dome to mount");
            DeviceControl.AllDomeCouplingOn();

            //Reset device states
            if (domeState)
                DeviceControl.DomeConnect();
            if (mountedState)
                DeviceControl.MountConnect();
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
            DeviceControl.MountDisconnect();
            //Decouple the dome
            LogEntry("Uncoupling dome to mount -- except for tracking as of now");
            DeviceControl.AllDomeCouplingOff();
            //Connect dome and decouple the dome from the mount position, if it fails, reset the connection states
            LogEntry("Connecting dome, if needed");
            DeviceControl.DomeConnect();
            //Stop whatever the dome might have been doing, if it fails, reset the connection states
            LogEntry("Aborting any outstanding dome commandes");
            if (!DeviceControl.DomeAbort())
                return false;
            //Park Dome
            //Home Dome
            LogEntry("Bringing dome to home position");
            DeviceControl.DomeHomeReliably();
            //Close slit
            LogEntry("Initiating closing dome slit");
            //Keep trying to close if failed the first time -- this is important
            System.Threading.Thread.Sleep(5000);
            DeviceControl.DomeClose();
            LogEntry("Dome slit close underway");
            System.Threading.Thread.Sleep(5000);
             // Wait for close slit competion or receive timout -- meaning that the battery has failed, probably
           while (!(bool)DeviceControl.IsDomeCloseComplete())
                 System.Threading.Thread.Sleep(5000);
            System.Threading.Thread.Sleep(5000);
            LogEntry("Dome slit close complete");
            //Reset device states
            if (domeState)
                DeviceControl.DomeConnect();
            if (mountedState)
                DeviceControl.MountConnect();
            LogEntry("Dome closing complete");
            return true;
        }

        private static void LogEntry(string upd)
        //Method for projecting log entry to the SuperScan Main Form
        {
            Logger.LogToFileAndScreen(upd);
            return;
        }

    }
}


