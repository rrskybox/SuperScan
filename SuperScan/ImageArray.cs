/// ImageArray Class
///
/// ------------------------------------------------------------------------
/// Module Name: ImageArray
/// Purpose: Encapsulate data and methods for managing a TSX image DataArray
/// Developer: Rick McAlister
/// Creation Date:  6/6/2017
/// Major Modifications:
/// Copyright: Rick McAlister, 2017
/// 
/// Description:
/// 
/// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSkyXLib;

namespace SuperScan
{
    public class ImageArray
    {
        //Private class storage
        private string imageFilePath;
        private int pixelSizeX;
        private int pixelSizeY;
        int[] imagePixels;

        private int pixeldepth = 16;  //Fixed pixel depth -- May be changed latger

        public ImageArray(string filepath)
        //Open a new integer array representing the contents of a TSX image
        //  get and store the width and height in pixels for changes
        //  close the TSX window
        {
            ccdsoftImage tsx_im = new ccdsoftImage();
            imageFilePath = filepath;
            tsx_im.Path = imageFilePath;
            tsx_im.Open();
            //tsx_im.AttachToActive();
            var img = tsx_im.DataArray;
            var pix = new int[img.Length];
            for (int i = 0; i < img.Length; i++)
            {
                pix[i] = Convert.ToInt32(img[i]);
            }
            imagePixels = pix;
            pixelSizeX = tsx_im.WidthInPixels;
            pixelSizeY = tsx_im.HeightInPixels;
            tsx_im.DetachOnClose = 0;
            tsx_im.Close();
            tsx_im = null;
            pix = null;
            GC.Collect();
            return;
        }

        public ImageArray(int xSize, int ySize)
        //Open a new integer array without a file connection
        //  get and store the width and height in pixels for changes
        //  close the TSX window  
        {
            //Might want to check for excessively large array here
            ccdsoftImage tsx_im = new ccdsoftImage();
            tsx_im.New(xSize, ySize, pixeldepth);
            var img = tsx_im.DataArray;
            var pix = new int[img.Length];
            for (int i = 0; i < img.Length; i++)
            {
                pix[i] = Convert.ToInt32(img[i]);
            }
            imagePixels = pix;
            pixelSizeX = tsx_im.WidthInPixels;
            pixelSizeY = tsx_im.HeightInPixels;
            tsx_im.DetachOnClose = 0;
            tsx_im.Close();
            tsx_im = null;
            pix = null;
            GC.Collect();
            return;
        }

        public void Store(string filepath)
        //Saves the image data as a new file through TSX
            //Adjust to 256 bit depth because TSX DataArray is still broken
            //  Change later
        {
           double bitdepth = 256;
            ccdsoftImage tsx_im = new ccdsoftImage();
            tsx_im.New(pixelSizeX, pixelSizeY, pixeldepth);
            var imgarr = tsx_im.DataArray;
            for (int i = 0; i < imagePixels.Length; i++)
            {
                imgarr[i] = imagePixels[i]/bitdepth;
            }
            tsx_im.DataArray = imgarr;
            tsx_im.Path = filepath;
            tsx_im.Save();
            tsx_im.DetachOnClose = 0;
            tsx_im.Close();
            tsx_im = null;
            GC.Collect();
            return;
        }

        public int Width
        {
            get
            { return pixelSizeX; }
        }

        public int Height
        {
            get
            { return pixelSizeY; }
        }

        public int GetPixel(int xLocation, int yLocation)
        //retrieve the value at pixel location X,Y out of the image array
        //return 0 if array is out of bounds
        {
            int lPix = (yLocation * pixelSizeX) + xLocation;
            if ((lPix < imagePixels.Length) && (lPix >= 0))
            {
                return (imagePixels[lPix]);
            }
            else
            {
                return 0;
            };
        }

        public void SetPixel(int xLocation, int yLocation, int pixValue)
        //set the value at pixel location X,Y out of the image array
        {
            int lPix = (yLocation * pixelSizeX) + xLocation;
            if ((lPix < imagePixels.Length) && (lPix >=0))
            {
                imagePixels[lPix] = pixValue;
            }
            return;
        }

    }
}
