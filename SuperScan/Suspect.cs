/// Suspect Class
///     
/// ------------------------------------------------------------------------
/// Module Name: Suspect.cs
/// Purpose: Methods and Properties for displaying and reporting suspect images
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// Description:  See SuperScanIPS.doc, Sec 2.
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TheSkyXLib;

namespace SuperScan
{
    public class Suspect
    {
        private string pImageBankPath;
        private bool pCleared;
        private DateTime pEvent;
        private string pGalName;
        private double pRA;
        private double pDec;
        private double pCurLocX;
        private double pCurLocY;

        public Suspect()
        //Class for encapsulating methods/properties of a yet unidentified suspect
        //
        {
            return;
        }

        public Suspect(string gName, DateTime sDate)
        //Class for encapsulating methods/properties of an individual suspect
        //
        {
            bool lstat = Load(gName, sDate);
            return;
        }

        //Class for encapsulating galaxyname and eventtime for suspects
        public class Perp
        {
            private bool suspicion;
            private string gName;
            private DateTime sDate;

            public Perp()
            {
                suspicion = true;
                gName = "";
                sDate = DateTime.Now;
                return;
            }

            public Perp(string galaxyName, DateTime suspectDate, bool suspectSuspicion)
            {
                suspicion = suspectSuspicion;
                gName = galaxyName;
                sDate = suspectDate;
                return;
            }
            public bool Suspicion
            {
                get
                { return suspicion; }
                set
                { suspicion = value; }
            }

            public string Galaxy
            {
                get
                { return gName; }
                set
                { gName = value; }
            }

            public DateTime EventDate
            {
                get
                {
                    return sDate;
                }
                set
                { sDate = value; }
            }
        }

        public List<Perp> PerpList()
        {
            //Reads the suspect file and lists all the galaxy names and dates
            //Return an array of "perps"

            //Generate the path to the prospect file
            Configuration ss_cfg = new Configuration();
            string susFilePath = ss_cfg.SuspectsFilePath;
            //Check for suspect file
            FileCheck(susFilePath);
            //Load the current suspect file
            XElement susXList = XElement.Load(susFilePath);
            //Get the number of suspects in the XML file
            int suspectCount = susXList.Elements("Suspect").Count();
            //Create a string array to take the galaxy and date info
            List<Perp> suslist = new List<Perp>();
            foreach (XElement suspect in susXList.Elements("Suspect"))
            {
                //Convert the XML Galaxy name to a string
                string susGalaxy = suspect.Element("GalaxyName").Value.ToString();
                //Convert the XML Event date to a datetime
                DateTime susEvent = Convert.ToDateTime(suspect.Element("SuspectEvent").Value.ToString());
                //Convert the XML Cleared status to a boolean
                bool susCleared = Convert.ToBoolean(suspect.Element("Cleared").Value.ToString());
                Perp entry = new Perp(susGalaxy, susEvent, susCleared);
                suslist.Add(entry);
            }
            return (suslist);
        }

        public bool Load(string gName, DateTime sDate)
        {
            //Loads  values from prospect file that match the galaxy name and date
            //  into the class variables.
            //Returns true if a matching element is found, false otherwise

            //Generate the path to the prospect file
            Configuration ss_cfg = new Configuration();
            string susFilePath = ss_cfg.SuspectsFilePath;
            //Check for suspect file
            FileCheck(susFilePath);
            //Load the current suspect file
            foreach (XElement suspect in XElement.Load(susFilePath).Elements("Suspect"))
            {
                //Convert the XML Event date to a datetime
                DateTime susEvent = Convert.ToDateTime(suspect.Element("SuspectEvent").Value.ToString());
                //Convert the XML Galaxy name to a string
                string susGalaxy = suspect.Element("GalaxyName").Value.ToString();
                //If the XML galaxy name and the XML date match the input galaxy name and date then read in the record
                //  and return true
                //  otherwise, return false
                if ((susGalaxy == gName) && (susEvent == sDate))
                {
                    //Found the record we were lookin for
                    //Load up the data store with it
                    //pEvent = Convert.ToDateTime(suspect.Element("SuspectEvent").ToString());
                    pEvent = susEvent;
                    pCleared = Convert.ToBoolean(suspect.Element("Cleared").Value.ToString());
                    pGalName = susGalaxy;
                    pRA = Convert.ToDouble(suspect.Element("ProspectRA").Value.ToString());
                    pDec = Convert.ToDouble(suspect.Element("ProspectDec").Value.ToString());
                    pCurLocX = Convert.ToDouble(suspect.Element("ProspectCurrentX").Value.ToString());
                    pCurLocY = Convert.ToDouble(suspect.Element("ProspectCurrentY").Value.ToString());
                    return (true);
                }
            }
            return (false);
        }

        public void Store()
        //Stores the values from the class variables to the prospects file
        {
            //Generate the path to the prospect file
            Configuration ss_cfg = new Configuration();
            string susFilePath = ss_cfg.SuspectsFilePath;
            //Create the propspect file if it doesn't exist
            FileCheck(susFilePath);
            //Load the current prospect file
            XElement proXfile = XElement.Load(susFilePath);
            //Create the new record to store
            XElement proRecord = new XElement("Suspect",
                new XElement("SuspectEvent", pEvent.ToString("yyyy-MM-dd HH:mm")),
                new XElement("Cleared", pCleared.ToString()),
                new XElement("GalaxyName", pGalName.ToString()),
                new XElement("ProspectRA", pRA.ToString()),
                new XElement("ProspectDec", pDec.ToString()),
                new XElement("ProspectCurrentX", pCurLocX.ToString()),
                new XElement("ProspectCurrentY", pCurLocY.ToString()));
            //Add the record and save the file
            proXfile.Add(proRecord);
            proXfile.Save(susFilePath);
            
            return;
        }

        public void Update()
        //Updates the values from the class variables to the prospects file
        {
            //Generate the path to the prospect file
            Configuration ss_cfg = new Configuration();
            string susFilePath = ss_cfg.SuspectsFilePath;
            //Check for file
            FileCheck(susFilePath);
            //Load the current prospect file
            XElement susXall = XElement.Load(susFilePath);
            foreach (XElement suspect in susXall.Elements("Suspect"))
            {
                DateTime susEvent = Convert.ToDateTime(suspect.Element("SuspectEvent").Value.ToString());
                string susGalaxy = suspect.Element("GalaxyName").Value.ToString();
                if ((susGalaxy == pGalName) && (susEvent == pEvent))
                {
                    //Found the record we were lookin for
                    //Create the new record to store
                    XElement newXRecord = new XElement("Suspect",
                        new XElement("SuspectEvent", pEvent.ToString("yyyy-MM-dd HH:mm")),
                        new XElement("Cleared", pCleared.ToString()),
                        new XElement("GalaxyName", pGalName.ToString()),
                        new XElement("ProspectRA", pRA.ToString()),
                        new XElement("ProspectDec", pDec.ToString()),
                        new XElement("ProspectCurrentX", pCurLocX.ToString()),
                        new XElement("ProspectCurrentY", pCurLocY.ToString()));
                    //Add the record and save the file
                    suspect.ReplaceWith(newXRecord);
                    susXall.Save(susFilePath);
                    
                    return;
                }
            }
            
            return;
        }

        private void FileCheck(string fname)
        {
            //Create the propspect file if it doesn't exist
            if (!System.IO.File.Exists(fname))
            {
                XElement susXnull = new XElement("Suspects");
                susXnull.Save(fname);
                susXnull = null;
            }
            return;
        }
        
        internal bool Cleared
        {
            get
            {
                return (pCleared);
            }
            set
            {
                pCleared = value;
            }
        }

        internal DateTime Event
        {
            get
            {
                return (pEvent);
            }
            set
            {
                pEvent = value;
            }
        }

        internal string GalaxyName
        {
            get
            {
                return (pGalName);
            }
            set
            {
                pGalName = value;
            }
        }


        internal string ImageBankPath
        {
            get
            {
                return (pImageBankPath);
            }
            set
            {
                pImageBankPath = value;
            }
        }



        internal double SuspectRA
        {
            get
            {
                return (pRA);
            }
            set
            {
                pRA = value;
            }
        }

        internal double SuspectDec
        {
            get
            {
                return (pDec);
            }
            set
            {
                pDec = value;
            }
        }

        internal double SuspectCurrentLocationX
        {
            get
            {
                return (pCurLocX);
            }
            set
            {
                pCurLocX = value;
            }
        }


        internal double SuspectCurrentLocationY
        {
            get
            {
                return (pCurLocY);
            }
            set
            {
                pCurLocY = value;
            }
        }
    }
}

