/// Configuration Class
///
/// ------------------------------------------------------------------------
/// Module Name: Configuration 
/// Purpose: Store and retrieve configuration data
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description: TBD
/// 
/// ------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace SuperScan
{
    public class Configuration
    {
        //Private data
        string SuperScanFolderName = "SuperScan";
        string SuperScanQueryFolder = "Software Bisque\\TheSkyX Professional Edition\\Database Queries";

        string SuperScanImageBankFoldername = "Image Bank";
        string SuperScanGalaxyListFilename = "GalaxyList.xml";
        string SuperScanQueryFilename = "SuperScanQuery.dbq";
        string SuperScanFreshImageFilename = "FreshImage.fit";
        string SuperScanDifferenceImageFilename = "DifferenceImage.fit";
        string SuperScanLogFoldername = "Logs";
        string SuperScanFollowUpFoldername = "FollowUp";
        string SuperScanConfigurationFilename = "Configuration.xml";
        string SuperScanSuspectsFilename = "suspects.xml";

        string ssdir;

        public Configuration()
        {
            //Create the SuperScan folder path for the base folder.
            //Check to see if the TSX query file has been installed in its Software Bisque directory.  If not, install it.
            //Check to see if it exists.  If not, create the folder
            //Check to see if configuration file exists.  If not create it with defaults
            //Check to see if the Image Bank folder exists.  If not create it.
            //Check to see if the Log folder exists.  If not, create it.
            //Check to see if the FollowUp folder exists.  If not, create it.

            ssdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + SuperScanFolderName;

            InstallDBQ();

            if (!Directory.Exists(ssdir + "\\" + SuperScanFolderName))
            {
                Directory.CreateDirectory(ssdir);
            }

            if (!File.Exists(ssdir + "\\" + SuperScanConfigurationFilename))
            {
                XElement cDefaultX = new XElement("SuperScan",
                    new XElement("SuperScanFoldername", ssdir),
                    new XElement("GalaxyListPath", (ssdir + "\\" + SuperScanGalaxyListFilename)),
                    new XElement("ImageBankFoldername", (ssdir + "\\" + SuperScanImageBankFoldername)),
                    new XElement("FreshImagePath", (ssdir + "\\" + SuperScanFreshImageFilename)),
                    new XElement("DifferenceImagePath", (ssdir + "\\" + SuperScanDifferenceImageFilename)),
                    new XElement("SuperScanQueryPath", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Software Bisque\\TheSkyX Professional Edition\\Database Queries\\" + SuperScanQueryFilename),
                    new XElement("LogFoldername", (ssdir + "\\" + SuperScanLogFoldername)),
                    new XElement("FollowUpFoldername", (ssdir + "\\" + SuperScanFollowUpFoldername)),
                    new XElement("SuspectsFilePath", (ssdir + "\\" + SuperScanSuspectsFilename)),
                    new XElement("Exposure", "180"),
                    new XElement("MinimumAltitude", "30"),
                    new XElement("MinimumGalaxySize", "5"),
                    new XElement("Filter", "3"),
                    new XElement("CCDTemp", "-20"),
                    new XElement("Postpone", "false"),
                    new XElement("AutoFocus", "false"),
                    new XElement("AutoStart", "false"),
                    new XElement("StageSystemOn", "false"),
                    new XElement("StartUpOn", "false"),
                    new XElement("ShutDownOn", "false"),
                    new XElement("StageSystemTime", System.DateTime.Now.ToShortTimeString()),
                    new XElement("StartUpTime", System.DateTime.Now.ToShortTimeString()),
                    new XElement("ShutDownTime", System.DateTime.Now.ToShortTimeString()),
                    new XElement("StageSystemPath", ""),
                    new XElement("StartUpPath", ""),
                    new XElement("ShutDownPath", ""),
                    new XElement("WatchWeather", "true"));

                cDefaultX.Save(ssdir + "\\" + SuperScanConfigurationFilename);
            }

            if (!Directory.Exists(ssdir + "\\" + SuperScanImageBankFoldername))
            {
                Directory.CreateDirectory(ssdir + "\\" + SuperScanImageBankFoldername);
            }

            if (!Directory.Exists(ssdir + "\\" + SuperScanLogFoldername))
            {
                Directory.CreateDirectory(ssdir + "\\" + SuperScanLogFoldername);

            }
            if (!Directory.Exists(ssdir + "\\" + SuperScanFollowUpFoldername))
            {
                Directory.CreateDirectory(ssdir + "\\" + SuperScanFollowUpFoldername);
            }
            return;
        }

        public string GalaxyListPath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("GalaxyListPath").Value);
            }
        }

        public string DifferenceImagePath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("DifferenceImagePath").Value);
            }
        }

        public string ImageBankFolder
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("ImageBankFoldername").Value);
            }
        }

        public string QueryPath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("SuperScanQueryPath").Value);
            }
        }

        public string FreshImagePath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("FreshImagePath").Value);
            }
        }

        public string LogFolder
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("LogFoldername").Value);
            }
        }

        public string FollowUpFolder
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("FollowUpFoldername").Value);
            }
        }

        public string Exposure
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("Exposure").Value);
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("Exposure");
                sscfgXel.ReplaceWith(new XElement("Exposure", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string MinAltitude
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("MinimumAltitude").Value);
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("MinimumAltitude");
                sscfgXel.ReplaceWith(new XElement("MinimumAltitude", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string MinGalaxySize
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("MinGalaxySize").Value);
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("MinGalaxySize");
                sscfgXel.ReplaceWith(new XElement("MinGalaxySize", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string Filter
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("Filter").Value);
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("Filter");
                sscfgXel.ReplaceWith(new XElement("Filter", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string CCDTemp
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                return (sscfgXf.Element("CCDTemp").Value);
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("CCDTemp");
                sscfgXel.ReplaceWith(new XElement("CCDTemp", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string Postpone
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("Postpone") == null)
                {
                    sscfgXf.Add(new XElement("Postpone", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("Postpone").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("Postpone");
                sscfgXel.ReplaceWith(new XElement("Postpone", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string WatchWeather
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("WatchWeather") == null)
                {
                    sscfgXf.Add(new XElement("WatchWeather", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("WatchWeather").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("WatchWeather");
                if (sscfgXel == null) sscfgXf.Add(new XElement("WatchWeather", value));
                else sscfgXel.ReplaceWith(new XElement("WatchWeather", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string HasDome
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("HasDome") == null)
                {
                    sscfgXf.Add(new XElement("HasDome", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("HasDome").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("HasDome");
                if (sscfgXel == null) sscfgXf.Add(new XElement("HasDome", value));
                else sscfgXel.ReplaceWith(new XElement("HasDome", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }


        public string AutoStart
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("AutoStart") == null)
                {
                    sscfgXf.Add(new XElement("AutoStart", "false"));
                    sscfgXf.Save(sscfgfilename);
                    //Also means that the StartUp, ShutDown and AutoStart times are not in the config file
                    //  so force them
                    string assp = StageSystemPath;
                    string asup = StartUpPath;
                    string asdp = ShutDownPath;
                    string asst = StageSystemTime;
                    string asut = StartUpTime;
                    string asdt = ShutDownTime;
                    string assc = StageSystemOn;
                    string asuc = StartUpOn;
                    string asdc = ShutDownOn;
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("AutoStart").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("AutoStart");
                sscfgXel.ReplaceWith(new XElement("AutoStart", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string AutoFocus
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("AutoFocus") == null)
                {
                    sscfgXf.Add(new XElement("AutoFocus", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("AutoFocus").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("AutoFocus");
                sscfgXel.ReplaceWith(new XElement("AutoFocus", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string StageSystemOn
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("StageSystemOn") == null)
                {
                    sscfgXf.Add(new XElement("StageSystemOn", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("StageSystemOn").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("StageSystemOn");
                sscfgXel.ReplaceWith(new XElement("StageSystemOn", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string StartUpOn
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("StartUpOn") == null)
                {
                    sscfgXf.Add(new XElement("StartUpOn", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("StartUpOn").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("StartUpOn");
                sscfgXel.ReplaceWith(new XElement("StartUpOn", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string ShutDownOn
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("ShutDownOn") == null)
                {
                    sscfgXf.Add(new XElement("ShutDownOn", "false"));
                    sscfgXf.Save(sscfgfilename);
                    return ("false");
                }
                else
                {
                    return (sscfgXf.Element("ShutDownOn").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("ShutDownOn");
                sscfgXel.ReplaceWith(new XElement("ShutDownOn", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string StageSystemTime
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("StageSystemTime") == null)
                {
                    sscfgXf.Add(new XElement("StageSystemTime", System.DateTime.Now.ToShortTimeString()));
                    sscfgXf.Save(sscfgfilename);
                    return (System.DateTime.Now.ToShortTimeString());
                }
                else
                {
                    return (sscfgXf.Element("StageSystemTime").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("StageSystemTime");
                sscfgXel.ReplaceWith(new XElement("StageSystemTime", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string StartUpTime
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("StartUpTime") == null)
                {
                    sscfgXf.Add(new XElement("StartUpTime", System.DateTime.Now.ToShortTimeString()));
                    sscfgXf.Save(sscfgfilename);
                    return (System.DateTime.Now.ToShortTimeString());
                }
                else
                {
                    return (sscfgXf.Element("StartUpTime").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("StartUpTime");
                sscfgXel.ReplaceWith(new XElement("StartUpTime", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string ShutDownTime
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("ShutDownTime") == null)
                {
                    sscfgXf.Add(new XElement("ShutDownTime", (DateTime.Today + TimeSpan.FromHours(5)).ToShortTimeString()));
                    sscfgXf.Save(sscfgfilename);
                    return (System.DateTime.Now.ToShortTimeString());
                }
                else
                {
                    return (sscfgXf.Element("ShutDownTime").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("ShutDownTime");
                sscfgXel.ReplaceWith(new XElement("ShutDownTime", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string StageSystemPath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("StageSystemPath") == null)
                {
                    sscfgXf.Add(new XElement("StageSystemPath", ""));
                    sscfgXf.Save(sscfgfilename);
                    return ("");
                }
                else
                {
                    return (sscfgXf.Element("StageSystemPath").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("StageSystemPath");
                sscfgXel.ReplaceWith(new XElement("StageSystemPath", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string StartUpPath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("StartUpPath") == null)
                {
                    sscfgXf.Add(new XElement("StartUpPath", ""));
                    sscfgXf.Save(sscfgfilename);
                    return ("");
                }
                else
                {
                    return (sscfgXf.Element("StartUpPath").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("StartUpPath");
                sscfgXel.ReplaceWith(new XElement("StartUpPath", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string ShutDownPath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("ShutDownPath") == null)
                {
                    sscfgXf.Add(new XElement("ShutDownPath", ""));
                    sscfgXf.Save(sscfgfilename);
                    return ("");
                }
                else
                {
                    return (sscfgXf.Element("ShutDownPath").Value);
                }
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("ShutDownPath");
                sscfgXel.ReplaceWith(new XElement("ShutDownPath", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        public string SuspectsFilePath
        {
            get
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                if (sscfgXf.Element("SuspectsFilePath") == null)
                {
                    sscfgXf.Add(new XElement("SuspectsFilePath", ssdir + "\\" + SuperScanSuspectsFilename));
                    sscfgXf.Save(sscfgfilename);
                }
                string susPath = sscfgXf.Element("SuspectsFilePath").Value.ToString();
                return (susPath);
            }
            set
            {
                string sscfgfilename = ssdir + "\\" + SuperScanConfigurationFilename;
                XElement sscfgXf = XElement.Load(sscfgfilename);
                XElement sscfgXel = sscfgXf.Element("SuspectsFilePath");
                sscfgXel.ReplaceWith(new XElement("SuspectsFilePath", value));
                sscfgXf.Save(sscfgfilename);
                return;
            }
        }

        private void InstallDBQ()
        {
            //Installs the dbq file in the proper destination folder if it is not installed already.
            //
            //  Generate the install path from the defaults.            
            string DBQInstallPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + SuperScanQueryFolder + "\\" + SuperScanQueryFilename;
            if (!File.Exists(DBQInstallPath))
            {
                Assembly dassembly = Assembly.GetExecutingAssembly();
                //Collect the file contents to be written
                Stream dstream = dassembly.GetManifestResourceStream("SuperScan.SuperScanQuery.dbq");
                int dlen = Convert.ToInt32(dstream.Length);
                int doff = 0;
                byte[] dbytes = new byte[dstream.Length];
                int dreadout = dstream.Read(dbytes, doff, dlen);
                FileStream dbqfile = File.Create(DBQInstallPath);
                dbqfile.Close();
                //write to destination file
                File.WriteAllBytes(DBQInstallPath, dbytes);
                dstream.Close();
            }
            return;
        }

    }
}
