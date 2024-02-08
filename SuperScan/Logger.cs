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

    public delegate void LogToScreenEvent(string LogText);

    public partial class Logger
    {
        //This class saves log entries to a text file and tells the Superscan window (form) to post the log entry
        //This is going to be our event publisher.  Events will be posted to subscriber FormSuperScan for 
        //  posting on board
        //Statements for Publishing as event generator

        public static event LogToScreenEvent LogToScreenMethod;

        public static void LogToFileAndScreen(string upd)
        {
            LogToFile(upd);
            LogToScreen(upd);
        }

        public static void LogToFile(string upd)
        {
            Configuration ss_cfg = new Configuration();
            string logfilename = DateTime.Now.ToString("yyyy_MM_dd") + ".log";
            string logfilepath = ss_cfg.LogFolder + "\\" + logfilename;
            System.IO.StreamWriter sys_sw = new System.IO.StreamWriter(logfilepath, true);
            sys_sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " :: " + upd);
            LogToScreen(upd);
            sys_sw.Close();
        }

        protected static void LogToScreen(string logEntry)
        {
            //Generates the event LogFileUpudate(sender string)
            LogToScreenMethod?.Invoke(logEntry);
        }
    }
}
