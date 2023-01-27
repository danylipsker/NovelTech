using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NovelTech.models.materials
{
    public class Material_family
    {
        public int id { get; set; }
        public string name { get; set; }
        public ObservableCollection<Material> materials = new ObservableCollection<Material>();
    }
}
