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
        //This class saves log entries to a text file and tells the Superscan window (form) to post the log entry
        //This is going to be our event publisher.  Events will be posted to subscriber FormSuperScan for 
        //  posting on board
        //Statements for Publishing as event generator
        
        //public delegate void LogEventHandler(string LogText);
        public event EventHandler<string> LogFileUpdate;

        private string logfilepath;

        public Logger()
        {
            Configuration ss_cfg = new Configuration();
            string logfilename = DateTime.Now.ToString("yyyy_MM_dd") + ".log";
            logfilepath = ss_cfg.LogFolder + "\\" + logfilename;
        }

        public void PostLogEntry(string upd)
        {
            System.IO.StreamWriter sys_sw = new System.IO.StreamWriter(logfilepath, true);
            sys_sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " :: " + upd);
            LogToScreen(upd);
            sys_sw.Close();
        }

        protected virtual void LogToScreen(string logEntry)
        {
            //Generates the event LogFileUpudate(sender string)
            LogFileUpdate?.Invoke(this, logEntry);
        }
    }
}
