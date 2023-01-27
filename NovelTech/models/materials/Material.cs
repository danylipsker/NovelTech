using System;
using System.Collections.Generic;
using System.Text;

namespace NovelTech.models.materials
{
    public class Material
    {
        public int id { get; set; }
        public string name { get; set; }
        public float feedrate { get; set; }
        public float RPM { get; set; }
    }
}
