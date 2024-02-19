using System;
using System.Drawing;
using System.Windows.Forms;
using TheSky64Lib;

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

        public static bool IsInsideGalacticRadius(double suspectLocRAHrs, double suspectLocDecDeg, double galaxyLocRAHrs, double galaxyLocDecDeg, double radiusDeg)
        {
            //Returns true if suspect location susLoc is within radius of galaxy location galLoc

            //Approximation of separation of ra/dec for very small differences (<<1)
            // separation (radians) theta ~= sqrt {δ x^{2}+δ y^{2}}
            // where δ x = (α A − α B ) cos ⁡ δ A {\displaystyle δ x = (\alpha _{ A} -\alpha _{ B})\cos δ _{ A} }
            // and δ y = δ A − δ B {\displaystyle δ y =δ _{ A} -δ _{ B} }

            //Convert hours to radians
            double sRA = AstroMath.Transform.HoursToRadians(suspectLocRAHrs);
            double sDec = AstroMath.Transform.DegreesToRadians(suspectLocDecDeg);
            double gRA = AstroMath.Transform.HoursToRadians(galaxyLocRAHrs);
            double gDec = AstroMath.Transform.DegreesToRadians(galaxyLocDecDeg);

            double deltaRa = (sRA - gRA) * Math.Cos(sDec);
            double deltaDec = sDec - gDec;
            double sepRadians = Math.Sqrt((deltaRa * deltaRa) + (deltaDec * deltaDec));
            double sepTSXDeg = AstroMath.Transform.RadiansToDegrees(sepRadians);

            if (sepTSXDeg < radiusDeg)
                return true;
            else
                return false;

        }
    }
}

