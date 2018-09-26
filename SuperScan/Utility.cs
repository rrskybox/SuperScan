using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using TheSkyXLib;

namespace SuperScan
{
    public static class TTUtility
    {

        //Common utilities for TSX connections
        //
        
        public static double ReduceTo360(double degrees)
        {
            degrees = Math.IEEERemainder(degrees, 360);
            if (degrees < 0)
            { degrees += 360; }
            return degrees;
        }

        public static void ButtonRed(Button genericButton)
        {
            genericButton.ForeColor = Color.Black;
            genericButton.BackColor = Color.LightSalmon;
            return;
        }

        public static void ButtonGreen(Button genericButton)
        {
            genericButton.ForeColor = Color.Black;
            genericButton.BackColor = Color.PaleGreen;
            return;
        }

        public static bool IsButtonRed(Button genericButton)
        {
            if (genericButton.BackColor == Color.LightSalmon)
            { return true; }
            else
            { return false; }
        }

        public static bool IsButtonGreen(Button genericButton)
        {
            if (genericButton.BackColor == Color.PaleGreen)
            { return true; }
            else
            { return false; }
        }

        public static bool CloseEnough(double testval, double targetval, double percentnear)
        {
            //Cute little method for determining if a value is withing a certain percentatge of
            // another value.
            //testval is the value under consideration
            //targetval is the value to be tested against
            //npercentnear is how close (in percent of target val, i.e. x100) the two need to be within to test true
            // otherwise returns false

            if (Math.Abs(targetval - testval) <= Math.Abs((targetval * percentnear / 100)))
            { return true; }
            else
            { return false; }
        }
    }
}

