using Newtonsoft.Json;
using NovelTech.models.tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace NovelTech.viewmodels
{
    public class VM_tools
    {
        public static string FileName = "ToolBox";

        public static ToolBox toolBox = GetToolBox();

        private static ToolBox GetToolBox()
        {
            ToolBox toolBox = new ToolBox();
            string path = $"{Directory.GetCurrentDirectory()}\\{VM_main.instance.appName}\\{FileName}.json";
            if (File.Exists(path))
            {
                toolBox = JsonConvert.DeserializeObject<ToolBox>(File.ReadAllText(path));
            }
            return toolBox;
        }

        public static void SaveToolBox()
        {
            File.WriteAllText(VM_main.instance.appName + "/" + FileName + ".json", JsonConvert.SerializeObject(toolBox));
        }

        public static void AddItem(object parent)
        {
            if (parent != null)
            {
                switch (parent)
                {
                    case Tool_family family:
                        family.subs.Add(new Tool_sub_family());
                        break;
                    case Tool_sub_family sub:
                        sub.blades.Add(new Blade_type());
                        break;
                    case Blade_type blade:
                        blade.tools.Add(new Tool());
                        break;
                }
            }
            else
            {
                VM_tools.toolBox.families.Add(new Tool_family());
            }
        }
    }
}
