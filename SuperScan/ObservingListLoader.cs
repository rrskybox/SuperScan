using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace SuperScan
{
    public class ObservingListLoader
    {

        const string XdecXMLDeclaration = "<?xml version=\"1.0\"?>";
        const string XdecDocType = "<!DOCTYPE TSXObservationDatabase>";
        const string XdecStartRoot = "<TSXObservationList version=\"1.0\">";
        const string XdecEndRoot = "</TSXObservationList>";

        const string XdecStartTarget = "<target>";
        const string XdecEndTarget = "</target>";

        const string XdecStartTgtCref = "<cref>";
        const string XdecEndTgtCref = "</cref>";
        const string XdecStartTgtName = "<name>";
        const string XdecEndTgtName = "</name>";
        const string XdecStartTgtType = "<type>";
        const string XdecEndTgtType = "</type>";
        const string XdecStartTgtRA = "<raHours>";
        const string XdecEndTgtRA = "</raHours>";
        const string XdecStartTgtDec = "<decDegrees>";
        const string XdecEndTgtDec = "</decDegrees>";
        const string XdecStartTgtMag = "<magnitude>";
        const string XdecEndTgtMag = "</magnitude>";
        const string XdecStartTgtMajAxis = "<majorAxis>";
        const string XdecEndTgtMajAxis = "</majorAxis>";
        const string XdecStartTgtMinAxis = "<minorAxis>";
        const string XdecEndTgtMinAxis = "</minorAxis>";

        const int TgtCRefcolBeg = 1;
        const int TgtCRefcolEnd = 15;
        const int tgtcrefcolLen = TgtCRefcolEnd - TgtCRefcolBeg + 1;
        const int TgtNamecolBeg = 17;
        const int TgtNamecolEnd = 47;
        const int tgtNamecolLen = TgtNamecolEnd - TgtNamecolBeg + 1;
        const int TgtTypecolBeg = 49;
        const int TgtTypecolEnd = 67;
        const int tgtTypecolLen = TgtTypecolEnd - TgtTypecolBeg + 1;
        const int TgtRAcolBeg = 69;
        const int TgtRAcolEnd = 78;
        const int tgtRAcolLen = TgtRAcolEnd - TgtRAcolBeg + 1;
        const int TgtDeccolBeg = 80;
        const int TgtDeccolEnd = 89;
        const int tgtDeccolLen = TgtDeccolEnd - TgtDeccolBeg + 1;
        const int TgtMagcolBeg = 92;
        const int TgtMagcolEnd = 98;
        const int tgtMagcolLen = TgtMagcolEnd - TgtMagcolBeg + 1;
        const int TgtMajAxiscolBeg = 100;
        const int TgtMajAxiscolEnd = 107;
        const int tgtMajAxiscolLen = TgtMajAxiscolEnd - TgtMajAxiscolBeg + 1;
        const int TgtMinAxiscolBeg = 109;
        const int TgtMinAxiscolEnd = 116;
        const int tgtMinAxiscolLen = TgtMinAxiscolEnd - TgtMinAxiscolBeg + 1;

        private string TSXObsList;

        public ObservingListLoader(string olist)
        {
            //Class instanciation:
            //Park the filename without extension
            char[] period = new char[] { '.' };
            TSXObsList = (olist.Split(period, StringSplitOptions.RemoveEmptyEntries))[0];
            return;
        }

        public string GetFileName => TSXObsList;

        public XElement TSXtoXML()
        {
            //Converst observinglist .txt file to an observing list .xml file  that conforms to XML standard
            //
            //Obsfilename has no extension
            //
            string obsline;
            const int lastXMLline = 33;

           //Open the output file for writing text, overwriting if the file exists
            StreamReader obsfile = File.OpenText(TSXObsList + ".txt");

            //Write first line:  XML declaration
            string olXML = XdecXMLDeclaration + "\r\n";
            //Write second line with new document type declaration
            olXML += XdecDocType + "\r\n";
            //Write new root element declaration
            olXML += XdecStartRoot + "\r\n";
            //Read in remaining TheSky64DataHeader declarations through the close declaration
            //  line by line, up to, but not including, the last conforming line
            string throwaway;
            for (int i = 1; i < lastXMLline; i++) throwaway = obsfile.ReadLine();


            //The remainder of the stream should be a set of single lines of text containing the observation list data
            //  (Tip:  Note, however, that the only information required for reading the file back into TSX is the name, type and one digit of RA)
            //Convert each line to an xml element by bracketing it by the name "target"
            obsline = obsfile.ReadLine();
            char[] spc = new char[] { ' ' };
            while (obsline != null)
            {
                olXML += XdecStartTarget + "\r\n";
                olXML += XdecStartTgtCref + (obsline.Substring(TgtCRefcolBeg - 1, tgtcrefcolLen)).TrimEnd(spc) + XdecEndTgtCref + "\r\n";
                olXML += XdecStartTgtName + (obsline.Substring(TgtNamecolBeg - 1, tgtNamecolLen)).TrimEnd(spc) + XdecEndTgtName + "\r\n";
                olXML += XdecStartTgtType + (obsline.Substring(TgtTypecolBeg - 1, tgtTypecolLen)).TrimEnd(spc) + XdecEndTgtType + "\r\n";
                olXML += XdecStartTgtRA + (obsline.Substring(TgtRAcolBeg - 1, tgtRAcolLen)).TrimEnd(spc) + XdecEndTgtRA + "\r\n";
                olXML += XdecStartTgtDec + (obsline.Substring(TgtDeccolBeg - 1, tgtDeccolLen)).TrimEnd(spc) + XdecEndTgtDec + "\r\n";
                olXML += XdecStartTgtMag + (obsline.Substring(TgtMagcolBeg - 1, tgtMagcolLen)).TrimEnd(spc) + XdecEndTgtMag + "\r\n";
                olXML += XdecStartTgtMajAxis + (obsline.Substring(TgtMajAxiscolBeg - 1, tgtMajAxiscolLen)).TrimEnd(spc) + XdecEndTgtMajAxis + "\r\n";
                olXML += XdecStartTgtMinAxis + (obsline.Substring(TgtMinAxiscolBeg - 1, tgtMinAxiscolLen)).TrimEnd(spc) + XdecEndTgtMinAxis + "\r\n";
                olXML += XdecEndTarget + "\r\n";
                obsline = obsfile.ReadLine();
            }
            //All done with the data lines, now wrap it up with the XML closing declaration
            olXML += XdecEndRoot + "\r\n";
            //Save the text string as a new file with the same file name but the .xml extension
            //File.WriteAllText(TSXObsList + ".xml", olXML);
            XElement oXdoc = XElement.Parse(olXML);
            //return doc.DocumentElement;
            return oXdoc;
        }

        public void XMLtoTSX()
        {
            //Translates an TSXObservingList (version 1.0) XML document to an TheSky64Database (version 1.00) sort of XML document
            //This procedure recreates the TSX Observing List file without using the builtin XML parsing functions, i.e. brute force
            //  such that porting to other OS environments is easier, but does place some constraints on files that can be converted.
            //
            const string TSXOpener = "<?xml version=\"1.0\"?>" + "\r\n" +
                   "<!DOCTYPE TheSkyDatabase>" + "\r\n" +
                   "<TheSkyDatabaseHeader version=\"1.00\">" + "\r\n" +
                    "    <identifier>Observing List</identifier>" + "\r\n" +
                    "    <sdbDescription>&lt;Add Description&gt;</sdbDescription>" + "\r\n" +
                    "    <searchPrefix></searchPrefix>" + "\r\n" +
                    "    <specialSDB>1</specialSDB>" + "\r\n" +
                    "    <plotObjects>1</plotObjects>" + "\r\n" +
                    "    <plotLabels>0</plotLabels>" + "\r\n" +
                    "    <plotOrder>0</plotOrder>" + "\r\n" +
                    "    <searchable>1</searchable>" + "\r\n" +
                    "    <clickIdentify>1</clickIdentify>" + "\r\n" +
                    "    <epoch>        2000.0</epoch>" + "\r\n" +
                    "    <referenceFrame>0</referenceFrame>" + "\r\n" +
                    "    <crossReferenceType>0</crossReferenceType>" + "\r\n" +
                    "    <defaultMaxFOV>      360.0000</defaultMaxFOV>" + "\r\n" +
                    "    <defaultObjectType index=\"20\" description=\"Mixed Deep Sky\"/>" + "\r\n" +
                    "    <raHours colBeg=\"69\" colEnd=\"78\"/>" + "\r\n" +
                    "    <decDegrees colBeg=\"80\" colEnd=\"89\"/>" + "\r\n" +
                    "    <magnitude colBeg=\"91\" colEnd=\"98\"/>" + "\r\n" +
                    "    <crossReference colBeg=\"1\" colEnd=\"15\"/>" + "\r\n" +
                    "    <labelOrSearch colBeg=\"17\" colEnd=\"47\"/>" + "\r\n" +
                    "    <objectType colBeg=\"49\" colEnd=\"67\"/>" + "\r\n" +
                    "    <majorAxis colBeg=\"100\" colEnd=\"107\"/>" + "\r\n" +
                    "    <minorAxis colBeg=\"109\" colEnd=\"116\"/>" + "\r\n" +
                    "    <specialSDB>1</specialSDB>" + "\r\n" +
                    "    <reportColumns>SPROP_NAME1,SPROP_NAME2,SPROP_CO_NAME,DPROP_RA_2000,DPROP_DEC_2000,DPROP_MAG,DPROP_MAJ_AXIS_MINS,DPROP_MIN_AXIS_MINS,</reportColumns>" + "\r\n" +
                    "    <sampleColumnHeader>" + "\r\n" +
                    ";ross Reference Label/Search                    Object Type         RA Hours   Dec Degree Magnitud Major Ax Minor Ax" + "\r\n" +
                    ";-------------- ------------------------------- ------------------- ---------- ---------- -------- -------- --------" + "\r\n" +
                    "</sampleColumnHeader>" + "\r\n" +
                    "</TheSkyDatabaseHeader>" + "\r\n";

            //For the first part of the TSX Observing List text file, there is no sense building anything, just copy the header
            string olText = TSXOpener;
            string elname;
            string elvalue;
            int elnamestart = 1;
            int elnamelen;
            int elvaluestart;
            int elvaluelen;

            //The next step is to read in the XML file and parse it for "target" element entries.
            //  assume that the file existance has been validated.
            StreamReader xmlfile = System.IO.File.OpenText(TSXObsList + ".xml");
            string tline = xmlfile.ReadLine();
            //Outer loop on finding <target> element
            while (tline != null)
            {
                if (tline == XdecStartTarget)
                {
                    //Create variables to hold element data that are padded with spaces (in case no element is found)
                    string ecref = new string(' ', (tgtcrefcolLen + 1));
                    string ename = new string(' ', (tgtNamecolLen + 1));
                    string etype = new string(' ', (tgtTypecolLen + 1));
                    string era = new string(' ', (tgtRAcolLen + 1));
                    string edec = new string(' ', (tgtDeccolLen + 1));
                    string emag = new string(' ', (tgtMagcolLen + 1));
                    string emaj = new string(' ', (tgtMajAxiscolLen + 1));
                    string emin = new string(' ', (tgtMinAxiscolLen + 1));
                    //Parse target element until the end of element is found.
                    tline = xmlfile.ReadLine();
                    while (tline != XdecEndTarget)
                    {
                        //Read in the data for each expected element
                        //The line should have the format "<namestring>  some text   <\namestring>"
                        //  the namestring will be the element name and the some text we//re interested in.  
                        //  The closing declaration can be ignored except as a delimiter
                        //  This assumes that all fields are well-formed, single lines (which may not be true, eventually)
                        elnamelen = tline.IndexOf(">") - elnamestart + 1;
                        elvaluestart = elnamestart + elnamelen;
                        elvaluelen = tline.IndexOf("</") - elnamelen - 1;
                        elname = tline.Substring(elnamestart, elnamelen);
                        elvalue = tline.Substring(elvaluestart, elvaluelen);
                        //Depending on the element name, save the data accordingly, pad to correct length for TSX format
                        // The selection will fall through if any non-TSX element name is found or the closing declaration for the element is found
                        switch (elname)
                        {
                            case XdecStartTgtCref:
                                {
                                    ecref = PadFillLeft(elvalue, tgtcrefcolLen);
                                    break;
                                }
                            case XdecStartTgtName:
                                {
                                    ename = PadFillLeft(elvalue, tgtNamecolLen);
                                    break;
                                }
                            case XdecStartTgtType:
                                {
                                    etype = PadFillLeft(elvalue, tgtTypecolLen);
                                    break;
                                }
                            case XdecStartTgtRA:
                                {
                                    era = PadFillRight(elvalue, tgtRAcolLen);
                                    break;
                                }
                            case XdecStartTgtDec:
                                {
                                    edec = PadFillRight(elvalue, tgtDeccolLen);
                                    break;
                                }
                            case XdecStartTgtMag:
                                {
                                    emag = PadFillRight(elvalue, tgtMagcolLen);
                                    break;
                                }
                            case XdecStartTgtMajAxis:
                                {
                                    emaj = PadFillRight(elvalue, tgtMajAxiscolLen);
                                    break;
                                }
                            case XdecStartTgtMinAxis:
                                {
                                    emin = PadFillRight(elvalue, tgtMinAxiscolLen);
                                    break;
                                }
                        }
                        //Get the next line
                        tline = xmlfile.ReadLine();
                    }
                    //Write the text data string to the output buffer
                    olText += ecref + " " + ename + " " + etype + " " + era + " " + edec + "  " + emag + " " + emaj + " " + emin + "\r\n";
                }
                tline = xmlfile.ReadLine();
            }

            File.WriteAllText(TSXObsList + ".txt", olText);

            return;
        }

        private string PadFillRight(string instr, int len)
        {
            string outStr = instr.Substring(instr.Length - len, len);
            return outStr.PadLeft(len);
        }

        private string PadFillLeft(string instr, int len)
        {
            string outStr = instr.Substring(0, len);
            return outStr.PadRight(len);

        }
    }
}
