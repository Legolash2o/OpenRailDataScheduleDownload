using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ScheduleDownloader.Helpers
{
    public static class DownloaderHelper
    {

        /// <summary>
        /// Downloads the specific url to the specific location.
        /// </summary>
        /// <param name="url">The url / address.</param>
        /// <param name="path">Where to save the file to.</param>
        public static void Download(string url, string path)
        {
            Console.WriteLine($"Downloading {url}...");

            string usr = AppSettingsHelper.ReadSetting("feedUsername");
            //Prepares authentication information.          
            string str = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppSettingsHelper.ReadSetting("feedUsername")}:{AppSettingsHelper.ReadSetting("feedPassword")}"));

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
                string str = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppSettingsHelper.ReadSetting("feedUsername")}:{AppSettingsHelper.ReadSetting("feedPassword")}"));

                HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(url);
                gameFile.Headers[HttpRequestHeader.Authorization] = "Basic " + str;
                var resp = (HttpWebResponse)gameFile.GetResponse();
                return resp.LastModified;
            }
            catch (Exception e)
            {

                //Error 401: Invalid username/password.
                //Error 500: File is being uploaded.
            }
            return null;
        }
    }
}
