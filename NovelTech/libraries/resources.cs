using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NovelTech.libraries
{
    public static class resources
    {
        public static string get_resource_string(string resourceName)
        {
            resourceName = FormatResourceName(resourceName);
            using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (resourceStream == null)
                return null;

            using (StreamReader reader = new StreamReader(resourceStream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string FormatResourceName(string resourceName)
        {

            return Assembly.GetExecutingAssembly().GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                              .Replace("/", ".");
        }
    }
}
