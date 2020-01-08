using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MergeTiffFiles
{
    public class ServiceCardImage
    {
        public string imageName { get; set; }
        public string frontImagePath { get; set; }
        public string backImagePath { get; set; }

        public ServiceCardImage() { }

        public ServiceCardImage(string backPath, string frontPath)
        {
            frontImagePath = frontPath;
            backImagePath = backPath;
        }
    }
}
