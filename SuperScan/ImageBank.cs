/// ImageBank Class
///
/// ------------------------------------------------------------------------
/// Module Name: ImageBank 
/// Purpose: Store and retrieve galaxy images
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description: TBD
/// 
/// ------------------------------------------------------------------------

using System;
using System.Linq;

namespace SuperScan
{
    public class ImageBank
    {
        string GalaxyStoreFolder;
        string GalaxyName;
        string ArchivedImagePath = "";

        public ImageBank(string targetGalaxyName)
        //Creates a new folder in ScanBank folder for Stored Images if none exists.
        // Creates a new folder for the galaxyName in the Stored Images folder if none exists.
        //  Sets the current directory path to this folder.
        //
        {
            GalaxyName = targetGalaxyName;
            Configuration ss_cfg = new Configuration();
            GalaxyStoreFolder = ss_cfg.ImageBankFolder + "\\" + targetGalaxyName;


            if (!System.IO.Directory.Exists(GalaxyStoreFolder))
            {
                System.IO.Directory.CreateDirectory(GalaxyStoreFolder);
            }
            ArchivedImagePath = GetArchivedImagePath();
            return;
        }

        public string MostRecentImagePath
        //Property that returns the path to this stored image
        {
            get
            {
                return (ArchivedImagePath);
            }
        }

        public string WorkingImageFolder
        //Property that returns the path to this stored image
        {
            get
            {
                return (GalaxyStoreFolder);
            }
        }

        public void AddImage(string fpath)
        //Copy the current Fresh Image to its galaxy image storage -- do not delete
        {
            //Configure filename
            string adt = DateTime.Now.ToString("yyyy-MM-dd-HHmm");
            string destinationImagePath = GalaxyStoreFolder + "\\" + GalaxyName + "_" + adt + ".fit";
            System.IO.File.Copy(fpath, destinationImagePath);
            return;
            //
        }

        private string GetArchivedImagePath()
        //Determines the path to the most recently stored galaxy image, if any.
        //  If none or if the image will not image link, then "" is returned.
        //Note that image file names are in the format of NGCXXX_YYYY-MM-DD-SSSS.fit
        //  where SSSS is a sequence number.  Using the file name to do a descending
        //  sort will leave the most recent file name path as the first in the list.
        {
            System.IO.DirectoryInfo sys_gbk = new System.IO.DirectoryInfo(GalaxyStoreFolder);
            if (sys_gbk.GetFiles("NGC*.fit").Count() > 0)
            {
                var latestFile = (from f in sys_gbk.GetFiles("NGC*.fit")
                                  orderby f.Name descending
                                  select f).First();
                string imagePath = sys_gbk + "\\" + latestFile.Name;
                return (imagePath);
            }
            else
            {
                //Temporary solution when storing files in archive on O: drive
                string odriveArchivePath = "O:\\SuperScan\\ImageArchive\\" + GalaxyName;
                if (System.IO.Directory.Exists(odriveArchivePath))
                {
                    System.IO.DirectoryInfo osys_gbk = new System.IO.DirectoryInfo(odriveArchivePath);
                    if (osys_gbk.GetFiles("NGC*.fit").Count() > 0)
                    {
                        var latestFile = (from f in osys_gbk.GetFiles("NGC*.fit")
                                          orderby f.Name descending
                                          select f).First();
                        string imagePath = osys_gbk + "\\" + latestFile.Name;
                        return (imagePath);
                    }
                }
                return ("");
            }

        }
    }
}

