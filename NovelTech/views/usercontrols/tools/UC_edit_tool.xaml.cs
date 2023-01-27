using NovelTech.models.tools;
using NovelTech.viewmodels.tools;
using NovelTech.views.windows.tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovelTech.views.usercontrols.tools
{
    /// <summary>
    /// Interaction logic for UC_edit_tool.xaml
    /// </summary>
    public partial class UC_edit_tool : BaseEditItemUserControl<Tool>
    {
        public UC_edit_tool(IList parent, Tool item, Mode mode) : base(parent, item, mode)
        {
            InitializeComponent();
        }

        public override void LoadCreateUI()
        {
        }

        public override void LoadEditUI()
        {
            b_left.Content = "Update";
            b_right.Content = "Delete";
            b_revert.Visibility = Visibility.Visible;
            b_equip.Visibility = Visibility.Visible;
        }


        public override void LoadItemData()
        {
            tb_name.Text = item.name;
            tb_manufacturer.Text = item.manufacturer;
            tb_length.Text = item.length.ToString();
            tb_thickness.Text = item.thickness.ToString();
            tb_tpi.Text = item.tpi.ToString();
            tb_rpm.Text = item.rpm.ToString();
            tb_feed.Text = item.feed_rate.ToString();
            tb_plunge.Text = item.plunge_rate.ToString();
            
        }

        public override void UpdateItemData()
        {
            item.name = tb_name.Text;
            item.manufacturer = tb_manufacturer.Text;
            item.length = float.Parse(tb_length.Text);
            item.thickness = float.Parse(tb_thickness.Text);
            item.tpi = float.Parse(tb_tpi.Text);
            item.rpm = float.Parse(tb_rpm.Text);
            item.feed_rate = float.Parse(tb_feed.Text);
            item.plunge_rate = float.Parse(tb_plunge.Text);
        }

        private void b_left_Click(object sender, RoutedEventArgs e)
        {
            LeftButtonClicked();
        }

        private void b_right_Click(object sender, RoutedEventArgs e)
        {
            if(item.currently_using > 0)
            {
                MessageBox.Show($"There is currently {item.currently_using.ToString()} using that tool template. \nPlease remove them first!");
            }
            else
            {
                Delete();
            }
        }

        private void b_revert_Click(object sender, RoutedEventArgs e)
        {
            Revert();
        }

        public override bool Leaving()
        {
            throw new NotImplementedException();
        }

        private void b_equip_Click(object sender, RoutedEventArgs e)
        {
            LeftButtonClicked();
            EditToolEquippedViewModel dataContext = new EditToolEquippedViewModel(new Tool_in_action(item)) { mode = viewmodels.tools.Mode.Create};
            dataContext.OnConfirm += (sender, e) =>
            {
                viewmodels.VM_tools.toolBox.equipped.Clear();
                viewmodels.VM_tools.toolBox.equipped.Add(e.tool);
                UC_machine_table.instance.UpdateRPMFeedrateValues();//show the rpm and feedrate of the new tool
                item.currently_using++;
            };
            new W_edit_tool_in_action(dataContext).ShowDialog();
        }
        
    }
}
