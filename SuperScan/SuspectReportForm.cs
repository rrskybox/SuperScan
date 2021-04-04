/// Module Name: SuspectReportForm
/// Purpose: Presentation of light source anomoly information
/// Developer: Rick McAlister
/// Creation Date:  7/15/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description:
/// 
/// This class encapsulates methods and properties for recording and viewing
/// SuperScan suspects.  When anomolies are detected by NovaDetection in the
/// current image, the information is logged and then sent here.  This class
/// saves the data in XML format in its prospect file.  The data includes the
/// galaxy name, suspect X,Y coordinates on cropped difference image and other stuff.
/// 
/// 
/// For display,
/// --
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TheSky64Lib;

namespace SuperScan
{
    public partial class SuspectReportForm : Form
    {
        //public FitsFile af { get; set; }
        public double SuspectRA { get; set; }
        public double SuspectDec { get; set; }
        public string GalaxyName { get; set; }
        public int ImageZoom { get; set; } = 4;
        public int BlinkZoom { get; set; } = 8;
        public Suspect CurrentSuspect { get; set; }
        public DrillDown CurrentDrillDown { get; set; }
        public Image CurrentFollowUpImage { get; set; }
        public Image[] BlinkList { get; set; }

        public SuspectReportForm()
        {
            InitializeComponent();
            ListSuspects();
            BlinkButton.BackColor = Color.LightSalmon;
            ClearButton.BackColor = Color.LightSalmon;
        }

        //Close the form when close button is pushed.
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Read in suspect list
        private void ListSuspects()
        {
            Suspect susobj = new Suspect();
            List<Suspect.Perp> susList = susobj.PerpList();
            //Clear the suspect list in the form window
            SuspectListbox.Items.Clear();

            //Add the galaxy name, event date and confirmation status to the form textbox
            foreach (Suspect.Perp susp in susList)
            {
                //translate the cleared status to a string
                string susCleared;
                if (susp.Suspicion)
                {
                    susCleared = "Cleared";
                }
                else
                {
                    susCleared = "Suspect";
                }
                //Assemble a string for the text box line
                string suspectdesc = susp.Galaxy
                                     + "  \t"
                                     + susp.EventDate.ToString("yyyy-MM-dd HH:mm")
                                     + "  "
                                     + susCleared;
                //Add the string to the form//s textbox
                //List the suspect only if uncleared
                if (!susp.Suspicion)
                { SuspectListbox.Items.Add(suspectdesc); }
            }
            if (SuspectListbox.Items.Count == 0)
            //Check for an empty list, if so then report it on the form, then 
            {
                SuspectListbox.Items.Add("No Suspects in Suspect File");
            }
            return;
        }

        //When a suspect is chosen from the textbox list, parse the line for galaxy name and date
        //  then have TSX open the difference image, the reference image and the current image
        //  then have TSX set the FOV to the size of the galaxy, set the center of the chart to the
        //  coordinates of the suspect and place the target circle over the coordinates.
        //
        //Then...
        //
        private void SuspectListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ///If nothing selected then exit
            ///
            ///Parse the selected line for the galaxy name and image time.
            ///Load up the suspect data
            ///Lay in the clearer follow up image and prep to blink the two latest images
            ///

            //Set the cursor to wait
            UseWaitCursor = true;
            Show();
            FormCrunchingNotice cForm = new FormCrunchingNotice();
            cForm.Show();
            BlinkButton.BackColor = Color.LightSalmon;
            //pull the selected suspect entry from the suspect list box
            string susitem;
            try { susitem = SuspectListbox.Items[SuspectListbox.SelectedIndex].ToString(); }
            catch (Exception ex) { return; }
            //Clear the blink list
            BlinkList = null;
            //Parse the selected entry for relavent info and create a suspect object to work with
            int galLen = susitem.IndexOf("\t");
            string galname = susitem.Substring(0, galLen - 1);
            galname = galname.TrimEnd();
            int dateLen = susitem.IndexOf(":") - galLen + 2;
            string galevent = susitem.Substring(galLen + 1, dateLen);
            DateTime galdate = Convert.ToDateTime(galevent);
            CurrentSuspect = new Suspect();
            //Check to see if the stored suspect info loads ok, if so then
            //  have TSX upload the image files and target the RA/Dec
            bool susLoad = CurrentSuspect.Load(galname, galdate);
            if (susLoad)
            {
                CurrentDrillDown = new DrillDown(CurrentSuspect.Event);

                CurrentDrillDown.Display(galname, CurrentSuspect.SuspectRA, CurrentSuspect.SuspectDec);

                sky6StarChart tsx_sc = new sky6StarChart();
                sky6ObjectInformation tsx_oi = new sky6ObjectInformation();
                //Get the galaxy major axis
                tsx_sc.Find(galname);
                tsx_oi.Index = 0;
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAJ_AXIS_MINS);
                double galaxis = tsx_oi.ObjInfoPropOut;
                //Set the FOV size to  the galaxy size
                tsx_sc.FieldOfView = galaxis / 60;
                //Set the center of view to the suspect//s RA/Dec and light up the target icon
                //tsx_sc.Find(suspect.SuspectRA.ToString() + ", " + suspect.SuspectDec.ToString());
                tsx_sc.RightAscension = CurrentSuspect.SuspectRA;
                tsx_sc.Declination = CurrentSuspect.SuspectDec;
                //Check TNS for supernova reports for 60 arc seconds around this location for the last 10 days
                TNSReader tnsReport = new TNSReader();
                List<string> snList = tnsReport.RunLocaleQuery(CurrentSuspect.SuspectRA, CurrentSuspect.SuspectDec, 60, 10);
                if (snList != null) NotesTextBox.AppendText("Supernova reported for " + snList[0] + " at this location\r\n");
                else NotesTextBox.AppendText("No supernova report for this location\r\n");
                //Give the user an opportunity to clear the suspect by updating its status to cleared -- or not.
                Clipboard.Clear();
                try { Clipboard.SetText(CurrentSuspect.SuspectRA.ToString() + ", " + CurrentSuspect.SuspectDec.ToString()); }
                catch (Exception ex)
                {
                    NotesTextBox.AppendText(ex.Message);
                    return;
                }
                //Display the suspect position information
                NotesTextBox.AppendText("Suspect RA and Dec written to clipboard\r\n");
                LocationTextBox.Text = CurrentSuspect.SuspectRA.ToString() + ", " + CurrentSuspect.SuspectDec.ToString();
                Show();
                System.Windows.Forms.Application.DoEvents();
                //Show followup Image in picture box
                CurrentFollowUpImage = CurrentDrillDown.GetFollowUpImage(ImageZoom);
                ImagePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                ImagePictureBox.Image = CurrentFollowUpImage;
                BlinkButton.BackColor = Color.LightGreen;
                ClearButton.BackColor = Color.LightGreen;
                UseWaitCursor = false;
            }
            cForm.Close();
            Show();
            return;
        }

        private void BlinkButton_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            FormCrunchingNotice cForm = new FormCrunchingNotice();
            cForm.Show();
            Show();
            BlinkButton.BackColor = Color.LightSalmon;
            if (BlinkList == null)
                BlinkList = CurrentDrillDown.GetBlinkImages(BlinkZoom);
            ImagePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            ImagePictureBox.Image = BlinkList[0];
            Show();
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(2000);
            ImagePictureBox.Image = BlinkList[1];
            Show();
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(2000);
            ImagePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            ImagePictureBox.Image = CurrentFollowUpImage;
            Show();
            System.Windows.Forms.Application.DoEvents();
            BlinkButton.BackColor = Color.LightGreen;
            UseWaitCursor = false;
            cForm.Close();
            Show();
            return;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            CurrentSuspect.Cleared = true;
            CurrentSuspect.Update();
            ListSuspects();
            return;
        }
    }
}
