///Platesolve Class
///
/// ------------------------------------------------------------------------
/// Module Name: PlateSolve
/// Purpose: Encapsulates data and methods doing a TSX Image Link
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description:See SuperScanIPS.doc, Sec xx
/// 
/// ------------------------------------------------------------------------

using System;
#if ISTSX32
using TheSkyXLib;
#endif
#if ISTSX64
using TheSky64Lib;
#endif

namespace SuperScan
{
    public class PlateSolve
    {
        private string plateImagePath = "";
        private string plateImageName = "";
        private double plateImageRA = 0;
        private double plateImageDec = 0;
        private double plateImagePA = 0;
        private double plateImageScale = 0;
        private int plateImageResult = 0;

        public PlateSolve(string psfilepath)
        //Creates a new instance of a plateImage and sets the filepath for storing it
        {
            plateImageName = psfilepath;
            ImageLink tsx_il = new ImageLink();
            tsx_il.pathToFITS = psfilepath;
            try
            {
                tsx_il.execute();
            }
            catch (Exception ex)
            {
                string evx = ex.Message;
                plateImageResult = ex.HResult;
                return;
            }
            ImageLinkResults tsx_ilr = new ImageLinkResults();
            plateImageRA = tsx_ilr.imageCenterRAJ2000;
            plateImageDec = tsx_ilr.imageCenterDecJ2000;
            plateImagePA = tsx_ilr.imagePositionAngle;
            plateImageScale = tsx_ilr.imageScale;
            tsx_il = null;
            tsx_ilr = null;
            GC.Collect();
            return;

        }

        public int Result
        //Retrieve the results of the plate solve);
        {
            get
            {
                return plateImageResult;
            }
        }

        public string ImagePath
        //Store and retrieve the current image RA (presumably after a plate solve);
        {
            get
            {
                return plateImagePath;
            }
        }

        public double RA
        //Store and retrieve the current image RA (presumably after a plate solve);
        {
            get
            {
                return plateImageRA;
            }
        }

        public double Dec
        //Store and retrieve the current image Dec (presumably after a plate solve);
        {
            get
            {
                return plateImageDec;
            }
        }

        public double PA
        //Store and retrieve the current image Position Angle (presumably after a plate solve);
        {
            get
            {
                return plateImagePA;
            }
        }

        public double Scale
        //Store and retrieve the current image Position Angle (presumably after a plate solve);
        {
            get
            {
                return plateImageScale;
            }
        }
    }
}
