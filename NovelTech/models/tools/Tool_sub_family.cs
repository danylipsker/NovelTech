using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NovelTech.models.tools
{
    public class Tool_sub_family : INotifyPropertyChanged
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
        public ObservableCollection<Blade_type> blades { get; set; }

        public Tool_sub_family(string name)
        {
            this.name = name;
            blades = new ObservableCollection<Blade_type>();
        }

        public Tool_sub_family()
        {
            name = "[In Creation]";
            blades = new ObservableCollection<Blade_type>();
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
