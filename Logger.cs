using System;
using System.IO;

namespace MergeTiffFiles
{
    public static class Logger
    {
        private static TextWriter tw;
        private static string logPath { get; set; }// = "\\\\GAXGPFS01\\ADALLIS$\\Desktop\\MergeTiffFilesLog";
        public static void init(string name)
        {
            logPath = $"{StringVars.CombinedImagePath}\\LogFile_{GetDateTime()}.txt";
            tw = new StreamWriter(logPath);
        }

        public static void Log(string message)
        {
            try
            {
                //tw.WriteLine($"{message}");
                Console.WriteLine($"{GetDateTime()} -- {message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetDateTime()} -- ERROR OCCURED -- {e.Message}");
            }
        }

        public static void LogToText(string message)
        {
            try
            {
                tw.WriteLine($"{message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetDateTime()} -- ERROR OCCURED -- {e.Message}");
            }
        }

        public static void DeleteOldFiles()
        { 
            //string rootPath = System.IO.Directory.GetParent(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()))).ToString();
            //string rootPath = System.IO.Directory.GetCurrentDirectory().ToString();
            DirectoryInfo di = new DirectoryInfo(logPath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        public static void LogToScreen(string message)
        {
            try
            {
                Console.WriteLine($"{GetDateTime()} -- {message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetDateTime()} -- ERROR OCCURED -- {e.Message}");
            }
        }

        public static void Close()
        {
            tw.Close();
        }

        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd__HHmmss");
        }
    }
}
