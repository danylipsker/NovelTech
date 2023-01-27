using MVVMLibrary.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVMLibrary.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public BaseViewModel()
        {
            AddCommands();
            AddDelegates();
            SetPropertyAnnotation();
        }

        protected virtual void AddCommands() { }
        protected virtual void AddDelegates() { }
        protected virtual void SetPropertyAnnotation()
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                foreach (var attribute in property.GetCustomAttributes<NotifyAttribute>())
                {

                    AddAnnotation(property.GetValue(this) as INotifyPropertyChanged, attribute.NotifyNames);
                }
            }
        }
        protected void AddAnnotation(INotifyPropertyChanged changer, string[] notifieNames)
        {
            if (changer != null && notifieNames != null)
            {
                changer.PropertyChanged += (s, e) =>
                {
                    foreach (var name in notifieNames)
                        OnPropertyChanged(name);
                };
            }
        }
    }
}
