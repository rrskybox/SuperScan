// --------------------------------------------------------------------------------
// VariScan module
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
using System.Collections.Generic;
using System.Linq;
using TheSky64Lib;

namespace SuperScan
{
    public class Reduction
    {

        //list of calibration library names, by reduction group index (zero based)
        private List<ReductionLibrary> LibraryList = new List<ReductionLibrary>();
        //list of filter names, by filter index (zero based)
        private string[] FilterList;

        public string ReductionGroupName { get; set; } = "";

        private class ReductionLibrary
        {
            //Calibration libraries should have format B<*>_T<*>_E<*>_F<*>

            public string Name { get; set; } = "";
            public string Filter { get; set; } = "";
            public double Exposure { get; set; } = 0;
            public int Temperature { get; set; } = 0;
            public string Binning { get; set; } = "1x1";

            public ReductionLibrary(string name)
            {
                Name = name;
                List<string> codeList = (name.Split('_')).ToList();
                foreach (string cType in codeList)
                {
                    char c = cType[0];
                    string n = cType.Remove(0, 1);
                    switch (c)
                    {
                        case 'T':
                            {
                                Temperature = Convert.ToInt32(n);
                                break;
                            }
                        case 'B':
                            {
                                Binning = n;
                                break;
                            }
                        case 'E':
                            {
                                Exposure = Convert.ToDouble(n);
                                break;
                            };
                        case 'F':
                            {
                                Filter = n;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        /*
         * Note:  Calibration folders are expected to have the format
         * 
         *  Group_B<*>_T<*>_E<*>_F<*>
         *  
         * Object decription
         * 
         * Opening a reduction object produces list of calibration folders
         * that can be parsed for specific temp, filter and exposure
         */

        public Reduction()
        {
            ccdsoftCamera tsxc = new ccdsoftCamera();
            //Figure out the filter mapping
            //Find the filter name for the filter filter Number
            FilterList = Filters.FilterNameSet();
            int reductionGroupCount = tsxc.ReductionGroupCount;
            for (int g = 0; g < reductionGroupCount; g++)
            {
                string parseName = tsxc.ReductionGroupFromIndex(g);
                if (parseName.Contains("Group_"))
                {
                    ReductionLibrary reductionLib = new ReductionLibrary(parseName);
                    LibraryList.Add(reductionLib);
                }
            }
        }

        /// <summary>
        /// Selects and sets the Reduction Group prior to imaging a light frame
        /// </summary>
        /// <param name="filterNumber">Filter to be used</param>
        /// <param name="exposure">Exposure length of image</param>
        /// <param name="temperature">Set temperature of camera</param>
        /// <param name="binning">Set binning level of image</param>
        /// <returns></returns>
        public bool SetReductionGroup(int filterNumber, double exposure, int temperature, string binning = "1X1")
        {
            {
                //
                ccdsoftCamera tsxc = new ccdsoftCamera();
                //Translate filterNumber into filterName
                string filterName = FilterList[filterNumber];
                //Build sublist for filterNumber, temperature and binning
                List<ReductionLibrary> rsubList = LibraryList.Where(x => (x.Filter == filterName) && (x.Temperature == temperature) && (x.Binning == binning)).ToList();
                ReductionGroupName = ClosestExposure(rsubList, exposure);
                if (ReductionGroupName != null)
                {
                    ReductionGroupName = ReductionGroupName.Remove(0, 6);
                    tsxc.ReductionGroupName = ReductionGroupName;
                    return true;
                }
                else
                    return false;
            }
        }


        /// <summary>
        ///Returns calibration library name whose exposure is closest to exposure argument
        /// </summary>
        /// <param name="rlib">Sublist of calibration libraries in TSX that match filter, temp and binning</param>
        /// <param name="exposure">in seconds for image to be noise reduced</param>
        /// <returns>name of appropriate reduction library or null if none found</returns>
        private string ClosestExposure(List<ReductionLibrary> rlib, double exposure)
        {
            string calLibName = null;
            int closest = int.MaxValue;
            foreach (ReductionLibrary rl in rlib)
            {
                int current = (int)Math.Abs(rl.Exposure - exposure);
                if (current <= closest)
                {
                    calLibName = rl.Name;
                    closest = current;
                }

            }
            return calLibName;
        }

    }
}
