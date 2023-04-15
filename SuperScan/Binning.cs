using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ISTSX32
using TheSkyXLib;
#endif
#if ISTSX64
using TheSky64Lib;
#endif

namespace SuperScan
{
    public class Binning
    {
        public static string GetBinning()
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            int xBin = tsxc.BinX;
            int yBin = tsxc.BinY;
            return (xBin.ToString() + "X" + yBin.ToString());
        }
    }
}

