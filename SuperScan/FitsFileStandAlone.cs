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
using System.Globalization;
using System.IO;
using System.Linq;

namespace VariScan
{
    public class FitsFileStandAlone
    {
        byte[] headerRecord = new byte[80];
        byte[] dataUnit = new byte[2880];
        int bCount;

        //private UInt16[,] FITSArray;

        private List<string> fitsHdr = new List<string>();
        private UInt16[] fitsData = new UInt16[1];
        private UInt16[] fitsHist = new UInt16[256];

        const int ImageHeaderLength = 56 + (256 * 4);

        public string FilePath { get; set; }
        public DateTime FitsUTCDateTime { get; set; }
        public string Filter { get; set; }
        public DateTime FitsLocalDateTime { get; set; }

        public FitsFileStandAlone(string filepath)
        {

            //Opens file set by filepath (assumes it's a FITS formatted file)
            //Reads in header in 80 character strings, while ("END" is found
            //if (dataswitch is true, { an array "FITSimage" is created and populated with the FITS image data as an class Image
            //  otherwise just the header info is retained
            //
            FilePath = filepath;

            int keyindex = -1;
            FileStream FitsHandle = File.OpenRead(filepath);
            do
            {
                keyindex++;
                bCount = FitsHandle.Read(headerRecord, 0, 80);
                //Check for empty file (file error on creation), just opt out if (so
                if (bCount == 0)
                    return;
                fitsHdr.Add(System.Text.Encoding.ASCII.GetString(headerRecord));
            } while (!fitsHdr.Last().StartsWith("END "));
            //Continue through any remaining header padding
            do
            {
                keyindex++;
                bCount = FitsHandle.Read(headerRecord, 0, 80);
            } while (!(keyindex % 36 == 0));

            //Get the array dimensions
            string fitsUTC = ReadKey("DATE-OBS").Split('.')[0];
            string[] dsts = fitsUTC.Split('T');
            string[] ds = dsts[0].Split('-');
            string[] dt = dsts[1].Split(':');
            int year = Convert.ToInt16(ds[0]);
            int month = Convert.ToInt16(ds[1]);
            int day = Convert.ToInt16(ds[2]);
            int hour = Convert.ToInt16(dt[0]);
            int minute = Convert.ToInt16(dt[1]) % 60;
            int second = Convert.ToInt16(dt[2]);
            DateTime utcDT = new DateTime(year, month, day, hour, minute, second);
            string FitsUTCDate = utcDT.Date.ToShortDateString();
            string FitsUTCTime = utcDT.TimeOfDay.ToString();
            FitsUTCDateTime = utcDT;
            Filter = ReadKey("FILTER");
            string fitsLocal = ReadKey("LOCALTIM");
            fitsLocal = fitsLocal.Substring(0, fitsLocal.Length - 4);
            FitsLocalDateTime = DateTime.ParseExact(fitsLocal, "M d yyyy hh:mm:ss.FFF tt", CultureInfo.CurrentCulture);  //'5/18/2020 01:53:24.469 AM STD' 

            //Close file
            FitsHandle.Close();
            return;
        }

        public string ReadKey(string keyword)
        {
            //return;s contents of key word entry, scrubbed of extraneous characters
            foreach (string keyline in fitsHdr)
            {
                if (keyline.Contains(keyword))
                {
                    int startindex = keyline.IndexOf("=");

                    int endindex = keyline.IndexOf(" /");
                    if (endindex == -1)
                    {
                        endindex = keyline.Length - 1;
                    }

                    string keylineN = keyline.Substring(startindex + 1, endindex - (startindex + 1));
                    // keyline = Replace(keyline, "//", " ");
                    keylineN = keylineN.Replace('/', ' ');
                    keylineN = keylineN.Replace('\'', ' ');
                    keylineN = keylineN.Trim(' ');
                    return (keylineN);
                }
            }
            return (null);
        }

    }
}


