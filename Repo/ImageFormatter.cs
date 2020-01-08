using MergeTiffFiles.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace MergeTiffFiles.Repo
{
    public class ImageFormatter
    {
        public static Image FormatImageFromPath(string imagePath)
        {
            //put bitmap into image object
            Bitmap bitmap = (Bitmap)Image.FromFile(imagePath);

            //save bitmap to memory as tiff
            MemoryStream byteStream = new MemoryStream();
            bitmap.Save(byteStream, ImageFormat.Tiff);

            //put tiff into image object and return 
            return Image.FromStream(byteStream);
        }

        /// <summary>
        /// Check image front and back sides to ensure the front of the image is
        /// the actual service card information. the larger image of the two 
        /// is usually the front of the card. this method uses that logic 
        /// method will perform swap if needed 
        /// </summary>
        /// <param name="image"></param>
        public static void ValidateImageSides(ServiceCardImage image)
        {
            long frontSize = new System.IO.FileInfo(image.frontImagePath).Length;
            long backSize = new System.IO.FileInfo(image.backImagePath).Length;

            if(Math.Abs(frontSize - backSize) <= 5000)
            {
                ImageFormatCounter.imagesCloseInSize.Add(image);
            }

            if (frontSize < backSize)
            {
                string temp = image.frontImagePath;
                image.frontImagePath = image.backImagePath;
                image.backImagePath = temp;

                ImageFormatCounter.cleanImages.Add(image);
            }
        }

        public static Boolean IsMatch(ServiceCardImage image)
        {
            string frontCard = Path.GetFileNameWithoutExtension(image.frontImagePath);
            string backCard = Path.GetFileNameWithoutExtension(image.backImagePath);

            return String.Equals(frontCard.Substring(0, frontCard.Length - 1), backCard.Substring(0, backCard.Length - 1));
        }

        public static void AssignBlankBack(ServiceCardImage image)
        {
            image.backImagePath = StringVars.GetImageBlankBack(new DirectoryInfo(image.frontImagePath).Name);

            // new DirectoryInfo(StringVars.CombinedImagePath).Name
        }

        public static void FormatImageName(ServiceCardImage image)
        {
            string imageName = Path.GetFileNameWithoutExtension(image.frontImagePath);

            image.imageName = $"{imageName.Substring(0, imageName.Length - 1)}.tif";
        }
    }
}
