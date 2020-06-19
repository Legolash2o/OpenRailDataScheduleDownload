using Model.Enums;
using Model.Helpers;
using Model.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ScheduleDownloader.Helpers
{
    public static class CIFHelper
    {


        public static CIF_Header GetWeekly()
        {
            string fullURL = "https://datafeeds.networkrail.co.uk/ntrod/CifFileAuthenticate?type=CIF_ALL_FULL_DAILY&day=toc-full.CIF.gz";
            var cifResult = GetCIF(fullURL);

            if (cifResult.state == DownloadState.Ok)
            {
                //Process data here.

                //Save CIF header into a database or file.
                SaveDownloaded(cifResult.cif.UploadedDate);
            }

            return cifResult.cif;
        }

        public static IEnumerable<CIF_Header> GetNightly()
        {

            for (int i = 0; i < 7; i++)
            {
                int dayOffset = i * -1;
                DateTime then = DateTime.UtcNow.AddDays(dayOffset);

                string upd = then.DayOfWeek.ToString().Substring(0, 3).ToLower();
                string url = "https://datafeeds.networkrail.co.uk/ntrod/CifFileAuthenticate?type=CIF_ALL_UPDATE_DAILY&day=toc-update-" + upd + ".CIF.gz";
                var cifResult = GetCIF(url);

                if (cifResult.state == DownloadState.Ok)
                {
                    //Process data here.

                    //Save CIF header into a database or file.
                    SaveDownloaded(cifResult.cif.UploadedDate);
                }
                yield return cifResult.cif;
            }
        }

        private static (CIF_Header cif, DownloadState state) GetCIF(string url)
        {
            var downloaded = GetDownloaded();

            DateTime? lastModified = DownloaderHelper.GetLastModified(url);

            if (lastModified == null)
                return (null, DownloadState.Error); //Error getting last modified.

            //Check if already downloaded and processed.
            if (downloaded.Any(dd => dd == lastModified.Value))
                return (null, DownloadState.AlreadyDownloaded);

            string downloadTo = $"D:\\cif_{lastModified.Value:yyyy-MM-dd_HH-mm-ss}.full.cif.gz";

            //Checks if already downloaded.
            if (!File.Exists(downloadTo))
                DownloaderHelper.Download(url, downloadTo);

            if (!File.Exists(downloadTo))
                return (null, DownloadState.Error); //Something went wrong downloading it.

            Extract.ExtractGz(new FileInfo(downloadTo));

            string cifFile = downloadTo.Substring(0, downloadTo.Length - 3);

            var cifHeader = new CIF_Header(cifFile);
            cifHeader.UploadedDate = lastModified.Value;
            return (cifHeader, DownloadState.Ok);
        }

        #region SAVING/LOADING

        //This section should be replaced with a database and take update type into consideration.

        private static string saveFile = "D:\\downloads.txt";

        private static IEnumerable<DateTime> GetDownloaded()
        {

            string line;
            if (!File.Exists(saveFile))
                yield break;

            // Read the file and display it line by line.  
            using (StreamReader file = new StreamReader(saveFile))
                while ((line = file.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    yield return DateTime.ParseExact(line.Trim(), "yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
                }

        }

        private static void SaveDownloaded(DateTime dt)
        {
            using (StreamWriter file = new StreamWriter(saveFile, true))
                file.WriteLine($"{dt:yyyy-MM-dd_HH-mm-ss}");
        }
        #endregion

    }
}
