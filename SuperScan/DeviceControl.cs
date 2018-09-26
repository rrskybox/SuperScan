﻿// Device Control Class
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSkyXLib;

namespace SuperScan
{
    class DeviceControl
    {
        public void TelescopeStartUp()
        {
            //Method for connecting and initializing the TSX mount
            sky6RASCOMTele tsxm = new sky6RASCOMTele();
            tsxm.Connect();
            try
            {
                //tsxm.FindHome();
            }
            catch
            {
                //ignor failures -- probably just not a Paramount} 
                return;
            }
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
            tsxm.Connect();
            try
            {
                tsxm.Park();
            }
            catch
            {
                //ignor failure 
                return;
            }
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
            try
            {
                tsxd.Connect();
            }
            catch
            {
                return;
            }
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
            try
            {
                tsxd.Connect();
            }
            catch
            {
                return;
            }
            //If a connection is set, then open the dome shutter
            tsxd.OpenSlit();
            System.Threading.Thread.Sleep(10000);  //Wait for close command to clear TSX and ASCOM driver
            while (tsxd.IsOpenComplete == 0)
            { System.Threading.Thread.Sleep(1000); } //one second wait loop
            tsxt.Connect();
            return;
        }

        public void CloseDome()
        {
            //Method for closing the TSX dome
            // use exception handlers to check for dome commands, opt out if none
            //  couple the dome to telescope if everything works out
            sky6Dome tsxd = new sky6Dome();
            try
            {
                tsxd.Connect();
            }
            catch
            {
                return;
            }
            //If a connection is set, then close the dome shutter
            tsxd.CloseSlit();
            System.Threading.Thread.Sleep(10000);  //Wait for close command to clear TSX and ASCOM driver
            while (tsxd.IsCloseComplete == 0)
            { System.Threading.Thread.Sleep(1000); } //one second wait loop
            return;
        }
    }
}
