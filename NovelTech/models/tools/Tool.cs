using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NovelTech.models.tools
{
    public class Tool : INotifyPropertyChanged
    {
        string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                Notify("name");
            }
        }
        public string manufacturer { get; set; }
        public float length { get; set; }
        public float thickness { get; set; }
        public float tpi { get; set; }
        public float rpm { get; set; }
        public float feed_rate { get; set; }
        public float plunge_rate { get; set; }
        public float work_material { get; set; }
        public float inner_diameter { get; set; }
        public float outer_diameter { get; set; }
        public float cutting_length { get; set; }
        public int currently_using { get; set; }
        public float position { get; set; }

        public Tool(string name, string manufacturer, float length, float thickness, float tpi, float rpm, float feed_rate, float plunge_rate, int currently_using, float position)
        {
            this.name = name;
            this.manufacturer = manufacturer;
            this.length = length;
            this.thickness = thickness;
            this.tpi = tpi;
            this.rpm = rpm;
            this.feed_rate = feed_rate;
            this.plunge_rate = plunge_rate;
            this.currently_using = currently_using;
            this.position = position;
        }

        public Tool()
        {
            name = "[In Creation]";
            currently_using = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
