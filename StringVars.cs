using System.IO;

namespace MergeTiffFiles
{
    public class StringVars
    {
        public static string ScannedImagePath{ get; set; }
        public static string CombinedImagePath { get; set; }

        public static string GetScannedImagePath()
        {
            return ScannedImagePath;
        }

        public static string GetCombinedImagePath()
        {
            return $"{CombinedImagePath}\\CombinedImages";
        }

        public static string GetImageBlankBack(string imageName)
        {
            return $"\\\\GAXGPFS01\\ADALLIS$\\default_blank_back.tif";
        }

        public static void SetCombinedImagesPath()
        {
            CombinedImagePath = Directory.GetParent(ScannedImagePath).ToString();
        }
    }
}
