using System;
using System.Windows.Forms;

namespace SuperScan
{
    public partial class AutoRunForm : Form
    {
        public AutoRunForm()
        {
            //When an instance of the autostart form is created, 
            // the text boxes -- start up filepath, shutdown filepath, start up time
            // are filled in from the superscan configuration file.  That's it.

            InitializeComponent();
            Configuration ss_cfg = new Configuration();
            StagingDateTimePicker.Checked = Convert.ToBoolean(ss_cfg.StageSystemOn);
            StageSystemFilePathBox.Text = ss_cfg.StageSystemPath;
            StagingDateTimePicker.Value = DateTime.Now;

            StartingDateTimePicker.Checked = Convert.ToBoolean(ss_cfg.StartUpOn);
            StartUpFilePathBox.Text = ss_cfg.StartUpPath;
            StartingDateTimePicker.Value = DateTime.Now;

            ShutdownDateTimePicker.Checked = Convert.ToBoolean(ss_cfg.ShutDownOn);
            ShutDownFilePathBox.Text = ss_cfg.ShutDownPath;
            DateTime sddt = Convert.ToDateTime(ss_cfg.ShutDownTime);

            if (sddt > StartingDateTimePicker.Value) ShutdownDateTimePicker.Value = sddt;
            else ShutdownDateTimePicker.Value = sddt.AddDays(1);
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
            //  a new datetime is created from the current date and the time of day from the starttime box
            //  if this new datetime is earlier than the current date and time, then it is assumed that
            //  the user means the following morning.  If so, a day is added to this new datetime.
            //  The the startime box is updated and the new datetime is stored, as a time of day, in the 
            //   configuration file

            Configuration ss_cfg = new Configuration();
            //Store program switches
            ss_cfg.StageSystemOn = StagingDateTimePicker.Checked.ToString();
            ss_cfg.StartUpOn = StartingDateTimePicker.Checked.ToString();
            ss_cfg.ShutDownOn = ShutdownDateTimePicker.Checked.ToString();
            //Store Program times for each
            //  Get the time from the form
            //  Check the current AM or PM and compare against the program AM or PM
            //    if the same then leave the date as the same, 
            //    if different, then set date as next day (i.e. morning)
            DateTime sst = DateTime.Now.Date + StagingDateTimePicker.Value.TimeOfDay;
            if (IsTomorrowAM(sst))
            {
                sst = sst.AddDays(1);
            }

            DateTime sut = DateTime.Now.Date + StartingDateTimePicker.Value.TimeOfDay;
            if (IsTomorrowAM(sut))
            {
                sut = sut.AddDays(1);
            }

            DateTime sdt = DateTime.Now.Date + ShutdownDateTimePicker.Value.TimeOfDay;
            if (IsTomorrowAM(sdt))
            {
                sdt = sdt.AddDays(1);
            }
            //Save the entered datetimes to the configuration.xml file and close out
            StagingDateTimePicker.Value = sst;
            ss_cfg.StageSystemTime = StagingDateTimePicker.Value.ToString("yyyy/MM/dd HH:mm:ss");
            StartingDateTimePicker.Value = sut;
            ss_cfg.StartUpTime = StartingDateTimePicker.Value.ToString("yyyy/MM/dd HH:mm:ss");
            ShutdownDateTimePicker.Value = sdt;
            ss_cfg.ShutDownTime = ShutdownDateTimePicker.Value.ToString("yyyy/MM/dd HH:mm:ss");
            Close();
            return;
        }

        private Boolean IsTomorrowAM(DateTime dayTime)
        {
            //Returns true is this dayTime is between 0 and 11:59 and the current time is not, that is in the AM.
            if ((dayTime.Hour < 12) && (DateTime.Now.Hour > 12))
            { return true; }
            else
            { return false; }
        }
    }
}

