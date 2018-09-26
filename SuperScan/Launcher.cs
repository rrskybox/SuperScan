/// Launcher Class
///
/// ------------------------------------------------------------------------
/// Module Name: Launcher 
/// Purpose: Methods for awaiting for a delayed staging, start up and shut down
///     executables
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description: TBD
/// 
/// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SuperScan
{
    class Launcher
    {
        public Launcher()
        {

        }

        public void WaitStage()
        {
            // Wait method gets the staging time from the superscan configuration file,
            //  then runs a one second sleep loop until the current time is greater than the
            //  staging time.

            Configuration ss_cfg = new Configuration();
            //Check to see if Staging executable has been enabled
            //  If so, then wait until the current time is greater than stage system time
            if (Convert.ToBoolean(ss_cfg.StageSystemOn))
            {
                DateTime stageTime = DateTime.Parse(ss_cfg.StageSystemTime);
                
                while (stageTime > System.DateTime.Now)
                {
                    System.Threading.Thread.Sleep(1000);
                    //SuperScanForm.ActiveForm.Show();
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            return;
        }

        public void WaitStart()
        {
            // Wait method gets the start up time from the superscan configuration file,
            //  then runs a one second sleep loop until the current time is greater than the
            //  start up time.
            Configuration ss_cfg = new Configuration();
            if (Convert.ToBoolean(ss_cfg.StartUpOn))
            {
                DateTime startTime = DateTime.Parse(ss_cfg.StartUpTime);
                
                while (startTime > System.DateTime.Now)
                {
                    System.Threading.Thread.Sleep(1000);
                    //SuperScanForm.ActiveForm.Show();
                    System.Windows.Forms.Application.DoEvents();
                }
                return;
            }
        }

        public bool CheckEnd()
        {
            // CheckEnd gets the configured end time and returns true
            //   if the datetime exceeds the end time
            Configuration ss_cfg = new Configuration();
            if (Convert.ToBoolean(ss_cfg.ShutDownOn))
            {
                DateTime startTime = DateTime.Parse(ss_cfg.StartUpTime);
                DateTime endTime = DateTime.Parse(ss_cfg.ShutDownTime);
                //if (startTime > endTime)
                //{
                //    endTime = endTime.AddDays(1);
                //}
                
                if (endTime < System.DateTime.Now)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                return (false);
            }
        }

        public void RunStageSystem()
        {
            //If StageSystemOn is set, then RunStageSystem gets the StageSystem filepath from the superscan config file, if any
            //  then launches it and waits for completion.

            Configuration ss_cfg = new Configuration();
            if (Convert.ToBoolean(ss_cfg.StageSystemOn))
            {
                Process stageSystemExe = new Process();
                if (ss_cfg.StageSystemPath != "")
                {
                    stageSystemExe.StartInfo.FileName = ss_cfg.StageSystemPath;
                    stageSystemExe.Start();
                    stageSystemExe.WaitForExit();
                }
            }
            return;
        }

        public void RunStartUp()
        {
            //If StageSystemOn is set, then RunStageSystem gets the StageSystem filepath from the superscan config file, if any
            //  then launches it and waits for completion.

            Configuration ss_cfg = new Configuration();
            if (Convert.ToBoolean(ss_cfg.StartUpOn))
            {
                if (ss_cfg.StartUpPath != "")
                {
                    Process startUpExe = new Process();
                    startUpExe.StartInfo.FileName = ss_cfg.StartUpPath;
                    startUpExe.Start();
                    startUpExe.WaitForExit();
                }
            }
            return;
        }

        public void RunShutDown()
        {
            //If ShutDownOn is set, then RunShutDown gets the postscan filepath from the superscan config file, if any
            //  then launches it and waits for completion.

            Configuration ss_cfg = new Configuration();
            if (Convert.ToBoolean(ss_cfg.ShutDownOn))
            {
                if (ss_cfg.ShutDownPath != "")
                {
                    Process shutDownExe = new Process();
                    shutDownExe.StartInfo.FileName = ss_cfg.ShutDownPath;
                    shutDownExe.Start();
                    shutDownExe.WaitForExit();
                }
            }
            return;
        }

    }
}
