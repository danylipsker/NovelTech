using MVVMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelTech.models.tools
{
    public class Tool_in_action : BaseModel
    {

        public string name { get; set; } = "";

        #region Property - position
        private double _position = 25;

        public double position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - orientation
        private Orientaions _orientation = Orientaions.Head;

        public Orientaions orientation
        {
            get { return _orientation; }
            set { _orientation = value; OnPropertyChanged(); }
        }
        #endregion
        
        public Tool tool { get; set; }

        public Tool_in_action(Tool tool)
        {
            this.tool = tool;
            this.position = tool.position;
        }
        
    }
    public enum Orientaions
    {
        Left,
        Head,
        Right
    }
}
