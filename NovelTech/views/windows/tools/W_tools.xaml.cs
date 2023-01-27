using NovelTech.libraries.extensions;
using NovelTech.models.tools;
using NovelTech.viewmodels;
using NovelTech.views.usercontrols.tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NovelTech.views.windows.tools
{
    /// <summary>
    /// Interaction logic for W_tools.xaml
    /// </summary>
    public partial class W_tools : Window
    {
        public W_tools()
        {
            InitializeComponent();

            tv_tools.ItemsSource = VM_tools.toolBox.families;
        }

        private void b_clear_selection_Click(object sender, RoutedEventArgs e)
        {
            tv_tools.ClearSelection();
        }

        private void b_add_item_Click(object sender, RoutedEventArgs e)
        {
            ///////צריך פה לצמצם את הכפילויות
            sp_properties.Children.Clear();
            if (tv_tools.SelectedItem != null)
            {
                switch (tv_tools.SelectedItem)
                {
                    case Tool_family parent:
                        var uc = new UC_edit_sub_family(parent.subs, new Tool_sub_family(), BaseEditItemUserControl<Tool_sub_family>.Mode.Create);
                        uc.OnDelete += Uc_OnDelete;
                        sp_properties.Children.Add(uc);
                        break;
                    case Tool_sub_family parent:
                        var uc2 = new UC_edit_blades(parent.blades, new Blade_type(), BaseEditItemUserControl<Blade_type>.Mode.Create);
                        uc2.OnDelete += Uc_OnDelete;
                        sp_properties.Children.Add(uc2);
                        break;
                    case Blade_type parent:
                        var uc3 = new UC_edit_tool(parent.tools, new Tool(), BaseEditItemUserControl<Tool>.Mode.Create);
                        uc3.OnDelete += Uc_OnDelete;
                        sp_properties.Children.Add(uc3);
                        break;
                }
            }
            else
            {
                var uc = new UC_edit_family(VM_tools.toolBox.families, new Tool_family(), BaseEditItemUserControl<Tool_family>.Mode.Create);
                uc.OnDelete += Uc_OnDelete;
                sp_properties.Children.Add(uc);
            }
        }

        private void Uc_OnDelete(object sender, EventArgs e)
        {
            sp_properties.Children.Clear();
        }

        private void tv_tools_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(tv_tools.SelectedItem == null)
            {
                sp_properties.Children.Clear();
                b_add_item.IsEnabled = true;
                b_clear_selection.IsEnabled = false;
                b_add_item.Content = "Add Family";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VM_tools.SaveToolBox();
        }

        private void tv_tools_Selected(object sender, RoutedEventArgs e)
        {
            sp_properties.Children.Clear();
            b_add_item.IsEnabled = true;
            b_clear_selection.IsEnabled = true;
            switch (tv_tools.SelectedItem)
            {
                case Tool_family item:
                    b_add_item.Content = "Add Sub Family";
                    var uc = new UC_edit_family(VM_tools.toolBox.families, item, BaseEditItemUserControl<Tool_family>.Mode.Edit);
                    uc.OnDelete += Uc_OnDelete;
                    sp_properties.Children.Add(uc);
                    break;
                case Tool_sub_family item:
                    b_add_item.Content = "Add Blade";
                    var uc2 = new UC_edit_sub_family(libraries.Treeview.GetParent<Tool_family>(e).subs, item, BaseEditItemUserControl<Tool_sub_family>.Mode.Edit);
                    uc2.OnDelete += Uc_OnDelete;
                    sp_properties.Children.Add(uc2);
                    break;
                case Blade_type item:
                    b_add_item.Content = "Add Tool";
                    var uc3 = new UC_edit_blades(libraries.Treeview.GetParent<Tool_sub_family>(e).blades, item, BaseEditItemUserControl<Blade_type>.Mode.Edit);
                    uc3.OnDelete += Uc_OnDelete;
                    sp_properties.Children.Add(uc3);
                    break;
                case Tool item:
                    b_add_item.Content = "";
                    b_add_item.IsEnabled = false;
                    var uc4 = new UC_edit_tool(libraries.Treeview.GetParent<Blade_type>(e).tools, item, BaseEditItemUserControl<Tool>.Mode.Edit);
                    uc4.OnDelete += Uc_OnDelete;
                    sp_properties.Children.Add(uc4);
                    break;
            }
        }
    }
}
