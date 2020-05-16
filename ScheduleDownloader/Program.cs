using System;
using System.IO;
using System.Net;
using System.Text;

namespace ScheduleDownloader
{
    class Program
    {
        //username/email and password for the open data. Typically stored in App.config
        static string username = "";
        static string password = "";
        static void Main(string[] args)
        {
            string url = "https://datafeeds.networkrail.co.uk/ntrod/CifFileAuthenticate?type=CIF_ALL_FULL_DAILY&day=toc-full";

            DateTime? lastModified = GetLastModified(url);

            string downloadTo = "D:\\example.gz";
            Download(url, downloadTo);

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Downloads the specific url to the specific location.
        /// </summary>
        /// <param name="url">The url / address.</param>
        /// <param name="path">Where to save the file to.</param>
        static void Download(string url, string path)
        {
            Console.WriteLine($"Downloading {url}...");

            //Prepares authentication information.          
            string str = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            //Downloads the data at the specific url.
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + str;
            byte[] buffer = new byte[4096];

            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        int count;
                        int counter = 0;

                        do
                        {
                            count = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, count);
                            counter++;
                        }
                        while (count != 0);

                        byte[] bytes = memoryStream.ToArray();
                        File.WriteAllBytes(path, bytes);
                    }
                }
            }
        }

        /// <summary>
        /// Return last modified date for an online file using the header.
        /// </summary>
        /// <param name="url">The web url</param>
        /// <returns></returns>
        public static DateTime? GetLastModified(string url)
        {
            try
            {
                string str = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

                HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(url);
                gameFile.Headers[HttpRequestHeader.Authorization] = "Basic " + str;
                var resp = (HttpWebResponse)gameFile.GetResponse();
                return resp.LastModified;
            }
            catch (Exception e)
            {
               
            }
            return null;
        }

    }
}
