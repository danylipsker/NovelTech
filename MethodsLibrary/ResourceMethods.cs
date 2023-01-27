using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MethodsLibrary
{
    public static class ResourceMethods
    {
        public static void CopyResourceToFile(string resourceName, string fileName)
        {
            using (System.IO.Stream stream = 
                Application.GetResourceStream(new Uri(resourceName, UriKind.Relative)).Stream)
            {
                using (System.IO.Stream output = System.IO.File.OpenWrite(fileName))
                {
                    stream.CopyTo(output);
                }
            }
        }
    }
}
