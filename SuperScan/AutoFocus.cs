using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSkyXLib;

namespace SuperScan
{
    public static class AutoFocus
    {
        //private DateTime afStartTime;
        public static double afLastTemp = -100;

        //Autofocus manages the TSX functions to refocus the camera
        // every change of 1 degree in temperature.
        //The first fime autofocus is called, the telescope is slewed to 
        // a position with Az = 90, Alt = 80.  Then @Focus2 is called with
        // TSX providing the star to use.  the temperature at that time is recorded.
        //Subsequent calls to autofocus check to see if the current focuser temperature
        //  is more than a degree celsius different from the last @autofocus2 time.
        //  if so, @autofocus2 is called again, although the telescope is not slewed.  And so on.

        public static string Check()
        {
               //check to see if current temperature is a degree different from last temperature
            //  If so, then set up and run @focus2
            //AtFocus2 chooses to use a 15 degree x 15 degree field of view to choose a focus star
            //  If the current position is close to the meridian then a focus star on the other
            //  side of the meridian can be choosen and the mount will flip trying to get to it
            //  and, if using a dome, the slew does not wait for the dome slit to catch up (CLS flaw)
            //  so not only will an exception be thrown (Dome command in progress Error 125) the first image
            //   will be crap and the focus fail (as of DB 11360).  So, this method will point the mount to a
            //  altitude that is no more than 80 degrees at the same azimuth of the current position in order
            //  to avoid a flip and subsequent bullshit happening

            ccdsoftCamera tsxc = new ccdsoftCamera();
            tsxc.Connect();
            double currentTemp = tsxc.focTemperature;
            if (Math.Abs(currentTemp - afLastTemp) > 1)
            {
                //Going to have to refocus.  

                //Move to altitude away from meridian, if need be
                sky6RASCOMTele tsxt = new sky6RASCOMTele();
                tsxt.GetAzAlt();
                double tAlt = tsxt.dAlt;
                if (tAlt > 80)
                {
                    double tAz = tsxt.dAz;
                    tAlt = 80.0;
                    tsxt.SlewToAzAlt(tAz, tAlt, "AtFocus2ReadyPosition");
                }

                //reset last temp
                afLastTemp = currentTemp;
                int syncSave = tsxc.Asynchronous;
                tsxc.Asynchronous = 0;
                try
                {
                    int focStat = tsxc.AtFocus2();
                }
                catch (Exception e)
                {
                    tsxc.Asynchronous = syncSave;
                    return ("Focus Check: " + e.Message);
                }
                return ("Focus Check: Focus successful");
            }
            return ("Focus Check: Temperature change less than 1 degree");
        }
    }
}
