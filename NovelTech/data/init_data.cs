using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace NovelTech.data
{
    public static class init_data
    {
        public static ObservableCollection<models.materials.Material_family> get_metarials()
        {
            if (File.Exists($"{viewmodels.VM_main.instance.appName}/materials.json"))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<models.materials.Material_family>>
                    (File.ReadAllText($"{viewmodels.VM_main.instance.appName}/materials.json"));
            }
            else
            {
                return new ObservableCollection<models.materials.Material_family>();
            }
        }
        public static List<models.tools.Tool_family> get_tools()
        {
            if (File.Exists($"{viewmodels.VM_main.instance.appName}/ToolBox.json"))
            {
                return JsonConvert.DeserializeObject<List<models.tools.Tool_family>>(File.ReadAllText($"{viewmodels.VM_main.instance.appName}/ToolBox.json"));
            }
            else
            {
                return new List<models.tools.Tool_family>();
            }
        }
        public static void save_tools(List<models.tools.Tool_family> families)
        {
            File.WriteAllText($"{viewmodels.VM_main.instance.appName}/ToolBox.json", JsonConvert.SerializeObject(families));
        }
    }
}
