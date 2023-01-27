using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NovelTech.libraries
{
    public class directories
    {
        public static void create_if_not_exist(string directory_name, bool replace = false)
        {
            if (Directory.Exists(directory_name) && replace)
            {
                Directory.Delete(directory_name, true);
                Directory.CreateDirectory(directory_name);
            }
            else if (!Directory.Exists(directory_name))
            {
                Directory.CreateDirectory(directory_name);
            }
        }
    }
}
