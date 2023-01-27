using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace NovelTech.libraries
{
    public static class Bindings
    {
        public static void bind_property(string prop_path, object source, FrameworkElement target, DependencyProperty prop)
        {
            Binding binding = new Binding(prop_path);
            binding.Source = source;
            //binding.Mode = BindingMode.TwoWay;
            //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            target.SetBinding(prop, binding);
        }
    }
}
