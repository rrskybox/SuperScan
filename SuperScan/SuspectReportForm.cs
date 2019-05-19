using AstroImage;
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
using System.Windows.Forms;
using TheSkyXLib;

namespace SuperScan
{
    public partial class SuspectReportForm : Form
    {
        public SuspectReportForm()
        {
            InitializeComponent();
            ListSuspects();
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
                //Add the string to the form's textbox
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
        private void SuspectListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string susitem = SuspectListbox.Items[SuspectListbox.SelectedIndex].ToString();
            int galLen = susitem.IndexOf("\t");
            string galname = susitem.Substring(0, galLen - 1);
            galname = galname.TrimEnd();
            int dateLen = susitem.IndexOf(":") - galLen + 2;
            string galevent = susitem.Substring(galLen + 1, dateLen);
            DateTime galdate = Convert.ToDateTime(galevent);
            Suspect suspect = new Suspect();
            //Check to see if the suspect loads ok, if so then
            //  have TSX upload the image files and target the RA/Dec
            bool susLoad = suspect.Load(galname, galdate);
            if (susLoad)
            {
                DrillDown ss_dd = new DrillDown(suspect.Event);

                ss_dd.Display(galname, suspect.SuspectRA, suspect.SuspectDec);
                sky6StarChart tsx_sc = new sky6StarChart();
                sky6ObjectInformation tsx_oi = new sky6ObjectInformation();
                //Get the galaxy major axis
                tsx_sc.Find(galname);
                tsx_oi.Index = 0;
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAJ_AXIS_MINS);
                double galaxis = tsx_oi.ObjInfoPropOut;
                //Set the FOV size to  the galaxy size
                tsx_sc.FieldOfView = galaxis / 60;
                //Set the center of view to the suspect's RA/Dec and light up the target icon
                //tsx_sc.Find(suspect.SuspectRA.ToString() + ", " + suspect.SuspectDec.ToString());
                tsx_sc.RightAscension = suspect.SuspectRA;
                tsx_sc.Declination = suspect.SuspectDec;
                //Check TNS for supernova reports for 60 arc seconds around this location for the last 10 days
                TNSReader tnsReport = new TNSReader();
                List<string> snList = tnsReport.RunLocaleQuery(suspect.SuspectRA, suspect.SuspectDec, 60, 10);
                if (snList != null) MessageBox.Show("Supernova " + snList[0] + " reported at this location");
                //Give the user an opportunity to clear the suspect by updating its status to cleared -- or not.
                Clipboard.Clear();
                try { Clipboard.SetText(suspect.SuspectRA.ToString() + ", " + suspect.SuspectDec.ToString()); }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
                DialogResult clearResult = MessageBox.Show(this, "Suspect at " + suspect.SuspectRA.ToString() + ", " + suspect.SuspectDec.ToString() + "\r\n" +
                    "(RA and Dec written to clipboard)" + "\r\n" +
                    "Clear Suspect?", "", MessageBoxButtons.YesNo);
                if (clearResult == DialogResult.Yes)
                {
                    suspect.Cleared = true;
                    suspect.Update();
                }
                else
                {
                    suspect.Cleared = false;
                    suspect.Update();
                }
                //Close images and Garbage collect...
                tsx_sc = null;
                tsx_oi = null;
                suspect = null;
                ss_dd = null;
                GC.Collect();
                ListSuspects();
                return;
            }
            return;
        }
    }
}
