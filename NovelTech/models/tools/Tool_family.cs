using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NovelTech.models.tools
{
    public class Tool_family : INotifyPropertyChanged
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

        public ObservableCollection<Tool_sub_family> subs { get; set; }

        public Tool_family(string name)
        {
            this.name = name;
            subs = new ObservableCollection<Tool_sub_family>();
        }

        public Tool_family()
        {
            name = "[In Creation]";
            subs = new ObservableCollection<Tool_sub_family>();
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
