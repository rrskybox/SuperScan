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
using TheSkyXLib;

namespace SuperScan
{
    class DeviceControl
    {
        public void TelescopeStartUp()
        {
            //Method for connecting and initializing the TSX mount
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            if (tsxm.IsConnected == 0) { tsxm.Connect(); }
            try { if (tsxm.IsParked()) { tsxm.Unpark(); } }
            catch
            { return; }
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
            tsxm.Asynchronous = 0;
            tsxm.Connect();
            if (side == "East")
            {
                tsxm.SlewToAzAlt(90.0, 80.0, "");
            }
            else
            {
                tsxm.SlewToAzAlt(270.0, 80.0, "");
            }
            return;
        }

        public void TelescopeShutDown()
        //Method for connecting and parking the TSX mount
        {
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            if (tsxm.IsConnected == 0) { tsxm.Connect(); }
            try { tsxm.Park(); }
            catch //ignor failure
            { return; }
        }

        public void CameraStartUp()
        {
            //Method for connecting and initializing the TSX camera
            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Connect();
            return;
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

        public void DomeStartUp()
        {
            //Method for connecting and initializing the TSX dome, if any
            // use exception handlers to check for dome commands, opt out if none
            //  couple the dome to telescope if everything works out
            sky6Dome tsxd = new sky6Dome();
            try { tsxd.Connect(); }
            catch { return; }
            //If a connection is set, then make sure the dome is coupled to the telescope slews
            tsxd.IsCoupled = Convert.ToInt32(true);
            return;
        }

        public void OpenDome()
        {
            //Method for opening the TSX dome
            // use exception handlers to check for dome commands, opt out if none
            //  couple the dome to telescope if everything works out
            sky6Dome tsxd = new sky6Dome();
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            //disconnect mount so dome won't keep chasing it
            tsxt.Disconnect();
            try { tsxd.Connect(); }
            catch { return; }
            //If a connection is set, then open the dome shutter
            tsxd.OpenSlit();
            System.Threading.Thread.Sleep(10000);  //Wait for close command to clear TSX and ASCOM driver
            while (tsxd.IsOpenComplete == 0)
            { System.Threading.Thread.Sleep(5000); } //five second wait loop
            //reconnect the mount
            if (tsxt.IsConnected != 0) { tsxt.Connect(); }
            return;
        }

        public void CloseDome()
        {
            //Method for closing the TSX dome
            // Mount should be parked at this time
            // use exception handlers to check for dome commands, opt out if none
            sky6Dome tsxd = new sky6Dome();
            try { tsxd.Connect(); }
            catch { return; }
            //Stop whatever the dome is doing, if any and wait a few seconds for it to clear
            try { tsxd.Abort(); }
            catch (Exception e) { return; }
            //Close up the dome:  Connect, Home (so power is to the dome), Close the slit
            if (tsxd.IsConnected == 1)
            {
                //Home the dome,wait for the command to propogate, then wait until the dome reports it is homed
                tsxd.FindHome();
                System.Threading.Thread.Sleep(10000);
                while (tsxd.IsFindHomeComplete == 0) { System.Threading.Thread.Sleep(5000); };
                //Close slit
                tsxd.CloseSlit();
                System.Threading.Thread.Sleep(10000);
                while (tsxd.IsCloseComplete == 0) { System.Threading.Thread.Sleep(5000); }
            }
            //disconnect dome controller
            tsxd.Disconnect();
        }
    }
}
