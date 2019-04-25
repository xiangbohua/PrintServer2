using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PrintService.Utility
{
    /// <summary>
    /// Helper for appSetting
    /// </summary>
    public class AppSettingHelper
    {
        /// <summary>
        /// Ttry to get value from appSetting with a seting name
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetOne(string settingName)
        {
            var config = GetConfiguration();

            if (config.AppSettings.Settings.AllKeys.Any(key => key == settingName)) {
                return config.AppSettings.Settings[settingName].Value;
            }

            return "";
        }

        /// <summary>
        /// Ttry to get value from appSetting with a seting name
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetOne(string settingName, string defaultValue)
        {
            var setting = GetOne(settingName);
            if ("" == setting)
            {
                return defaultValue;
            }
            return setting;
        }

        /// <summary>
        /// Get the configuration object
        /// </summary>
        /// <returns></returns>
        private static Configuration GetConfiguration()
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            var config = ConfigurationManager.OpenExeConfiguration(file);
            return config;
        }

        /// <summary>
        /// Try to update setting
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="newValue"></param>
        public static void SetOne(string settingName, string newValue)
        {
            var config = GetConfiguration();
                   
            if (config.AppSettings.Settings.AllKeys.Any(key => key == settingName))
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
