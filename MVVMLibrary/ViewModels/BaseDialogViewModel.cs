using System;
using System.Collections.Generic;
using System.Text;
using WPFLibrary;

namespace MVVMLibrary.ViewModels
{
    public abstract class BaseDialogViewModel : BaseViewModel
    {
        #region Commands
        public RelayCommand DoneCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand WindowClosingCommand { get; private set; }
        #endregion
        #region Properties
        public string WindowTitle { get; set; }
        #endregion

        protected override void AddCommands()
        {
            base.AddCommands();
            CloseCommand = new RelayCommand(w => (w as System.Windows.Window).Close());
            DoneCommand = new RelayCommand(Done, Validate);
            WindowClosingCommand = new RelayCommand(OnClosing);
        }
        public void Done(object window)
        {
            UpdateResult();
            (window as System.Windows.Window).DialogResult = true;
        }
        public abstract bool Validate(object none);
        public virtual void UpdateResult() { }

        public virtual void OnClosing(object none) { }
    }
}
