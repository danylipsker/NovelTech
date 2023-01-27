using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NovelTech.models.tools
{
    public class Blade_type : INotifyPropertyChanged
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
        public ObservableCollection<Tool> tools { get; set; }

        public Blade_type(string name)
        {
            this.name = name;
            tools = new ObservableCollection<Tool>();
        }

        public Blade_type()
        {
            name = "[In Creation]";
            tools = new ObservableCollection<Tool>();
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
