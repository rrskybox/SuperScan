

/*
* TennisNet is a Transient Name Server client for assembling supernova data
* 
* Author:           Rick McAlister
* Date:             12/21/18
* Current Version:  0.1
* Developed in:     Visual Studio 2017
* Coded in:         C 7.0
* App Envioronment: Windows 10 Pro (V1809)
* 
* Change Log:
* 
* 12/22/18 Rev 1.0  Release
* 
*/

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Windows.Forms;

namespace SuperScan
{
    public class TNSReader
    {
        //List of names of key entries
        const string startDateQ = "date_start[date]";
        const string endDateQ = "date_end[date]";
        const string raQ = "RA";
        const string decQ = "decl";
        const string radiusQ = "radius";

        const string url_tns_search = "http://wis-tns.weizmann.ac.il/search?";
        private NameValueCollection queryURL { get; set; }
        private XElement queryXResult { get; set; }

        public TNSReader()
        {
            queryURL = MakeSearchQuery();
        }
        
        public List<string> RunLocaleQuery(double raHrs, double decDeg, double radiusArcSec, int days)
        {
            //Configures the query to return the list of recent (last days) supernovae from TNS
            //  at a radius of radius arcsecs, centered on ra/dec
            //  ra in decimal degrees (0 to 360).  dec in decimal degrees (-90 to +90), radius in arcsecs
            ChangeQuery(startDateQ,DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd"));
            ChangeQuery(endDateQ, DateTime.Now.ToString("yyyy-MM-dd"));
            ChangeQuery(raQ, (raHrs*360.0/24.0).ToString());
            ChangeQuery(decQ, decDeg.ToString());
            ChangeQuery(radiusQ, radiusArcSec.ToString());
            //Run query
            RunTNSQuery();
            //queryXResult contains results
            //  if empty,then return null
            //  if entries, then make list of SN, but probably only one
            if (queryXResult==null) return null;
            else
            {
                List<string> snList = new List<string>();
                foreach (XElement xlm in queryXResult.Descendants() )
                {
                    snList.Add(xlm.Element("Name").Value);
                }
                return snList;
            }
        }

        [STAThread]

        public XElement RunTNSQuery()
        {
            // url of TNS and TNS-sandbox api                                     
            string contents;

            WebClient client = new WebClient();
            string urlQuery = url_tns_search + queryURL.ToString();
            try
            {
                contents = client.DownloadString(urlQuery);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            //Clean up the column headers so they can be used as XML item names
            string[] lines = contents.Split('\n');
            lines[0] = lines[0].Replace(" ", "_");
            lines[0] = lines[0].Replace("/", "");
            lines[0] = lines[0].Replace("(", "");
            lines[0] = lines[0].Replace(")", "");
            lines[0] = lines[0].Replace(".", "");
            lines[0] = lines[0].Replace("\"", "");

            //Split into rows and load the header line
            char[] csvSplit = new char[] { '\t' }; ;
            string[] headers = lines[0].Split(csvSplit, System.StringSplitOptions.None).Select(x => x.Trim('\"')).ToArray();

            //create an xml working database
            XElement xmlWorking = new XElement("SuperNovaList");
            for (int line = 1; line < lines.Length; line++)
            {
                lines[line] = lines[line].Replace("\"", "");
                string[] entries = lines[line].Split(csvSplit, System.StringSplitOptions.None);
                XElement xmlItem = new XElement("SNEntry");
                for (int i = 0; i < headers.Length; i++)
                {
                    xmlItem.Add(new XElement(headers[i], entries[i]));
                }
                xmlWorking.Add(xmlItem);
            }
            return xmlWorking;
        }

        private NameValueCollection MakeSearchQuery()
        {
            //Returns a url query collection for querying the TNS website

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["format"] = "tsv";

            queryString["name"] = "";
            queryString["name_like"] = "0";
            queryString["isTNS_AT"] = "all";
            queryString["public"] = "all";
            queryString["unclassified_at"] = "0";
            queryString["classified_sne"] = "1";
            queryString["ra"] = "";
            queryString["decl"] = "";
            queryString["radius"] = "";
            queryString["coords_unit"] = "arcsec";
            queryString["groupid[]"] = "null";
            queryString["classifier_groupid[]"] = "null";
            queryString["objtype[]"] = "null";
            queryString["AT_objtype[]"] = "null";
            queryString["date_start[date]"] = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            queryString["date_end[date]"] = DateTime.Now.ToString("yyyy-MM-dd");
            queryString["discovery_mag_min"] = "";
            queryString["discovery_mag_max"] = "";
            queryString["internal_name"] = "";
            queryString["redshift_min"] = "";
            queryString["redshift_max"] = "";
            queryString["spectra_count"] = "";
            queryString["discoverer"] = "";
            queryString["classifier"] = "";
            queryString["discovery_instrument[]"] = "";
            queryString["classification_instrument[]"] = "";
            queryString["hostname"] = "";
            queryString["associated_groups[]"] = "null";
            queryString["ext_catid"] = "";
            queryString["num_page"] = "50";

            //query elements specific to displaying results on TNS webpage -- unneeded but we'll keep them around
            //queryString["display[redshift]"] = "1";
            //queryString["display[hostname]"] = "1";
            //queryString["display[host_redshift]"] = "1";
            //queryString["display[source_group_name]"] = "1";
            //queryString["display[classifying_source_group_name]"] = "1";
            //queryString["display[discovering_instrument_name]"] = "0";
            //queryString["display[classifing_instrument_name]"] = "0";
            //queryString["programs_name]"] = "0";
            //queryString["internal_name]"] = "1";
            //queryString["display[isTNS_AT]"] = "0";
            //queryString["display[public]"] = "1";
            //queryString["displa[end_pop_period]"] = "0";
            //queryString["display[pectra_count]"] = "1";
            //queryString["display[discoverymag]"] = "1";
            //queryString["display[Bdiscmagfilter]"] = "1";
            //queryString["display[discoverydate]"] = "1";
            //queryString["display[discoverer]"] = "1";
            //queryString["display[sources]"] = "0";
            //queryString["display[bibcode]"] = "0";
            //queryString["display[ext_catalogs]"] = "0";

            return queryString;
        }

        private string FitFormat(string entry, int slotSize)
        {
            //Returns a string which is the entry truncated to the slot Size, if necessary
            if (entry.Length > slotSize)
                return entry.Substring(0, slotSize - 1).PadRight(slotSize);
            else
                return entry.PadRight(slotSize);
        }

        public void ChangeQuery(string queryCommand, string newValue)
        {
            //Changes the given querycommand to the newValue 
            queryURL.Set(queryCommand, newValue);
        }
    }

}


