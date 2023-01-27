using NovelTech.models.materials;
using NovelTech.models.tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelTech.models
{
    class Machine_table
    {
        public int length { get; set; }
        public int width { get; set; }

        public Machine_table(int length, int width)
        {
            this.length = length;
            this.width = width;
        }
    }
}
