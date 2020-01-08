using MergeTiffFiles.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace MergeTiffFiles.Repo
{
    public class FileMerger
    {
        /// <summary>
        /// merge two separate tiff files into of tiff image and place the final file in the 
        /// path currently defined by "sOutFilePath"
        /// </summary>
        /// <param name="image"></param>
        public void MergeFile(ServiceCardImage image, string path)
        {
            string sOutFilePath = $"{path}\\{image.imageName}";

            if (File.Exists(sOutFilePath))
                return;

            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
            ImageCodecInfo encoderInfo = ImageCodecInfo.GetImageEncoders().First(i => i.MimeType == "image/tiff");
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);


            //put bitmap into image object
            // Save the first frame of the multi page tiff
            Bitmap firstImage = (Bitmap)Image.FromFile(image.frontImagePath);
            firstImage.Save(sOutFilePath, encoderInfo, encoderParameters);

            encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionPage);

            // Add the second image
            Bitmap img = (Bitmap)Image.FromFile(image.backImagePath);
            firstImage.SaveAdd(img, encoderParameters);

            // Close out the file
            encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
            firstImage.SaveAdd(encoderParameters);

            //count merged card in the total number of cards 
            ImageFormatCounter.totalCardCount++;
        }
    }
}
