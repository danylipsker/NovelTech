using NovelTech.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NovelTech.models.tools
{
    public class ToolBox
    {
        public ObservableCollection<Tool_family> families { get; set; }
        public List<Tool_in_action> equipped { get; set; }


        public ToolBox()
        {
            families = new ObservableCollection<Tool_family>();
            equipped = new List<Tool_in_action>();
        }
    }
}

