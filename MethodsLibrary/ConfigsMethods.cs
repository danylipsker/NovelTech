using System;
using System.Collections.Generic;
using System.Text;

namespace MethodsLibrary
{
    public static class ConfigsMethods
    {
        public static void ChangeNSaveConfig(string key, string value)
        {
            var configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            configFile.AppSettings.Settings[key].Value = value;
            configFile.Save();
        }
    }
}
