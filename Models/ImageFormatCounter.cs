using System;
using System.Collections.Generic;
using System.Text;

namespace MergeTiffFiles.Models
{
    public static class ImageFormatCounter
    {
        public static string ServiceCenterName { get; set; }
        public static int totalCardCount = 0;
        public static List<ServiceCardImage> cleanImages = new List<ServiceCardImage>();
        public static List<ServiceCardImage> imagesCloseInSize = new List<ServiceCardImage>();
        public static List<ServiceCardImage> noBackImage = new List<ServiceCardImage>();

        public static void Clear()
        {
            cleanImages.Clear();
            imagesCloseInSize.Clear();
            noBackImage.Clear();
            totalCardCount = 0;
        }

        public static void LogInfoToText()
        {
            Logger.LogToText($"Service Center: {ServiceCenterName}");
            Logger.LogToText($"Cards Cleaned: {cleanImages.Count}");
            Logger.LogToText($"Images with 5KB or less size difference: {imagesCloseInSize.Count}");
            Logger.LogToText($"Images with no back image: {noBackImage.Count}");
            Logger.LogToText($"Total Cards: {totalCardCount}");
            
            Logger.LogToText("------------------------------------------");
            Logger.LogToText("Images close in size:");
            foreach (var image in imagesCloseInSize)
            {
                Logger.LogToText(image.imageName);
            }
            Logger.LogToText("------------------------------------------");
            Logger.LogToText("Cleaned Images:");
            foreach (var image in cleanImages)
            {
                Logger.LogToText(image.imageName);
            }
            Logger.LogToText("------------------------------------------");
            Logger.LogToText("Images with no Back:");
            foreach (var image in noBackImage)
            {
                Logger.LogToText(image.imageName);
            }
            Logger.LogToText("*******************************************");
            Logger.LogToText("*******************************************");
        }
    }
}
