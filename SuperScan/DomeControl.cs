using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSkyXLib;

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
            IsDomeCoupled = true;
            return true;
        }

        /// <summary>
        /// //Property for coupling the TSX dome, if any
        // use exception handlers to check for dome commands, opt out if none
        //  couple the dome to telescope if everything works out
        /// </summary>
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

        /// <summary>
        /// Opens the dome slit after rotating the dome to dome home position
        /// to align power connections
        /// </summary>
        /// <param name="domeHomeAz">Home aximuth of dome in degrees</param>
        public static void OpenDome(int domeHomeAz)
        {
            //Method for opening the TSX dome
            // use exception handlers to check for dome commands, opt out if none
            //  couple the dome to telescope if everything works out
            sky6Dome tsxd = new sky6Dome();
            //Decouple the dome from the telescope
            IsDomeCoupled = false;
            //Move dome to 20 degrees short of home position
            tsxd.GotoAzEl(domeHomeAz - 20, 0);
            //Home the dome to make sure power is supplied
            tsxd.FindHome();
            while (tsxd.IsFindHomeComplete == 0) //one second wait loop
            { System.Threading.Thread.Sleep(1000); }
            // open the dome shutter
            tsxd.OpenSlit();
            System.Threading.Thread.Sleep(10000);  //Wait for close command to clear TSX and ASCOM driver
            while (tsxd.IsOpenComplete == 0)
            { System.Threading.Thread.Sleep(5000); } //five second wait loop
             return;
        }

        /// <summary>
        /// Closes the dome slit and disconnects by
        /// decoupling the dome slews from the mount
        /// then slewing the dome to home to align power strips
        /// then closing the dome slit, leaving the dome control disconnected
        /// </summary>
        /// <param name="domeHomeAz">Azimuth of home position of dome</param>
        public static void CloseDome(int domeHomeAz)
        {
            //Method for closing the TSX dome
            // use exception handlers to check for dome commands, opt out if none
            //Park Mount, if not parked already
            sky6RASCOMTele tsxt = new sky6RASCOMTele();
            //Decouple the dome from the mount position
            IsDomeCoupled = false;
            sky6Dome tsxd = new sky6Dome();
            try { tsxd.Connect(); }
            catch { return; }
            //Stop whatever the dome is doing, if any and wait a few seconds for it to clear
            try { tsxd.Abort(); }
            catch (Exception e) { return; }
            //Close up the dome:  Connect, Home (so power is to the dome), Close the slit
            if (tsxd.IsConnected == 1)
            {
                //Move the dome to 20 degrees short of home
                tsxd.GotoAzEl(domeHomeAz - 20, 0);
                //Home the dome,wait for the command to propogate, then wait until the dome reports it is homed
                tsxd.FindHome();
                System.Threading.Thread.Sleep(10000);
                while (tsxd.IsFindHomeComplete == 0) { System.Threading.Thread.Sleep(5000); };
                //Close slit
                //Standard false stop avoidance code
                System.Threading.Thread.Sleep(5000);
                bool slitClosed = false;
                try
                {
                    tsxd.CloseSlit();
                    System.Threading.Thread.Sleep(10000);
                    while (tsxd.IsCloseComplete == 0)
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    //Report success  
                    slitClosed = true;
                }
                catch
                {
                    slitClosed = false;
                }

                //Check to see if slit got closed, if not, then try one more time
                if (!slitClosed)
                {
                    tsxd.CloseSlit();
                    System.Threading.Thread.Sleep(10000);
                    try
                    {
                        while (tsxd.IsCloseComplete == 0)
                        {
                            System.Threading.Thread.Sleep(5000);
                        }
                        //Report success  
                    }
                    catch
                    {
                    }
                }
            }
            //disconnect dome controller
            tsxd.Disconnect();
        }
    }
}
