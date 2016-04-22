using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Management;
using System.Configuration;
using System.IO;
using System.Xml;

namespace FolderOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Creating Directories...");
                string root = ConfigurationManager.AppSettings["RootPath"].ToString() + GetLoggedInUser();
                string downloads = root + ConfigurationManager.AppSettings["DownloadsFolder"].ToString();
                string videos = root + ConfigurationManager.AppSettings["VideosFolder"].ToString();
                string music = root + ConfigurationManager.AppSettings["MusicFolder"].ToString();
                string pictures = root + ConfigurationManager.AppSettings["PicturesFolder"].ToString();
                    string documents = root + ConfigurationManager.AppSettings["DocumentsFolder"].ToString();

                Console.WriteLine("Creating List of files in Downloads Folder...");
                string[] fileList = Directory.GetFiles(downloads);

                Console.WriteLine("Organizing files...");

                if (fileList.Length > 0)
                {
                    foreach (var file in fileList)
                    {
                        string fileName = Path.GetFileName(file);

                        if (IsVideoFile(file))
                        {
                            File.Move(file, videos + fileName);
                            Console.WriteLine(Path.GetFileName(file) + " has been moved to 'Videos'");
                        }
                        else if (IsAudioFile(file))
                        {
                            File.Move(file, music + fileName);
                            Console.WriteLine(Path.GetFileName(file) + " has been moved to 'Music'");
                        }
                        else if (IsImageFile(file))
                        {
                            File.Move(file, pictures + fileName);
                            Console.WriteLine(Path.GetFileName(file) + " has been moved to 'Pictures'");
                        }
                        else
                        {
                            if (!Directory.Exists(documents))
                            {
                                System.IO.Directory.CreateDirectory(documents);
                            }
                            File.Move(file, documents + fileName);
                            Console.WriteLine(fileName + " has been moved to 'Uncategorized' in 'Documents'");
                        }
                    }
                }

                Console.WriteLine("Organizer has finished.");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                Console.Read();
            }

        }

        private static string GetLoggedInUser()
        {
            string userName = Environment.UserName;

            userName = userName.Replace('\\', '/');

            return userName;
        }

        private static bool IsVideoFile(string file)
        {
            var xmldoc = new XmlDocument();
            var fs = new FileStream("FileExtensions.xml", FileMode.Open, FileAccess.Read);

            xmldoc.Load(fs);

            List<string> videoExtensions = (from XmlElement element in xmldoc.GetElementsByTagName("videoExtension")
                                            select element.Attributes["extension"].Value).ToList();

            foreach (var extension in videoExtensions)
            {
                if (extension == Path.GetExtension(file))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsAudioFile(string file)
        {
            var xmldoc = new XmlDocument();
            var fs = new FileStream("FileExtensions.xml", FileMode.Open, FileAccess.Read);

            xmldoc.Load(fs);

            List<string> videoExtensions = (from XmlElement element in xmldoc.GetElementsByTagName("audioExtension")
                                            select element.Attributes["extension"].Value).ToList();

            foreach (var extension in videoExtensions)
            {
                if (extension == Path.GetExtension(file))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsImageFile(string file)
        {
            var xmldoc = new XmlDocument();
            var fs = new FileStream("FileExtensions.xml", FileMode.Open, FileAccess.Read);

            xmldoc.Load(fs);

            List<string> videoExtensions = (from XmlElement element in xmldoc.GetElementsByTagName("imageExtension")
                                            select element.Attributes["extension"].Value).ToList();

            foreach (var extension in videoExtensions)
            {
                if (extension == Path.GetExtension(file))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
