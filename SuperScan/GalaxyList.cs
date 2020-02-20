/// GalaxyList Class
///
/// ------------------------------------------------------------------------
/// Module Name: GalaxyList
/// Purpose: SuperScan Class for creation and management of list of galaxies to scan during a session
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description:  See SuperScanIPS.doc, Sec 2.
/// 
/// ------------------------------------------------------------------------

using System.Linq;
using System.Xml.Linq;
using TheSkyXLib;

namespace SuperScan
{
    public class GalaxyList
    {
        //Private variables

        double MinGalAlt = 30;  //Minimum altitude will default to 30 degrees if not set

        public GalaxyList()

        //Upon instantiation...
        //Open empty working XML file for galaxy list
        //Create connection to TSX DataWizard
        //Get the path to the query files, then set the path to SuperScanQuery.sdb
        //Run the DataWizard
        //Create an XML datastructure for the Observing List and load it with Observing LIst entries
        //Replace the current working Galaxy List file with the new XML data list
        //Close the file
        {
            string gname = "";
            string gRA;
            string gDec;
            string gMag;
            string gMajorAxis;
            string gMinorAxis;
            string gAltitude;
            string gHA;
            string gSide;

            XElement gXgalaxies = new XElement("TargetGalaxies");
            XElement gXrec;

            Configuration ss_cfg = new Configuration();

            ///Create object information and datawizard objects
            sky6DataWizard tsx_dw = new sky6DataWizard();
            ///Set query path 
            tsx_dw.Path = ss_cfg.QueryPath;
            tsx_dw.Open();
            string tst = tsx_dw.Path;

            sky6ObjectInformation tsx_oi = new sky6ObjectInformation();
            tsx_oi = tsx_dw.RunQuery;
            ///
            ///tsx_oi is an array (tsx_oi.Count) of object information indexed by the tsx_oi.Index property
            ///
            ///For each object information in the list, get the name, perform a "Find" and look for the catalog ID.  If there is one, print it.

            for (int i = 0; i <= (tsx_oi.Count - 1); i++)
            {
                tsx_oi.Index = i;
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_NAME1);
                gname = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
                gRA = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
                gDec = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAG);
                gMag = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MAJ_AXIS_MINS);
                gMajorAxis = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_MIN_AXIS_MINS);
                gMinorAxis = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_ALT);
                gAltitude = tsx_oi.ObjInfoPropOut.ToString();
                tsx_oi.Property(Sk6ObjectInformationProperty.sk6ObjInfoProp_HA_HOURS);
                gHA = tsx_oi.ObjInfoPropOut.ToString();

                if (System.Convert.ToDouble(gHA) < 0)
                { gSide = "East"; }
                else
                { gSide = "West"; };

                gXrec = new XElement("Galaxy",
                    new XElement("Name", gname),
                    new XElement("RA", gRA),
                    new XElement("Dec", gDec),
                    new XElement("Magnitude", gMag),
                    new XElement("MajorAxis", gMajorAxis),
                    new XElement("MinorAxis", gMinorAxis),
                    new XElement("Altitude", gAltitude),
                    new XElement("HA", gHA),
                    new XElement("Side", gSide));
                gXgalaxies.Add(gXrec);
            }

            gXgalaxies.Save(ss_cfg.GalaxyListPath);
            return;
        }


        public int GalaxyCount
        //Public Property GalaxyCount
        //
        // Returns the current number of galaxies remaining in the galaxy list
        //
        {
            get
            {
                Configuration ss_cfg = new Configuration();
                XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);
                return (gxgalaxies.Elements("Galaxy").Count());
            }
        }

        public double MinAltitude
        //Public Property MinAltitude
        // Sets (and Gets) the Minimum Altitude for a galaxy to be scanned
        //
        {
            get
            {
                return (this.MinGalAlt);
            }
            set
            {
                this.MinGalAlt = value;
            }
        }

        public string Next
        //Public Property Next
        //  Gets the next galaxy for targeting
        //
        {
            get
            {
                string tSide;
                string nextName;
                string gname;
                double gRA;
                double gDec;
                double gHA;
                string gSide;

                //Set access to configuration information
                Configuration ss_cfg = new Configuration();

                //Get the current mount position RA & Dec, and set the current hour angle flag
                //Assumes that the mount is connected and pointing at something

                sky6RASCOMTele tsx_mt = new sky6RASCOMTele();
                tsx_mt.GetRaDec();
                double tRA = tsx_mt.dRa;
                double tDec = tsx_mt.dDec;
                tsx_mt.GetAzAlt();
                double tHA = (tsx_mt.dAz - 180) * 24 / 360;

                if (tHA < 0)
                { tSide = "East"; }
                else
                { tSide = "West"; };
                //Set the least slew distance to zero
                double leastSlew = 0;
                //Load up the list of galaxies
                XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);
                //Update the hour angle for all galaxies in the list
                UpdatePosition(gxgalaxies);
                //Set the next name to nothing
                nextName = "";
                //Compute the slew distance from the current pointing to each of the galaxies, if the galaxy is on the same side
                var xgals = from grec in gxgalaxies.Elements("Galaxy") select grec;
                foreach (var grec in xgals)
                {
                    //Look up the target by galaxy name and get the hourangle
                    gname = grec.Element("Name").Value.ToString();
                    gRA = System.Convert.ToDouble(grec.Element("RA").Value.ToString());
                    gDec = System.Convert.ToDouble(grec.Element("Dec").Value.ToString());
                    gHA = System.Convert.ToDouble(grec.Element("HA").Value.ToString());
                    gSide = grec.Element("Side").Value.ToString();
                    //Get the slew distance from the current pointing, if the galaxy is on the same side as the pointing
                    if (tSide == gSide)
                    {
                        double slew = ComputeDistance(gRA, gDec, tRA, tDec);
                        if (leastSlew == 0)
                        {
                            leastSlew = slew;
                            nextName = gname;
                        }
                        else
                        {
                            if (leastSlew > slew)
                            {
                                leastSlew = slew;
                                nextName = gname;
                            }
                        }
                    }
                }
                //Check to see if there is a name in nextName, if so then return it.
                if (nextName != "")
                {
                    return (nextName);
                }
                //If not, then we'll need a meridian flip, so look to the other side
                leastSlew = 0;
                foreach (var grec in xgals)
                {
                    //Look up the target by galaxy name and get the hourangle
                    gname = grec.Element("Name").Value.ToString();
                    gRA = System.Convert.ToDouble(grec.Element("RA").Value.ToString());
                    gDec = System.Convert.ToDouble(grec.Element("Dec").Value.ToString());
                    gHA = System.Convert.ToDouble(grec.Element("HA").Value.ToString());
                    gSide = grec.Element("Side").Value.ToString();
                    //Get the slew distance from the current pointing, if the galaxy is on the same side as the pointing
                    if (tSide != gSide)
                    {
                        double slew = ComputeDistance(gRA, gDec, tRA, tDec);
                        if (leastSlew == 0)
                        {
                            leastSlew = slew;
                            nextName = gname;
                        }
                        else
                        {
                            if (leastSlew > slew)
                            {
                                leastSlew = slew;
                                nextName = gname;
                            }
                        }
                    }
                }
                //By now, either we have a galaxy name or not.  Make sure the calling function checks for a null name.
                return (nextName);
            }
        }

        public void Remove(string gname)
        //Public Method Remove
        // Deletes the galaxy entry named "gname" from the XML file database
        //
        {
            if (gname == "")
            { return; }
            Configuration ss_cfg = new Configuration();
            XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);
            var xgals = from grec in gxgalaxies.Elements("Galaxy") select grec;
            foreach (var grec in xgals)
            {
                string checkname = grec.Element("Name").Value.ToString();
                if (checkname == gname)
                {
                    grec.Remove();
                };
            }
            gxgalaxies.Save(ss_cfg.GalaxyListPath);
            return;
        }

        private XElement UpdatePosition(XElement xGalList)
        //Updates the values of HA and Altitude, then derives Side in the galaxy list by performing a TSX ComputeHA on each entry
        //
        {
            string gname;
            string gRA;
            string gHA;
            string gDec;
            string gAlt;

            //Open a TSX utility object for computing the hour angle
            sky6Utils tsx_ut = new sky6Utils();

            var xgals = from grec in xGalList.Elements("Galaxy") select grec;
            foreach (var grec in xgals)
            {
                //Look up the target by galaxy name and get the hourangle
                gname = grec.Element("Name").Value.ToString();
                gRA = grec.Element("RA").Value.ToString();
                gDec = grec.Element("Dec").Value.ToString();

                tsx_ut.ComputeHourAngle(System.Convert.ToDouble(gRA));
                gHA = tsx_ut.dOut0.ToString();
                grec.Element("HA").Value = gHA;

                tsx_ut.ConvertRADecToAzAlt(System.Convert.ToDouble(gRA), System.Convert.ToDouble(gDec));
                gAlt = tsx_ut.dOut1.ToString();
                grec.Element("Altitude").Value = gAlt;

                if (System.Convert.ToDouble(gHA) < 0)
                { grec.Element("Side").Value = "East"; }
                else
                { grec.Element("Side").Value = "West"; };
            }
            return (xGalList);
        }

        private double ComputeDistance(double ra1, double dec1, double ra2, double dec2)
        //Computes the angular distance between two polar coordinates using TSX utility function
        //
        {
            sky6Utils tsx_ut = new sky6Utils();
            tsx_ut.ComputeAngularSeparation(ra1, dec1, ra2, dec2);
            double dist = tsx_ut.dOut0;
            return dist;
        }

        public double RA(string gname)
        {
            //Look up the target by galaxy name and get the RA
            Configuration ss_cfg = new Configuration();
            XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);

            var xgals = from grec in gxgalaxies.Elements("Galaxy") select grec;
            foreach (var grec in xgals)
            {
                string checkname = grec.Element("Name").Value.ToString();
                if (checkname == gname)
                {
                    return (System.Convert.ToDouble(grec.Element("RA").Value.ToString()));
                };
            }
            return 0;
        }

        public double Dec(string gname)
        {
            //Look up the target by galaxy name and get the Dec
            Configuration ss_cfg = new Configuration();
            XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);

            var xgals = from grec in gxgalaxies.Elements("Galaxy") select grec;
            foreach (var grec in xgals)
            {
                string checkname = grec.Element("Name").Value.ToString();
                if (checkname == gname)
                {
                    return (System.Convert.ToDouble(grec.Element("Dec").Value.ToString()));
                };
            }
            return 0;
        }

        public double MaxAxis(string gname)
        {
            //Look up the target by galaxy name and get the Major Axis (in arc secs)
            Configuration ss_cfg = new Configuration();
            XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);


            var xgals = from grec in gxgalaxies.Elements("Galaxy") select grec;
            foreach (var grec in xgals)
            {
                string checkname = grec.Element("Name").Value.ToString();
                if (checkname == gname)
                {
                    return (System.Convert.ToDouble(grec.Element("MajorAxis").Value.ToString()));
                };
            }
            return 0;
        }

        public double Altitude(string gname)
        {
            //Look up the target by galaxy name and get the Altitude (in arc secs)
            Configuration ss_cfg = new Configuration();
            XElement gxgalaxies = XElement.Load(ss_cfg.GalaxyListPath);


            var xgals = from grec in gxgalaxies.Elements("Galaxy") select grec;
            foreach (var grec in xgals)
            {
                string checkname = grec.Element("Name").Value.ToString();
                if (checkname == gname)
                {
                    return (System.Convert.ToDouble(grec.Element("Altitude").Value.ToString()));
                };
            }
            return 0;
        }
    }
}
