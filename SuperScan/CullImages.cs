///
/// /// ------------------------------------------------------------------------
/// Module Name: CullImages
/// Purpose: Encapsulates data and methods for safely removing old images
/// Developer: Rick McAlister
/// Creation Date:  2/8/20
/// Major Modifications:
/// Copyright: Rick McAlister, 2020
/// 
/// Description:See SuperScanDescription.docx
/// 
/// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheSkyXLib;

namespace SuperScan
{
    /// <summary>
    /// Static class for removing the worst of the old images
    /// </summary>
    public static class CullImages
    {
        public static void DeletellButBest()
        {
            /// For each directory in the Image Bank, 
            /// 
            ///      For each image file in the directory, Inventory the image, storing the star count
            ///      Determine the most recent image (to break ties) with the largest star count
            ///      Delete all other images.
            ///      

            //Get Image Bank location
            Configuration sscfg = new Configuration();
            string ibPath = sscfg.ImageBankFolder;
            //Form list of Image Bank directories
            List<string> gDirList = Directory.GetDirectories(ibPath).ToList<string>();
            List<string> gImageList;

            //step through each diretory
            foreach (string gDir in gDirList)
            {
                //form list of images in the galaxy//s directory
                gImageList = Directory.GetFiles(gDir).ToList<string>();
                //delete all the *.src, *.apm files and Cropped (ImageLink Allsky fits files)
                foreach (string imagePath in gImageList)
                {
                    if (imagePath.Contains(".SRC"))
                        File.Delete(imagePath);
                    if (imagePath.Contains(".apm"))
                        File.Delete(imagePath);
                    if (imagePath.Contains("Cropped"))
                        File.Delete(imagePath);
                }
                //set baseline for minimal star count
                int bestStarCount = 0;
                string bestImage = null;
                //Load up the filelist again
                gImageList = Directory.GetFiles(gDir).ToList<string>();
                //step through each image
                foreach (string imagePath in gImageList)
                {
                    //Skip over any of the working image files
                    if (!(imagePath.Contains("Image.fit")))
                    {
                        //Have TSX inventory the image for the number of stars
                        int starCount = CountStars(imagePath);
                        //If the number of stars in the image is greater, then note the image name and count
                        //  otherwise, delete the image
                        if (starCount > bestStarCount)
                        {
                            if (bestImage != null)
                                File.Delete(bestImage);
                            bestStarCount = starCount;
                            bestImage = imagePath;
                        }
                        else
                        {
                            //delete this second best image...
                            File.Delete(imagePath);
                        }
                    }
                    //Next...
                }
                //Load up the filelist again
                gImageList = Directory.GetFiles(gDir).ToList<string>();
                //delete all the *.src, *.apm files and Cropped (ImageLink Allsky fits files) -- Again
                foreach (string imagePath in gImageList)
                {
                    if (imagePath.Contains(".SRC"))
                        File.Delete(imagePath);
                    if (imagePath.Contains(".apm"))
                        File.Delete(imagePath);
                    if (imagePath.Contains("Cropped"))
                        File.Delete(imagePath);
                }
            }


        }

        /// <summary>
        /// Asks TSX for the star count in an image
        /// </summary>
        /// <param name="fPath"></param>
        /// <returns></returns>
        private static int CountStars(string fPath)
        {
            //Have TSX open the fits file fPath
            ImageLink tsxil = new ImageLink();
            tsxil.pathToFITS = fPath;
            //Image Link the image, return 0 if it fails
            try { tsxil.execute(); }
            catch (Exception ex) { return 0; }
            //return the count of stars
            ImageLinkResults tsxilr = new ImageLinkResults();
            return (tsxilr.imageStarCount);
        }

    }
}
