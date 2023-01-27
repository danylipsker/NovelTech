using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NovelTech.views.usercontrols.tools
{
    public abstract class BaseEditItemUserControl<T> : UserControl
    {
        public IList parent;
        public T item;
        private Mode mode;

        public event EventHandler OnDelete;

        public enum Mode
        {
            Create,
            Edit
        }

        public BaseEditItemUserControl(IList parent, T item, Mode mode)
        {
            this.parent = parent;
            this.item = item;
            this.mode = mode;
            
            Initialized += BaseEditItemUserControl_Initialized;
        }

        private void BaseEditItemUserControl_Initialized(object sender, EventArgs e)
        {
            if (mode == Mode.Create)
            {
                parent.Add(item);
                LoadCreateUI();
            }
            else
            {
                LoadEditUI();
                LoadItemData();
            }
        }

        public void LeftButtonClicked()
        {
            if(mode == Mode.Create)
            {
                UpdateItemData();
                mode = Mode.Edit;
                LoadEditUI();
            }
            else
            {
                UpdateItemData();
            }
        }

        public void Delete()
        {
            parent.Remove(item);
            OnDelete(this, EventArgs.Empty);
        }

        public void Revert()
        {
            if (MessageBox.Show("Are you sure you want to revert that item ?", "Revert", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LoadItemData();
            }
        }

        public abstract void LoadCreateUI();
        public abstract void LoadEditUI();
        public abstract void LoadItemData();
        public abstract void UpdateItemData();
        public abstract bool Leaving();
    }
}
