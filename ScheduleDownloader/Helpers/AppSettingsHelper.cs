using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ScheduleDownloader.Helpers
{
    public static class AppSettingsHelper
    {
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string val = appSettings[key] ?? null;
                return val;
            }
            catch (ConfigurationErrorsException e)
            {               
                return null;
            }
        }
    }
}
