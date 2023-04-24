/// Logger Class
///
/// ------------------------------------------------------------------------
/// Module Name: Logger 
/// Purpose: Write log entries to SuperScan Form and to the log file.
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description: TBD
/// 
/// ------------------------------------------------------------------------
using System;

namespace SuperScan
{
    public class Logger
    {
        private string logfilepath;

        public Logger()
        {
            Configuration ss_cfg = new Configuration();
            string logfilename = DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
            logfilepath = ss_cfg.LogFolder + "\\" + logfilename;

            return;
        }

        public void LogEntry(string upd)
        {
            System.IO.StreamWriter sys_sw = new System.IO.StreamWriter(logfilepath, true);
            sys_sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " :: " + upd);
            sys_sw.Close();
            return;
        }

    }
}
