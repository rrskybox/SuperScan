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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheSkyXLib;
using WeatherWatch;

namespace SuperScan
{
    public partial class SuperScanForm : Form
    {
        private GalaxyList gList;
        private Logger ss_log = new Logger();

        const int DOME_HOME_AZ = 220;

        public SuperScanForm()
        {
            //Initialize application form interfaces
            InitializeComponent();

            //Initialize SuperScan configuration folders, files, default data, as needed
            Configuration ss_cfg = new Configuration();

            //Populate the form configuration parameters with the configuration entries.
            ExposureTimeSetting.Value = Convert.ToDecimal(ss_cfg.Exposure);
            MinAltitudeSetting.Value = Convert.ToDecimal(ss_cfg.MinAltitude);
            FilterNumberSetting.Value = Convert.ToDecimal(ss_cfg.Filter);
            PostponeDetectionCheckBox.Checked = Convert.ToBoolean(ss_cfg.Postpone);
            AutoRunCheckBox.Checked = false;
            AutoFocusCheckBox.Checked = Convert.ToBoolean(ss_cfg.AutoFocus);
            WatchWeatherCheckBox.Checked = Convert.ToBoolean(ss_cfg.WatchWeather);
            try
            { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                this.Text = " in Debug";
            }
            this.Text = "SuperScan V" + this.Text;

            gList = new GalaxyList();
            GalaxyCount.Text = gList.GalaxyCount.ToString();

            LogEventHandler("\r\n" + "********** Initiating SuperScan **********");
            LogEventHandler("Found " + GalaxyCount.Text + " galaxies available at this time.");

            return;
        }

        private void SuperScanForm_Load(object sender, EventArgs e)
        {
        }

        private void StartScanButton_Click(object sender, EventArgs e)
        {
            LogEventHandler("\r\n" + "********** Full Scan Selected **********" + "\r\n");
            StartScan();
            return;
        }

        private void ReScanButton_Click(object sender, EventArgs e)
        {
            LogEventHandler("\r\n" + "********** ReScan Selected **********" + "\r\n");
            ReScan();
            return;
        }

        private void SuspectsButton_Click(object sender, EventArgs e)
        {
            LogEventHandler("\r\n" + "**********Checking Suspect List **********" + "\r\n");
            SuspectReportForm ss_sfm = new SuspectReportForm();
            ss_sfm.ShowDialog();
            return;
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            LogEventHandler("\r\n" + "********** Aborted by User **********" + "\r\n");
            Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            LogEventHandler("\r\n" + "********** Closed by User **********" + "\r\n");

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
                AutoRunForm ss_asf = new AutoRunForm();
                ss_asf.ShowDialog();
                ss_cfg.AutoStart = "True";
                {
                    if (Convert.ToBoolean(ss_cfg.StageSystemOn)) { LogEventHandler("Staging set for " + ss_cfg.StageSystemTime); }
                    if (Convert.ToBoolean(ss_cfg.StartUpOn)) { LogEventHandler("Start up set for " + ss_cfg.StartUpTime); }
                    if (Convert.ToBoolean(ss_cfg.ShutDownOn)) { LogEventHandler("Shut down set for " + ss_cfg.ShutDownTime); }
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
                LogEventHandler("\r\n" + "********** AutoRun Initiated **********" + "\r\n" + "Unless unchecked, AutoRun will begin in 15 seconds!\r\n");
                for (int i = 0; i < 60; i++)
                {
                    Show();
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(250);
                    if (!AutoRunCheckBox.Checked)
                    {
                        LogEventHandler("\r\n" + "********** AutoRun Suspended **********" + "\r\n");
                        break;
                    }
                }
                //If AutoStart is still enabled, initiate PreScan, then StartScan, then PostScan
                //   Otherwise, exit on out
                if (AutoRunCheckBox.Checked)
                {
                    LogEventHandler("Awaiting System Staging Time at " + ss_cfg.StageSystemTime.ToString() + "\r\n");
                    Launcher.WaitStage();
                    LogEventHandler("Running System Staging Program **********" + "\r\n");
                    Launcher.RunStageSystem();
                    LogEventHandler("Awaiting Start Up Time at " + ss_cfg.StartUpTime.ToString() + "\r\n");
                    Launcher.WaitStart();
                    LogEventHandler("Running Start Up Program **********" + "\r\n");
                    Launcher.RunStartUp();
                }
            }

            gList = new GalaxyList();
            GalaxyCount.Text = gList.GalaxyCount.ToString();

            LogEventHandler("\r\n" + "********** Beginning SuperScan Run **********");
            LogEventHandler("Found " + GalaxyCount.Text + " galaxies for this session");

            // Scan Running...
            // Connect telescope mount and camera, and dome, if any
            DeviceControl ss_hwp = new DeviceControl();
            if (ss_hwp.TelescopeStartUp()) LogEventHandler("Initializing Mount");
            else LogEventHandler("Mount initialization failed");
            if (ss_hwp.CameraStartUp()) LogEventHandler("Initializing Camera");
            else LogEventHandler("Camera initialization failed");
            if (Convert.ToBoolean(ss_cfg.HasDome))
            {
                if (DomeControl.DomeStartUp()) LogEventHandler("Initializing Dome");
                else LogEventHandler("Dome initialization failed");
            }
            ;
            Show();

            //Start the sequence on the west side.
            //Theoretically, nearly all galaxies on west side will be scanned before setting below limit,
            //  and all galaxies that have transited during that time.  Then all galaxies on the list which are east
            //  will be scanned.  Lastly, the scan will return to the west to pick up any galaxies that transited
            //  during the scan on the east side.  Get it?
            ss_hwp.TelescopePrePosition("West");
            //Let's do an autofocus to start out.  Set the temperature to -100 to fake out the temperature test
            //  and force an autofocus.
            if (AutoFocusCheckBox.Checked)
            {
                AutoFocus.afLastTemp = -100;
                LogEventHandler("Running Auto Focus");
                string focStat = AutoFocus.Check();
                LogEventHandler(focStat);
            }

            //Open up the configuration parameteters, update with current form inputs
            string ts = ExposureTimeSetting.Value.ToString();
            ss_cfg.Exposure = ExposureTimeSetting.Value.ToString();
            ss_cfg.MinAltitude = MinAltitudeSetting.Value.ToString();
            ss_cfg.Filter = FilterNumberSetting.Value.ToString();
            ss_cfg.CCDTemp = CCDTemperatureSetting.Value.ToString();
            ss_cfg.Postpone = PostponeDetectionCheckBox.Checked.ToString();
            ss_cfg.AutoStart = AutoRunCheckBox.Checked.ToString();
            ss_cfg.AutoFocus = AutoFocusCheckBox.Checked.ToString();
            ss_cfg.WatchWeather = WatchWeatherCheckBox.Checked.ToString();
            LogEventHandler("Starting Scan");
            LogEventHandler("Bringing camera to temperature");
            ss_hwp.SetCameraTemperature(Convert.ToDouble(CCDTemperatureSetting.Value));

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
            while (gList.GalaxyCount > 0)
            {
                //Check weather conditions, if enabled
                //  if unsafe then spin until it is safe or endingtime occurs.
                if (WatchWeatherCheckBox.Checked)
                {
                    LogEventHandler("Checking Weather");
                    if (!IsWeatherSafe())
                    {
                        LogEventHandler("Waiting on unsafe weather conditions...");
                        LogEventHandler("Parking telescope");
                        if (ss_hwp.TelescopeShutDown()) LogEventHandler("Mount parked");
                        else LogEventHandler("Mount park failed");

                        LogEventHandler("Closing Dome");
                        if (Convert.ToBoolean(ss_cfg.HasDome)) DomeControl.CloseDome(DOME_HOME_AZ);
                        do
                        {
                            System.Threading.Thread.Sleep(10000);  //ten second wait loop
                            if (Convert.ToBoolean(Launcher.CheckEnd()))
                            { break; };
                        } while (!IsWeatherSafe());
                        if (Convert.ToBoolean(Launcher.CheckEnd()))
                        { break; };
                        if (IsWeatherSafe())
                        {
                            LogEventHandler("Weather conditions safe");
                            LogEventHandler("Opening Dome");
                            if (Convert.ToBoolean(ss_cfg.HasDome)) DomeControl.OpenDome(DOME_HOME_AZ);
                            LogEventHandler("Unparking telescope");
                            if (ss_hwp.TelescopeStartUp()) LogEventHandler("Mount unparked");

                            //Wait for 30 seconds for everything to settle
                            LogEventHandler("Waiting for dome to settle");
                            System.Threading.Thread.Sleep(30000);
                            //Recouple dome
                            if (Convert.ToBoolean(ss_cfg.HasDome)) DomeControl.IsDomeCoupled = true;
                        }
                    }
                } //Check for autofocus selection.  If so then run the autofocus check.
                if (AutoFocusCheckBox.Checked)
                {
                    //One stop shopping
                    LogEventHandler("Checking Focus");
                    string focStat = AutoFocus.Check();
                    LogEventHandler(focStat);
                }

                //Get the next galaxy.  display its name and size.
                string targetName = gList.Next;
                CurrentGalaxyName.Text = targetName;
                CurrentGalaxySizeArcmin.Text = gList.MaxAxis(targetName).ToString();
                Show();

                LogEventHandler("Queueing up next galaxy: " + targetName);

                //Check altitude.  If too low then pass on this one.
                if (gList.Altitude(targetName) < gList.MinAltitude)
                {
                    LogEventHandler(targetName + " at " + gList.Altitude(targetName).ToString("0.0") + " degrees Alt is below minimum");
                }
                else
                {
                    //Take fresh image
                    FreshImage fso = new FreshImage();
                    fso.LogUpdate += LogEventHandler;
                    //Seek location of next galaxy
                    //Ignor return value
                    fso.Acquire(targetName, (Convert.ToDouble(ExposureTimeSetting.Value)));
                    if (fso.ImagePath == "")
                    {
                        LogEventHandler(targetName + ": " + " Image capture failed -- probably CLS failure.");
                        LogEventHandler("");
                    }
                    else
                    {
                        LogEventHandler(targetName + " Image capture complete.");
                        LogEventHandler(targetName + " Looking in Image Bank for most recent image.");
                        //Save Image
                        ImageBank sio = new ImageBank(targetName);
                        LogEventHandler(targetName + ":" + " Banking new image in " + ss_cfg.FreshImagePath);
                        sio.AddImage(ss_cfg.FreshImagePath);
                        //Increment the galaxy count for reporting purposes
                        gSuccessfulCount++;
                        //check for a reference image
                        //  if so then move on to detecting any prospects
                        //  if not, then just log the situation and move on
                        if (sio.MostRecentImagePath != "")
                        {
                            int subFrameSize = Convert.ToInt32(60 * gList.MaxAxis(targetName));
                            LogEventHandler("Detecting supernova prospects.");
                            //Check to see if detection is to be run or not
                            //  if so, open a detection object with the log handler property set up
                            //   if not, just move on
                            if (!PostponeDetectionCheckBox.Checked)
                            {
                                NovaDetection ss_ndo = new NovaDetection();
                                ss_ndo.LogUpdate += LogEventHandler;
                                ss_ndo.Detect(targetName, subFrameSize, gList.RA(targetName), gList.Dec(targetName), fso.ImagePath, sio.MostRecentImagePath, sio.WorkingImageFolder);
                            }
                            else
                            {
                                LogEventHandler("Supernova detecton postponed per request.");
                            }
                        }
                        else
                        {
                            LogEventHandler("No banked image for comparison.");
                        }
                    }
                }
                //Update tries counter
                gTriedCount++;
                //Clear galaxy from list and decrement galaxies left to image
                gList.Remove(targetName);
                GalaxyCount.Text = gList.GalaxyCount.ToString();
                Show();
                //Check for time to shut down
                LogEventHandler("Checking for ending time");
                if (Convert.ToBoolean(Launcher.CheckEnd()))
                {
                    LogEventHandler("Scan is past end time.  Shutting down.");
                    break;
                }
            }

            LogEventHandler("Session Completed");
            LogEventHandler("SuperScan successfully surveyed " + gSuccessfulCount + " out of " + gTriedCount + " galaxies.");

            //Park the telescope so it doesn't drift too low
            ss_hwp.TelescopeShutDown();
            LogEventHandler("AutoRun Running Shut Down Program **********" + "\r\n");
            Launcher.RunShutDown();
            StartScanButton.BackColor = scanbuttoncolorsave;
            return;
        }

        private void ReScan()
        {
            string curFilePath = "";
            string refFilePath = "";
            string targetName;

            //Open up the configuration parameteters, update with current form inputs
            Configuration ss_cfg = new Configuration();
            LogEventHandler("Starting ReScan");

            string ibdir = ss_cfg.ImageBankFolder;
            string[] imageBankDirs = System.IO.Directory.GetDirectories(ibdir);
            GalaxyCount.Text = imageBankDirs.Length.ToString();

            if (imageBankDirs.Length == 0)
            {
                LogEventHandler("Empty Image Bank");
                return;
            }
            foreach (string galdir in imageBankDirs)
            {
                System.IO.DirectoryInfo sys_imd = new System.IO.DirectoryInfo(galdir);
                targetName = sys_imd.Name;
                LogEventHandler("Running " + targetName);
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
                    ss_ndo.LogUpdate += LogEventHandler;
                    ss_ndo.Detect(targetName, subFrameSize, gRA, gDec, curFilePath, refFilePath, galdir);
                }
                else
                {
                    LogEventHandler("Insufficient Images");
                }
                GalaxyCount.Text = (Convert.ToInt16(GalaxyCount.Text) - 1).ToString();
            }
            LogEventHandler("ReScan Done");
            return;
        }

        private DateTime GetDate(string fname)
        {
            //fname format expected to be *_dd_MM_yyy_hh_mm.fit"
            //Parse fname the substrings, then convert to datetime
            int uscr = fname.IndexOf("_");
            string yearstr = fname.Substring(uscr + 1, 4);
            string monstr = fname.Substring(uscr + 6, 2);
            string daystr = fname.Substring(uscr + 9, 2);
            string hourstr = fname.Substring(uscr + 12, 2);
            string minstr = fname.Substring(uscr + 14, 2);
            DateTime fdate = new DateTime(
                Convert.ToInt32(yearstr),
                Convert.ToInt32(monstr),
                Convert.ToInt32(daystr),
                Convert.ToInt32(hourstr),
                Convert.ToInt32(minstr),
                0);
            return fdate;
        }

        private string FindCurrentFile(string dname)
        {
            DateTime latestDate = new DateTime(0);
            string latestFile = null;
            System.IO.DirectoryInfo sys_ibd = new System.IO.DirectoryInfo(dname);
            var galImages = sys_ibd.GetFiles("NGC*.fit");
            for (int i = 0; i < galImages.Length; i++)
            {
                if (GetDate(galImages.ElementAt(i).FullName) > latestDate)
                {
                    latestFile = galImages.ElementAt(i).FullName;
                }
            }
            return latestFile;
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
                if ((GetDate(galImages.ElementAt(i).FullName) > nextLatestDate)
                    && (galImages.ElementAt(i).FullName != latestFile))
                {
                    nextLatestFile = galImages.ElementAt(i).FullName;
                }
            }
            return nextLatestFile;
        }

        private void LogEventHandler(string logline)
        {
            ss_log.LogEntry(logline);
            LogBox.AppendText(logline + "\r\n");
            this.Show();
            System.Windows.Forms.Application.DoEvents();
            return;
        }

        private void OnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OnTopCheckBox.Checked)
            {
                this.TopMost = true;
                this.Show();
            }
            else
            {
                this.TopMost = false;
                this.Show();
            }
            return;
        }

        private void MinAltitudeSetting_ValueChanged(object sender, EventArgs e)
        {
            if (gList != null) gList.MinAltitude = (double)MinAltitudeSetting.Value;
            return;
        }

        private bool IsWeatherSafe()
        {
            //Returns true if no weather alert, false if it is unsafe

            WeatherFileReader wmon = new WeatherFileReader();
            //if no weather file or other problem, return false
            if (wmon == null) return false;
            if (wmon.AlertFlag == WeatherFileReader.WeaAlert.Alert) return false;
            else return true;
        }

        /// <summary>
        /// Runs the culling operation to clear Image Bank of all but the best image for each galaxy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


            //
        }

        /// <summary>
        /// Stores the WatchWeather control in Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatchWeatherCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            if (WatchWeatherCheckBox.Checked) ss_cfg.WatchWeather = "True";
            else ss_cfg.WatchWeather = "False";
            return;
        }

        /// <summary>
        /// Stores Dome control in settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DomeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Configuration ss_cfg = new Configuration();
            if (DomeCheckBox.Checked) ss_cfg.HasDome = "True";
            else ss_cfg.HasDome = "False";
            return;

        }
    }
}

