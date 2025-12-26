/// SuperScanForm Class
///     Base Class for the SuperScan application
/// ------------------------------------------------------------------------
/// Module Name: SuperScanForm
/// Purpose: SuperScan Class for primary form and applicaiton flow management
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// Description:  See SuperScanIPS.doc, Sec 2.
/// 
/// Version Information - See SuperScan Description docx
///  
/// ------------------------------------------------------------------------
using System;
using System.Deployment.Application;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeatherWatch;
using System.IO;

using TheSky64Lib;



namespace SuperScan
{
    public partial class FormSuperScan : Form
    {
        private GalaxyList gList;
        public Logger ss_log = new Logger();

        public FormSuperScan()
        {
            //Initialize application form interfaces
            InitializeComponent();

            //Initialize SuperScan configuration folders, files, default data, as needed
            Configuration ss_cfg = new Configuration();

            //Populate the form configuration parameters with the configuration entries.
            ExposureTimeSetting.Value = Convert.ToDecimal(ss_cfg.Exposure);
            FollowUpExposureTime.Value = Convert.ToDecimal(ss_cfg.FollowUpExposure);
            MinAltitudeSetting.Value = Convert.ToDecimal(ss_cfg.MinAltitude);
            FilterNumberSetting.Value = Convert.ToDecimal(ss_cfg.Filter);
            PostponeDetectionCheckBox.Checked = Convert.ToBoolean(ss_cfg.Postpone);
            AutoRunCheckBox.Checked = Convert.ToBoolean(ss_cfg.AutoStart);
            AutoFocusCheckBox.Checked = Convert.ToBoolean(ss_cfg.AutoFocus);
            WatchWeatherCheckBox.Checked = Convert.ToBoolean(ss_cfg.WatchWeather);
            DomeCheckBox.Checked = Convert.ToBoolean(ss_cfg.UsesDome);
            OnTopCheckBox.Checked = Convert.ToBoolean(ss_cfg.FormOnTop);
            RefreshTargetsCheckBox.Checked = Convert.ToBoolean(ss_cfg.RefreshTargets);
            ImageReductionBox.SelectedItem = ss_cfg.ImageReductionType;
            MinGalaxySetting.Value = Convert.ToDecimal(ss_cfg.MinGalaxySize);
            CCDTemperatureSetting.Value = Convert.ToDecimal(ss_cfg.CCDTemp);
            CLSReductionBox.SelectedItem = ss_cfg.CLSReductionType;
            RefocusTriggerBox.Value = Convert.ToDecimal(ss_cfg.RefocusTrigger);
            try
            { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                this.Text = " in Debug";
            }
            this.Text = "SuperScan V" + this.Text;
            this.Show();

            //Subscribe to logging entry events from other classes/threads
            Logger.LogToScreenMethod += PostLogEntry_Handler;

            gList = new GalaxyList();
            GalaxyCount.Text = gList.GalaxyCount.ToString();

            LogEntry("\r\n" + "********** Initiating SuperScan **********");
            LogEntry("Found " + GalaxyCount.Text + " galaxies available at this time.");

            return;
        }

        private void PostLogEntry_Handler(string logText)
        {
            //event from other classes/threads
            PostLogEntry(logText);
        }

        private void PostLogEntry(string logline)
        {
            this.LogBox.AppendText(logline + "\r\n");
            this.Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void LogEntry(string upd)
        //Method for projecting log entry to the SuperScan Main Form
        {
            Logger.LogToFileAndScreen(upd);
            return;
        }

        private void StartScanButton_Click(object sender, EventArgs e)
        {
            LogEntry("\r\n" + "********** Full Scan Selected **********" + "\r\n");
            StartScan();
            return;
        }

        private void ReScanButton_Click(object sender, EventArgs e)
        {
            LogEntry("\r\n" + "********** ReScan Selected **********" + "\r\n");
            RefreshScanList();
            return;
        }

        private void SuspectsButton_Click(object sender, EventArgs e)
        {
            LogEntry("\r\n" + "**********Checking Suspect List **********" + "\r\n");
            FormSuspectReport ss_sfm = new FormSuspectReport();
            ss_sfm.ShowDialog();
            return;
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            LogEntry("\r\n" + "********** Aborted by User **********" + "\r\n");
            Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            LogEntry("\r\n" + "********** Closed by User **********" + "\r\n");
            Close();
        }

        private void PostponeDetectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Set or reset Postpone Detection configuration to the value in the Postpone Detection checkbox
            Configuration ss_cfg = new Configuration();
            if (PostponeDetectionCheckBox.Checked)
            {
                ss_cfg.Postpone = "True";
            }
            else
            { ss_cfg.Postpone = "False"; }
            return;
        }

        private void AutoFocusCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Set or reset AutoFocus configuration to the value in the AutoFocus checkbox
            Configuration ss_cfg = new Configuration();
            if (AutoFocusCheckBox.Checked)
            {
                ss_cfg.AutoFocus = "True";
            }
            else
            { ss_cfg.AutoFocus = "False"; }
            return;
        }

        private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Set or reset AutoStart configuration to the value in the autostart checkbox
            Configuration ss_cfg = new Configuration();
            if (AutoRunCheckBox.Checked)
            {
                FormAutoRun ss_asf = new FormAutoRun();
                ss_asf.ShowDialog();
                ss_cfg.AutoStart = "True";
                {
                    if (Convert.ToBoolean(ss_cfg.StageSystemOn)) { LogEntry("Staging set for " + ss_cfg.StageSystemTime); }
                    if (Convert.ToBoolean(ss_cfg.StartUpOn)) { LogEntry("Start up set for " + ss_cfg.StartUpTime); }
                    if (Convert.ToBoolean(ss_cfg.ShutDownOn)) { LogEntry("Shut down set for " + ss_cfg.ShutDownTime); }
                }
            }
            else
            { ss_cfg.AutoStart = "False"; }
            return;
        }

        private void StartScan()
        {
            //Check to see if scan already underway, if so, then just ignor
            if (StartScanButton.BackColor == Color.LightCoral)
            { return; }
            //save current color of start scan button
            Color scanbuttoncolorsave = StartScanButton.BackColor;
            StartScanButton.BackColor = Color.LightCoral;
            Configuration ss_cfg = new Configuration();
            //AutoStart section
            if (AutoRunCheckBox.Checked)
            //If AutoStart is enabled, then wait for 15 seconds for the user to disable, if desired
            //  Otherwise, initiate the scan
            {
                LogEntry("\r\n" + "********** AutoRun Initiated **********" + "\r\n" + "Unless unchecked, AutoRun will begin in 15 seconds!\r\n");
                for (int i = 0; i < 60; i++)
                {
                    Show();
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(250);
                    if (!AutoRunCheckBox.Checked)
                    {
                        LogEntry("\r\n" + "********** AutoRun Suspended **********" + "\r\n");
                        break;
                    }
                }
                //If AutoStart is still enabled, initiate PreScan, then StartScan, then PostScan
                //   Otherwise, exit on out
                if (AutoRunCheckBox.Checked)
                {
                    LogEntry("Awaiting System Staging Time at " + ss_cfg.StageSystemTime.ToString() + "\r\n");
                    Launcher.WaitStage();
                    LogEntry("Running System Staging Program **********" + "\r\n");
                    Launcher.RunStageSystem();
                    LogEntry("Awaiting Start Up Time at " + ss_cfg.StartUpTime.ToString() + "\r\n");
                    Launcher.WaitStart();
                    LogEntry("Running Start Up Program **********" + "\r\n");
                    Launcher.RunStartUp();
                }
            }

            gList = new GalaxyList();
            GalaxyCount.Text = gList.GalaxyCount.ToString();
            SuspectCountLabel.Text = "0";

            LogEntry("\r\n" + "********** Beginning SuperScan Run **********");
            LogEntry("Found " + GalaxyCount.Text + " galaxies for this session");

            // Scan Running...
            // Connect telescope mount and camera, and dome, if any
            if (DeviceControl.MountStartUp())
                LogEntry("Initializing Mount");
            else
                LogEntry("Mount initialization failed");
            if (DeviceControl.CameraStartUp())
                LogEntry("Initializing Camera");
            else
                LogEntry("Camera initialization failed");
            if (Convert.ToBoolean(ss_cfg.UsesDome))
            {
                if (DomeControl.DomeStartUp())
                    LogEntry("Initializing Dome");
                else
                    LogEntry("Dome initialization failed");
            }
            ;
            Show();

            //Start the sequence on the west side.
            //Theoretically, nearly all galaxies on west side will be scanned before setting below limit,
            //  and all galaxies that have transited during that time.  Then all galaxies on the list which are east
            //  will be scanned.  Lastly, the scan will return to the west to pick up any galaxies that transited
            //  during the scan on the east side.  Get it?
            LogEntry("Unparking mount");
            DeviceControl.MountUnpark(); //Just in case we're still parked for some reason.
                                         //If using dome, wait until dome catches up with mount position -- TSX won't do that for you and will throw an error
            if (Convert.ToBoolean(ss_cfg.UsesDome))
            {
                LogEntry("Waiting on Dome Alignment");
                DeviceControl.WaitIfDomeRotating();
            }
            //Now move mount to preposition(s)
            LogEntry("Prepositioning mount to West");
            DeviceControl.MountPrePosition("West");
            //Let's do an autofocus to start out.  Set the temperature to -100 to fake out the temperature test
            //  and force an autofocus.
            if (AutoFocusCheckBox.Checked)
            {
                AutoFocus.afLastTemp = -100;
                LogEntry("Running Auto Focus");
                string focStat = AutoFocus.Check();
                LogEntry(focStat);
            }

            //Open up the configuration parameteters, update with current form inputs
            ss_cfg.Exposure = ExposureTimeSetting.Value.ToString();
            ss_cfg.MinAltitude = MinAltitudeSetting.Value.ToString();
            ss_cfg.Filter = FilterNumberSetting.Value.ToString();
            ss_cfg.CCDTemp = CCDTemperatureSetting.Value.ToString();
            ss_cfg.Postpone = PostponeDetectionCheckBox.Checked.ToString();
            ss_cfg.AutoStart = AutoRunCheckBox.Checked.ToString();
            ss_cfg.AutoFocus = AutoFocusCheckBox.Checked.ToString();
            ss_cfg.WatchWeather = WatchWeatherCheckBox.Checked.ToString();
            ss_cfg.RefocusTrigger = RefocusTriggerBox.Value.ToString();
            LogEntry("Starting Scan");

            LogEntry("Bringing camera to temperature");
            double setTemp = Convert.ToDouble(CCDTemperatureSetting.Value);
            const int temperatureSettlingRange = 5; //in percent
            DeviceControl.CameraTemperature = setTemp;
            DateTime startTime = DateTime.Now;
            while (!TTUtility.CloseEnough(DeviceControl.CameraTemperature, setTemp, temperatureSettlingRange))
            {
                System.Threading.Thread.Sleep(10000);
                LogEntry("Camera temperature at " + DeviceControl.CameraTemperature.ToString("0.0"));
                if (DateTime.Now - startTime > TimeSpan.FromMinutes(1))
                {
                    LogEntry("Temperature Regulation Timeout at 1 minute");
                    break;
                }
            }
            ;

            int gTriedCount = 0;
            int gSuccessfulCount = 0;

            //
            //
            //Main Loop on the list of galaxies =================
            //
            //  1. Check the weather, if enabled
            //  2. Check the focus (1 degree diff), if enabled
            //  3. Take an image and detect, if enabled
            //
            while (!Launcher.IsSessionElapsed() && gList.GalaxyCount > 0)
            {
                //Check weather conditions, if enabled
                //  if unsafe then spin until it is safe or endingtime occurs.
                //if (!Launcher.IsSessionElapsed() && WatchWeatherCheckBox.Checked)
                if (WatchWeatherCheckBox.Checked)
                {
                    LogEntry("Checking Weather");
                    if (!IsWeatherSafe())
                    {
                        LogEntry("Waiting on unsafe weather conditions...");
                        LogEntry("Parking telescope");
                        if (DeviceControl.MountShutDown())
                            LogEntry("Mount parked");
                        else
                            LogEntry("Mount park failed");
                        if (Convert.ToBoolean(ss_cfg.UsesDome))
                        {
                            LogEntry("Closing Dome");
                            if (DomeControl.CloseDome())
                                LogEntry("Dome Closed");
                            else
                                LogEntry("Dome Close Failed");
                            while (!Launcher.IsSessionElapsed() && !IsWeatherSafe())
                            {
                                System.Threading.Thread.Sleep(10000);  //ten second wait loop
                                Show();
                                System.Windows.Forms.Application.DoEvents();
                            }
                            if (!Launcher.IsSessionElapsed() && IsWeatherSafe())
                            {
                                LogEntry("Weather conditions safe");
                                LogEntry("Opening Dome");
                                if (Convert.ToBoolean(ss_cfg.UsesDome)) DomeControl.OpenDome();
                                LogEntry("Unparking telescope");
                                if (DeviceControl.MountStartUp()) LogEntry("Mount unparked");

                                //Wait for 30 seconds for everything to settle
                                LogEntry("Waiting for dome to settle");
                                System.Threading.Thread.Sleep(30000);
                                //Recouple dome
                                if (Convert.ToBoolean(ss_cfg.UsesDome))
                                    DeviceControl.AllDomeCouplingOn();
                            }
                        }
                    }
                }
                //Weather, if checked, is safe and we may still be in session
                //Check for autofocus selection.  If so then run the autofocus check.
                if (!Launcher.IsSessionElapsed() && AutoFocusCheckBox.Checked)
                {
                    //One stop shopping
                    LogEntry("Checking Focus");
                    string focStat = AutoFocus.Check();
                    LogEntry(focStat);
                }
                //Check one more time on session elapsed, if all good then proceed to survey a galaxy

                if (!Launcher.IsSessionElapsed())
                {
                    //Get the next galaxy.  display its name and size.
                    string targetName = gList.Next;
                    CurrentGalaxyName.Text = targetName;
                    CurrentGalaxySizeArcmin.Text = gList.MaxAxisArcMin(targetName).ToString();
                    //Show();


                    //If altitude too low then pass on this one.
                    if (gList.Altitude(targetName) < gList.MinAltitude)
                    {
                        //LogEntry(targetName + " at " + gList.Altitude(targetName).ToString("0.0") + " degrees Alt is below minimum");
                    }
                    //If the target was imaged within the last 12 hours then pass on this one.
                    else if (IsSurveyedCurrentSession(targetName))
                    {
                        //LogEntry(targetName + " previously surveyed this session");
                    }
                    else
                    {
                        LogEntry("Queueing up next galaxy: " + targetName);
                        //Take fresh image
                        FreshImage fso = new FreshImage();
                        //Seek location of next galaxy
                        //Ignor return value
                        fso.Acquire(targetName, (Convert.ToDouble(ExposureTimeSetting.Value)));
                        if (fso.ImagePath == "")
                        {
                            LogEntry(targetName + ": " + " Image capture failed -- probably CLS failure.");
                            LogEntry("");
                        }
                        else
                        {
                            LogEntry(targetName + " Image capture complete.");
                            LogEntry(targetName + " Looking in Image Bank for most recent image.");
                            //Save Image
                            ImageBank sio = new ImageBank(targetName);
                            LogEntry(targetName + ":" + " Banking new image in " + ss_cfg.FreshImagePath);
                            sio.AddImage(ss_cfg.FreshImagePath);
                            //Increment the galaxy count for reporting purposes
                            gSuccessfulCount++;
                            //check for a reference image
                            //  if so then move on to detecting any prospects
                            //  if not, then just log the situation and move on
                            if (sio.MostRecentImagePath != "")
                            {
                                int subFrameSize = Convert.ToInt32(60 * gList.MaxAxisArcMin(targetName));
                                LogEntry("Detecting supernova prospects.");
                                //Check to see if detection is to be run or not
                                //  if so, open a detection object with the log handler property set up
                                //   if not, just move on
                                if (!PostponeDetectionCheckBox.Checked)
                                {
                                    NovaDetection ss_ndo = new NovaDetection();
                                    ss_ndo.LogUpdate += LogEntry;
                                    if (ss_ndo.Detect(targetName,
                                        subFrameSize,
                                        gList.RA(targetName),
                                        gList.Dec(targetName),
                                        fso.ImagePath,
                                        sio.MostRecentImagePath,
                                        sio.WorkingImageFolder))
                                    {
                                        SuspectCountLabel.Text = (Convert.ToInt32(SuspectCountLabel.Text) + 1).ToString();
                                    }
                                }
                                else
                                    LogEntry("Supernova detecton postponed per request.");
                            }
                            else
                                LogEntry("No banked image for comparison.");
                        }
                    }
                    //Update tries counter
                    gTriedCount++;
                    //Clear galaxy from list and decrement galaxies left to image
                    gList.Remove(targetName);
                    GalaxyCount.Text = gList.GalaxyCount.ToString();
                    Show();
                }
                //Check for time to shut down, then...
                //if we aren't out of time, check to see if we're out of galaxies
                //  if so, refresh the list and continue
                //  otherwise time to end
                LogEntry("Checking for ending time");
                if (Convert.ToBoolean(Launcher.IsSessionElapsed()))
                    LogEntry("Session elapsed.");
                else if (gList.GalaxyCount == 0 && ss_cfg.RefreshTargets == "True")
                    RefreshTargetsNow();
            }

            LogEntry("Session Completed");
            LogEntry("SuperScan surveyed " + gSuccessfulCount + " out of " + gTriedCount + " galaxies.");

            //Park the telescope so it doesn't drift too low
            DeviceControl.MountShutDown();
            LogEntry("AutoRun Running Shut Down Program **********" + "\r\n");
            Launcher.RunShutDown();
            StartScanButton.BackColor = scanbuttoncolorsave;
            return;
        }


        private void RefreshScanList()
        {
            string curFilePath = "";
            string refFilePath = "";

            //Open up the configuration parameteters, update with current form inputs
            Configuration ss_cfg = new Configuration();
            LogEntry("Starting ReScan");

            string ibdir = ss_cfg.ImageBankFolder;
            string[] imageBankDirs = System.IO.Directory.GetDirectories(ibdir);
            GalaxyCount.Text = imageBankDirs.Length.ToString();

            if (imageBankDirs.Length == 0)
            {
                LogEntry("Empty Image Bank");
                return;
            }
            //Determine if all or just one target is to be tested
            FormChooseGalaxyTarget ngd = new FormChooseGalaxyTarget();
            ngd.ShowDialog(this);
            var selectedTargetList = ngd.TargetNameList.SelectedItems;
            ngd.Dispose();
            //Convert selectedTargetList to list of targets
            foreach (string pickName in selectedTargetList)
                foreach (string galdir in imageBankDirs)
                {
                    System.IO.DirectoryInfo sys_imd = new System.IO.DirectoryInfo(galdir);
                    if (sys_imd.Name == pickName)
                    {
                        string targetName = sys_imd.Name;
                        LogEntry("Running " + targetName);
                        System.IO.DirectoryInfo sys_gal = new System.IO.DirectoryInfo(galdir);
                        if (sys_gal.GetFiles("NGC*.fit").Length >= 2)
                        {
                            curFilePath = FindCurrentFile(galdir);
                            refFilePath = FindReferenceFile(galdir);

                            CurrentGalaxyName.Text = targetName;
                            sky6StarChart tsx_sc = new sky6StarChart();
                            sky6ObjectInformation tsx_oi = new sky6ObjectInformation();
                            tsx_sc.Find(targetName);
                            tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
                            double gRA = Convert.ToDouble(tsx_oi.ObjInfoPropOut);
                            tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
                            double gDec = Convert.ToDouble(tsx_oi.ObjInfoPropOut);
                            tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAJ_AXIS_MINS);
                            double gMaxArcMin = Convert.ToDouble(tsx_oi.ObjInfoPropOut);
                            CurrentGalaxySizeArcmin.Text = gMaxArcMin.ToString();
                            int subFrameSize = Convert.ToInt32(60 * gMaxArcMin);

                            NovaDetection ss_ndo = new NovaDetection();
                            ss_ndo.LogUpdate += LogEntry;
                            ss_ndo.Detect(targetName, subFrameSize, gRA, gDec, curFilePath, refFilePath, galdir);
                        }
                        else
                        {
                            LogEntry("Insufficient Images");
                        }
                    }
                    GalaxyCount.Text = (Convert.ToInt16(GalaxyCount.Text) - 1).ToString();
                }
            LogEntry("ReScan Done");
            return;
        }

        private DateTime? GetDate(string fname)
        {
            //fname format expected to be *_dd-MM-yyyy-hh-mm.fit"
            //Parse fname the substrings, then convert to datetime
            //The fname is possibily a path that contains an underscore, so isolate the filename first
            string filenameonly = Path.GetFileName(fname);

            //int uscr = fname.IndexOf("_");
            //string yearstr = fname.Substring(uscr + 1, 4);
            //string monstr = fname.Substring(uscr + 6, 2);
            //string daystr = fname.Substring(uscr + 9, 2);
            //string hourstr = fname.Substring(uscr + 12, 2);
            //string minstr = fname.Substring(uscr + 14, 2);

            int uscr = fname.Length - 20;
            string yearstr = fname.Substring(uscr + 1, 4);
            string monstr = fname.Substring(uscr + 6, 2);
            string daystr = fname.Substring(uscr + 9, 2);
            string hourstr = fname.Substring(uscr + 12, 2);
            string minstr = fname.Substring(uscr + 14, 2);

            DateTime? fdate = null;
            try
            {
                fdate = new DateTime(
                Convert.ToInt32(yearstr),
                Convert.ToInt32(monstr),
                Convert.ToInt32(daystr),
                Convert.ToInt32(hourstr),
                Convert.ToInt32(minstr),
                0);
            }
            catch
            {
                LogEntry("Invalid Filename Format: " + fname);
            }
            return fdate;
        }

        private string FindCurrentFile(string dname)
        {
            //Returns most current image file of target dname
            DateTime latestDate = new DateTime(0);
            string latestFile = null;
            System.IO.DirectoryInfo sys_ibd = new System.IO.DirectoryInfo(dname);
            var galImages = sys_ibd.GetFiles("NGC*.fit");
            for (int i = 0; i < galImages.Length; i++)
            {
                DateTime? imageDate = GetDate(galImages.ElementAt(i).FullName);
                if (imageDate != null && imageDate > latestDate)
                {
                    latestFile = galImages.ElementAt(i).FullName;
                }
            }
            return latestFile;
        }

        private DateTime? LatestImageTime(string dname)
        {
            //Returns most current image file of target dname
            Configuration ss_cfg = new Configuration();
            string ibdir = ss_cfg.ImageBankFolder;
            DateTime? latestDate = new DateTime(0);
            if (!Directory.Exists(ibdir + "\\" + dname))
                Directory.CreateDirectory(ibdir + "\\" + dname);
            System.IO.DirectoryInfo sys_ibd = new System.IO.DirectoryInfo(ibdir + "\\" + dname);
            var galImages = sys_ibd.GetFiles("NGC*.fit");
            for (int i = 0; i < galImages.Length; i++)
            {
                DateTime? imageDate = GetDate(galImages.ElementAt(i).FullName);
                if (imageDate != null && imageDate > latestDate)
                    latestDate = imageDate;
            }
            return latestDate;
        }

        private bool IsSurveyedCurrentSession(string targetName)
        {
            if ((LatestImageTime(targetName) + TimeSpan.FromHours(12)) < DateTime.Now)
                return false;
            else
                return true;
        }

        private string FindReferenceFile(string dname)
        {
            DateTime nextLatestDate = new DateTime(0);
            string latestFile = FindCurrentFile(dname);
            string nextLatestFile = null;
            System.IO.DirectoryInfo sys_ibd = new System.IO.DirectoryInfo(dname);
            var galImages = sys_ibd.GetFiles("NGC*.fit");
            for (int i = 0; i < galImages.Length; i++)
            {
                DateTime? imageDate = GetDate(galImages.ElementAt(i).FullName);
                if (imageDate != null && (GetDate(galImages.ElementAt(i).FullName) > nextLatestDate)
                    && (galImages.ElementAt(i).FullName != latestFile))
                {
                    nextLatestFile = galImages.ElementAt(i).FullName;
                }
            }
            return nextLatestFile;
        }

        private void OnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            if (OnTopCheckBox.Checked)
            {
                ss_cfg.FormOnTop = "True";
                this.TopMost = true;
                this.Show();
            }
            else
            {
                ss_cfg.FormOnTop = "False";
                this.TopMost = false;
                this.Show();
            }
            return;
        }

        private void MinAltitudeSetting_ValueChanged(object sender, EventArgs e)
        {
            if (gList != null) gList.MinAltitude = (double)MinAltitudeSetting.Value;
            Configuration ss_cfg = new Configuration();
            ss_cfg.MinAltitude = MinAltitudeSetting.Value.ToString();
        }

        private void ExposureTimeSetting_ValueChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            ss_cfg.Exposure = ExposureTimeSetting.Value.ToString();
        }

        private void FilterNumberSetting_ValueChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            ss_cfg.Filter = FilterNumberSetting.Value.ToString();
        }

        private void CCDTemperatureSetting_ValueChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            ss_cfg.CCDTemp = CCDTemperatureSetting.Value.ToString();
        }

        private void RefocusTriggerBox_ValueChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            ss_cfg.RefocusTrigger = RefocusTriggerBox.Value.ToString();
        }


        private bool IsWeatherSafe()
        {
            //Returns true if no weather alert, false if it is unsafe

            WeatherReader wmon = new WeatherReader();
            //if no weather file or other problem, return false
            if (wmon == null)
                return false;
            if (wmon.AlertFlag == WeatherReader.WeaAlert.Alert)
                return false;
            else
                return true;
        }

        private void CullButton_Click(object sender, EventArgs e)
        {
            //Verify that the user really wants to clear the files, then cull images
            DialogResult cleanUp = MessageBox.Show("Do you really want to clean up the Image Bank directories?", "Verify Clean Up", MessageBoxButtons.YesNo);
            if (cleanUp == DialogResult.No) return;
            else
            {
                FormCrunchingNotice cForm = new FormCrunchingNotice();
                cForm.Show();
                CullImages.DeletellButBest();
                cForm.Close();
            }
        }

        private void WatchWeatherCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            if (WatchWeatherCheckBox.Checked) ss_cfg.WatchWeather = "True";
            else ss_cfg.WatchWeather = "False";
        }

        private void DomeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            if (DomeCheckBox.Checked) ss_cfg.UsesDome = "True";
            else ss_cfg.UsesDome = "False";
        }

        private void MinGalaxySetting_ValueChanged(object sender, EventArgs e)
        {
            //Attempt to change minimum galaxy size -- save changed value and rerun galaxy list
            Configuration ss_cfg = new Configuration();
            ss_cfg.MinGalaxySize = MinGalaxySetting.Value.ToString();
            Color oldColor = MinGalaxySetting.BackColor;
            MinGalaxySetting.BackColor = Color.Salmon;
            gList = new GalaxyList();
            GalaxyCount.Text = gList.GalaxyCount.ToString();
            MinGalaxySetting.BackColor = oldColor;
        }



        private void RefreshTargetsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            if (RefreshTargetsCheckBox.Checked)
            {
                ss_cfg.RefreshTargets = "True";
                RefreshTargetsNow();
                return;
            }
            else
            {
                ss_cfg.RefreshTargets = "False";
                //Verify that there is content in the static target file
                LogEntry("Refresh galaxy list deselected -- now using fixed list, if populated.");
                List<string> targetSet = new List<string>();
                try { targetSet = File.ReadAllLines(ss_cfg.ObservingListPath).ToList(); }
                catch
                {
                    //ss_cfg.RefreshTargets = "True";
                    LogEntry("Fixed target set file is missing. Make sure that SuperScanObservingList.txt exists and contains targets.");
                    //RefreshTargetsCheckBox.Checked = true;
                    return;
                }
                if (targetSet.Count < 1)
                {
                    //ss_cfg.RefreshTargets = "True";
                    LogEntry("Fixed target set file is empty. Make sure that SuperScanObservingList.txt contains targets.");
                    //RefreshTargetsCheckBox.Checked = true;
                    return;
                }
                else
                {
                    gList = new GalaxyList();
                    GalaxyCount.Text = gList.GalaxyCount.ToString();
                    LogEntry("Fixed target set has " + GalaxyCount.Text + " targets.");
                    return;
                }
            }
        }

        private void CalibrationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Index 0 = No Cal
            Configuration ss_cfg = new Configuration();
            ss_cfg.ImageReductionType = ImageReductionBox.SelectedItem.ToString();
        }

        private void CLSReductionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Index 0 = No Cal
            Configuration ss_cfg = new Configuration();
            ss_cfg.CLSReductionType = CLSReductionBox.SelectedItem.ToString();
        }

        private void FollowUpExposureTime_ValueChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            ss_cfg.FollowUpExposure = FollowUpExposureTime.Value.ToString();
        }

        private void RefreshTargetsNow()
        {
            Configuration cfg = new Configuration();
            //cfg.RefreshTargets = "True";
            LogEntry("Refresh galaxy list selected -- launching observing list query");
            Cursor.Current = Cursors.WaitCursor;
            gList = new GalaxyList();
            GalaxyCount.Text = gList.GalaxyCount.ToString();
            LogEntry("Queried target set has " + GalaxyCount.Text + " new targets.");
        }

    }
}


