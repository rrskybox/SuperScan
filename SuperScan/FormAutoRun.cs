using System;
using System.Windows.Forms;

namespace SuperScan
{
    public partial class FormAutoRun : Form
    {
        public FormAutoRun()
        {
            //When an instance of the autostart form is created, 
            // the text boxes -- start up filepath, shutdown filepath, start up time
            // are filled in from the superscan configuration file.  That//s it.

            InitializeComponent();
            Configuration ss_cfg = new Configuration();
            StagingDateTimePicker.Checked = Convert.ToBoolean(ss_cfg.StageSystemOn);
            StageSystemFilePathBox.Text = ss_cfg.StageSystemPath;
            StagingDateTimePicker.Value = Convert.ToDateTime(ss_cfg.StageSystemTime);

            StartingDateTimePicker.Checked = Convert.ToBoolean(ss_cfg.StartUpOn);
            StartUpFilePathBox.Text = ss_cfg.StartUpPath;
            StartingDateTimePicker.Value = Convert.ToDateTime(ss_cfg.StartUpTime);

            ShutdownDateTimePicker.Checked = Convert.ToBoolean(ss_cfg.ShutDownOn);
            ShutDownFilePathBox.Text = ss_cfg.ShutDownPath;
            ShutdownDateTimePicker.Value = Convert.ToDateTime(ss_cfg.ShutDownTime);

            //if (sddt > StartingDateTimePicker.Value) ShutdownDateTimePicker.Value = sddt;
            //else ShutdownDateTimePicker.Value = sddt.AddDays(1);

            ResyncSchedule();
        }

        private void StageSystemBrowseButton_Click(object sender, EventArgs e)
        {
            //Upon clicking the Browse button on the Stage System filename box,
            //  A file selection dialog is run to pick up a filepath for the
            //  system staging file.  The result, if chosen, is entered in the stage System filename box
            //  in the form, and the superscan configuration file updated accordingly.

            DialogResult stageSystemPathDiag = StageSystemFileDialog.ShowDialog();
            if (stageSystemPathDiag == System.Windows.Forms.DialogResult.OK)
            {
                Configuration ss_cfg = new Configuration();
                ss_cfg.StageSystemPath = StageSystemFileDialog.FileName;
                StageSystemFilePathBox.Text = StageSystemFileDialog.FileName;
                return;
            }
        }

        private void StartUpBrowseButton_Click(object sender, EventArgs e)
        {
            //Upon clicking the Browse button on the Start Up filename box,
            //  A file selection dialog is run to pick up a filepath for the
            //  start up file.  The result, if chosen, is entered in the start up filename box
            //  in the form, and the superscan configuration file updated accordingly.

            DialogResult startUpPathDiag = StartUpFileDialog.ShowDialog();
            if (startUpPathDiag == System.Windows.Forms.DialogResult.OK)
            {
                Configuration ss_cfg = new Configuration();
                ss_cfg.StartUpPath = StartUpFileDialog.FileName;
                StartUpFilePathBox.Text = StartUpFileDialog.FileName;
                return;
            }
        }

        private void ShutDownBrowseButton_Click(object sender, EventArgs e)
        {
            //Upon clicking the Browse button on the Shutdown filename box,
            //  A file selection dialog is run to pick up a filepath for the
            //  shutdown file.  The result, if chosen, is entered in the shutdown filename box
            //  in the form, and the superscan configuration file updated accordingly.

            DialogResult shutDownPathDiag = ShutDownFileDialog.ShowDialog();
            if (shutDownPathDiag == System.Windows.Forms.DialogResult.OK)
            {
                Configuration ss_cfg = new Configuration();
                ss_cfg.ShutDownPath = ShutDownFileDialog.FileName;
                ShutDownFilePathBox.Text = ShutDownFileDialog.FileName;
                return;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            //Upon clicking the OK button,
            //Save configured times and close form
            Configuration ss_cfg = new Configuration();
            ss_cfg.StageSystemTime = StagingDateTimePicker.Value.ToString("yyyy/MM/dd HH:mm:ss");
            ss_cfg.StartUpTime = StartingDateTimePicker.Value.ToString("yyyy/MM/dd HH:mm:ss");
            ss_cfg.ShutDownTime = ShutdownDateTimePicker.Value.ToString("yyyy/MM/dd HH:mm:ss");
            //Store program switches
            ss_cfg.StageSystemOn = StagingDateTimePicker.Checked.ToString();
            ss_cfg.StartUpOn = StartingDateTimePicker.Checked.ToString();
            ss_cfg.ShutDownOn = ShutdownDateTimePicker.Checked.ToString();
            Close();
            return;
        }

        private void ResyncSchedule()
        {

            //Values for stage, start and shutdown times are reset to the current datetime, if
            //their current values precede the current datetime.
            //Otherwise, it is assumpted that the operator knows what he/she is doing

            Configuration ss_cfg = new Configuration();
            //Store Program times for each
            //  Get the time from the form
            //  Check the current AM or PM and compare against the program AM or PM
            //    if the same then leave the date as the same, 
            //    if different, then set date as next day (i.e. morning)
            DateTime currentMoment = DateTime.Now;
            Boolean currentlyAM = IsAM(currentMoment);

            //if stage time is earlier than current time then set stage time to now
            DateTime sst = StagingDateTimePicker.Value;
            if (sst < currentMoment) sst = DateTime.Now;

            //if start time is earlier than current time then set start to now
            DateTime sut = StartingDateTimePicker.Value;
            if (sut < currentMoment) sut = DateTime.Now;

            //If end time is earlier than current time then change the end time
            //  to tomorrow, at the same time of day
            DateTime sdt = ShutdownDateTimePicker.Value;
            if (sdt < currentMoment) sdt = DateTime.Now.Date.AddDays(1) + sdt.TimeOfDay;
            //figure out how long this session is going to be.  If greater than a day, cut the
            // shut down time by one day.
            while ((sdt - sut).Days >= 1) sdt = sdt.AddDays(-1);
            //Save the entered datetimes to the configuration.xml file and close out
            StagingDateTimePicker.Value = sst;
            StartingDateTimePicker.Value = sut;
            ShutdownDateTimePicker.Value = sdt;
        }

        private Boolean IsAM(DateTime dayTime)
        {
            //Returns true is this dayTime is between 0 and 11:59 and the current time is not, that is in the AM.
            if ((dayTime.Hour < 12) && (DateTime.Now.Hour > 12))
            { return true; }
            else
            { return false; }
        }

        private DateTime UpdateToNow(DateTime oldDate)
        {
            //if oldDate is earlier than the current datetime, then return the
            //current datetime, otherwise return the old datetime
            if (oldDate < DateTime.Now) return DateTime.Now;
            return oldDate;
        }
    }
}

