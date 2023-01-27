using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVMLibrary
{
    public class FullProperty<T> : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public object GetValue()
        {
            return value;
        }
        #endregion

        private T _value;
        public T value
        {
            get
            {
                if (getterAction != null)
                    return getterAction(_value);
                else
                    return _value;
            }
            set
            {
                _value = value;
                setterAction?.Invoke(value);
                OnPropertyChanged("value");
            }
        }

        public Func<T, T> getterAction;
        public Action<T> setterAction;

        public FullProperty(T value, Func<T, T> returnAction = null, Action<T> setterAction = null)
        {
            this.value = value;
            this.getterAction = returnAction;
            this.setterAction = setterAction;
        }
    }
}
