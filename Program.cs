using MergeTiffFiles.Models;
using MergeTiffFiles.Repo;
using System;
using System.IO;

namespace MergeTiffFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Choose folder that contains the Service Card images (type Q to quit):\n");
                StringVars.ScannedImagePath = Console.ReadLine();

                if (StringVars.ScannedImagePath.Equals("q", StringComparison.OrdinalIgnoreCase))
                    break;

                //exit program if start or end path are not valid
                if (!System.IO.Directory.Exists(StringVars.ScannedImagePath))
                {
                    Console.Clear();
                    Console.WriteLine("File path(s) not valid");
                    Console.ReadKey();
                }
                else
                {
                    StringVars.SetCombinedImagesPath();

                    Logger.LogToScreen("Starting process");
                    //Logger.DeleteOldFiles();

                    //create directory for combined cards
                    string dirName = new DirectoryInfo(StringVars.CombinedImagePath).Name;

                    Logger.init(dirName);
                    Logger.LogToText($"Card QA For {dirName}:");
                    ImageFormatCounter.ServiceCenterName = dirName;
                    string newPath = StringVars.GetCombinedImagePath();

                    //create new folder (if needed) for combined images
                    Directory.CreateDirectory(newPath);

                    FileMerger merger = new FileMerger();
                    //Console.Clear();
                    //Logger.LogToScreen($"Combining images from {dir}");

                    var directoryFiles = Directory.GetFiles(StringVars.ScannedImagePath);

                    int previousPercentage = 0;

                    for (int i = 0; i + 1 < directoryFiles.Length; i++)
                    {
                        //DELETE AFTERWARDS
                        ServiceCardImage scImage = new ServiceCardImage(directoryFiles[i + 1], directoryFiles[i]);
                        //ServiceCardImage scImage = new ServiceCardImage(directoryFiles[i], directoryFiles[i + 1]);
                        int percentComplete = (int)Math.Round((double)(100 * i) / directoryFiles.Length);

                        //print completion percention to console
                        if (percentComplete % 5 == 0 && percentComplete != previousPercentage)
                        {
                            Console.Clear();
                            Logger.LogToScreen($"Combining images from {StringVars.ScannedImagePath}\n ({percentComplete}%)");
                            previousPercentage = percentComplete;
                        }

                        /* if front and back of image don't match, it's possible only the front of the image
                         * was scanned 
                         * */
                        if (!ImageFormatter.IsMatch(scImage))
                        {
                            //check if it's not the first card in the list. otherwise, checking the previous card
                            //will throw an index error if index is still 0
                            //CURRENTLY - THIS LOGIC DOES NOT ACCOUNT FOR IF THE FIRST IMAGE IN THE LIST DOES NOT 
                            //HAVE A MATCH
                            if (i != 0)
                            {
                                var tempImage = new ServiceCardImage(directoryFiles[i], directoryFiles[i - 1]);
                                if (!ImageFormatter.IsMatch(tempImage))
                                {
                                    //ImageFormatCounter.totalCardCount++;
                                    ImageFormatter.FormatImageName(tempImage);
                                    ImageFormatter.AssignBlankBack(tempImage);
                                    scImage = tempImage;

                                    ImageFormatCounter.noBackImage.Add(scImage);

                                    merger.MergeFile(scImage, newPath);
                                }
                            }
                        }
                        //the pre-existing images selected do exists. proceed as normal with combining the images
                        else
                        {
                            ImageFormatCounter.totalCardCount++;

                            ImageFormatter.FormatImageName(scImage);
                            //ONLY FOR CHATTANOOGA SERVICE CARDS -- REMOVE AFTER AFTERWARDS
                            ImageFormatter.ValidateImageSides(scImage);

                            merger.MergeFile(scImage, newPath);
                        }

                    }
                    ImageFormatCounter.LogInfoToText();
                    ImageFormatCounter.Clear();
                    Logger.LogToText("Done");
                    Logger.Close();
                    Console.Clear();
                }

                
            }

            Console.Clear();
            Logger.LogToScreen("Done");
            Console.ReadKey();
        }

        static void ClearImageFolder(string imagesPath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(imagesPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception e)
            {
                Logger.LogToScreen($"Error occured: {e.Message}");
                throw;
            }
        }
    }
}