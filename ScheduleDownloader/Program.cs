using Model.Helpers;
using Model.Objects;
using ScheduleDownloader.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ScheduleDownloader
{
    class Program
    {
        static void Main(string[] args)
        {

            ///The next 3 lines can be used to download a file.
            //string fullURL = "https://datafeeds.networkrail.co.uk/ntrod/CifFileAuthenticate?type=CIF_ALL_FULL_DAILY&day=toc-full";
            //string downloadTo = "D:\\example.gz";
            //Download(fullURL, downloadTo);

            //The next two calls demonstrate how you can download only new CIF files.

            while (true)
            {
                Console.WriteLine("Checking for new cifs...");
                var nightlyRes = CIFHelper.GetWeekly();
                var weeklyRes = CIFHelper.GetNightly().ToArray();
                Console.WriteLine("Finished.");

                Thread.Sleep(60000 * 60); //Every hour.
            }
        }



    }
}
