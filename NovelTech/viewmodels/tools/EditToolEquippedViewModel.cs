using MVVMLibrary.ViewModels;
using NovelTech.models.tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WPFLibrary;

namespace NovelTech.viewmodels.tools
{
    public class EditToolEquippedViewModel : BaseViewModel
    {
        #region Events
        #region EventArgs
        public class ActionToolArgs : EventArgs
        {
            public Tool_in_action tool;
        }
        #endregion
        public event EventHandler<ActionToolArgs> OnConfirm;
        public event EventHandler<ActionToolArgs> OnDelete;
        #endregion
        #region Commands

        public RelayCommand ConfirmCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }

        #endregion

        #region Properties

        #region Property - tool
        private Tool_in_action _tool;

        public Tool_in_action tool
        {
            get { return _tool; }
            set { _tool = value; OnPropertyChanged(); }
        }
        #endregion

        #endregion
        public Mode mode { get; set; }

        #region Fields
        #endregion

        #region Constructor
        public EditToolEquippedViewModel(Tool_in_action tool)
        {
            this.tool = tool;
        }
        #endregion
        #region Methods
        protected override void AddCommands()
        {
            base.AddCommands();
            ConfirmCommand = new RelayCommand(Confirm);
            RemoveCommand = new RelayCommand(Remove);
        }
        #endregion

        private void Confirm(object window)
        {
            VM_tools.SaveToolBox(); // not sure needed
            OnConfirm?.Invoke(this, new ActionToolArgs() { tool = tool }); // not sure needed
            //VM_machine_table.instance.locate_tools();
            if (mode == Mode.Create)
                VM_machine_table.instance.tools.Add(tool);
            (window as Window).Close();
        }
        private void Remove(object window)
        {
            VM_tools.toolBox.equipped.Remove(tool);
            VM_machine_table.instance.locate_tools();
            VM_tools.SaveToolBox();
            (window as Window).Close();
        }
    }
    #region Enums
    public enum Mode
    {
        Create,
        Edit
    }
    #endregion
}
