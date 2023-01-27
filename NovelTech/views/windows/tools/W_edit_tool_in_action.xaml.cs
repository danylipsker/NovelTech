using NovelTech.models.tools;
using NovelTech.viewmodels.tools;
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
using NovelTech.viewmodels;
using NovelTech.views.usercontrols;
using IronXL;

namespace NovelTech.views.windows.tools
{
    /// <summary>
    /// Interaction logic for W_edit_tool_in_action.xaml
    /// </summary>
    public partial class W_edit_tool_in_action : Window
    {
        int toolIndex = -1;

        string[,] prefixes = new string[20, 12];
        public W_edit_tool_in_action(EditToolEquippedViewModel dataContext)
        {
            WorkBook workbook = WorkBook.Load("D:\\programing\\NovelTech\\git\\NovelTech\\NovelTech\\views\\windows\\tools\\ToolData.xlsx");
            WorkSheet sheet = workbook.DefaultWorkSheet;
            Cell[] sheetArray = sheet.ToArray();
            for (int i = 0; i < 20; i++)
            {
                for (int j =0; j < 12; j++)
                {
                    prefixes[i, j] = sheetArray[12*i+j].StringValue;
                }
            }

            switch (dataContext.tool.position)
            {
                case 25:
                    toolIndex = 0;
                    break;
                case 225:
                    toolIndex = 1;
                    break;
                case 425:
                    toolIndex = 2;
                    break;
                case 125:
                    toolIndex = 3;
                    break;
                case 325:
                    toolIndex = 4;
                    break;
                case 525:
                    toolIndex = 5;
                    break;
            }
            this.DataContext = dataContext;
            InitializeComponent();


            nameTX.Text = VM_machine_table.instance.tools[toolIndex].tool.name;
            manufacturerTX.Text = VM_machine_table.instance.tools[toolIndex].tool.manufacturer;
            lengthTX.Text = VM_machine_table.instance.tools[toolIndex].tool.length.ToString();
            thicknessTX.Text = VM_machine_table.instance.tools[toolIndex].tool.thickness.ToString();
            tpiTX.Text = VM_machine_table.instance.tools[toolIndex].tool.tpi.ToString();
            rpmTX.Text = VM_machine_table.instance.tools[toolIndex].tool.rpm.ToString();
            feedrateTX.Text = VM_machine_table.instance.tools[toolIndex].tool.feed_rate.ToString();
            plungeRateTX.Text = VM_machine_table.instance.tools[toolIndex].tool.plunge_rate.ToString();
            workMaterialTX.Text = VM_machine_table.instance.tools[toolIndex].tool.work_material.ToString();
            innerDiameterTX.Text = VM_machine_table.instance.tools[toolIndex].tool.inner_diameter.ToString();
            outerDiameterTX.Text = VM_machine_table.instance.tools[toolIndex].tool.outer_diameter.ToString();
            cuttingLengthTX.Text = VM_machine_table.instance.tools[toolIndex].tool.cutting_length.ToString();

            //LoadUI();


            for (int i = 0; i < 15; i++)
            {
                prefix_CB.Items.Add(prefixes[i, 0]);
            }
        }


        private void prefix_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = prefix_CB.SelectedIndex;
            nameTX.Text = prefixes[index, 0];
            manufacturerTX.Text = prefixes[index, 1];
            lengthTX.Text = prefixes[index, 2];
            thicknessTX.Text = prefixes[index, 3];
            tpiTX.Text = prefixes[index, 4];
            rpmTX.Text = prefixes[index, 5];
            feedrateTX.Text = prefixes[index, 6];
            plungeRateTX.Text = prefixes[index, 7];
            workMaterialTX.Text = prefixes[index, 8];
            innerDiameterTX.Text = prefixes[index, 9];
            outerDiameterTX.Text = prefixes[index, 10];
            cuttingLengthTX.Text = prefixes[index, 11];
        }

        //public W_edit_tool_in_action(Tool tool)
        //{
        //    InitializeComponent();
        //    this.tool = new Tool_in_action("", 100, Tool_in_action.Orientaions.Head, tool);
        //    mode = Mode.Create;
        //    LoadUI();
        //}

        /// <summary>
        /// happens when confirm is clicked updates the tool info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_confirm_click(object sender, RoutedEventArgs e)
        {
            VM_machine_table.instance.tools[toolIndex].tool.name = nameTX.Text;
            VM_machine_table.instance.tools[toolIndex].tool.manufacturer = manufacturerTX.Text;
            VM_machine_table.instance.tools[toolIndex].tool.length = float.Parse(lengthTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.thickness = float.Parse(thicknessTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.tpi = float.Parse(tpiTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.rpm = float.Parse(rpmTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.feed_rate = float.Parse(feedrateTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.plunge_rate = float.Parse(plungeRateTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.work_material = float.Parse(workMaterialTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.inner_diameter = float.Parse(innerDiameterTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.outer_diameter = float.Parse(outerDiameterTX.Text);
            VM_machine_table.instance.tools[toolIndex].tool.cutting_length = float.Parse(cuttingLengthTX.Text);
            UC_machine_table.instance.InfoTextUpdate(toolIndex);
        }


        //private void b_cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}

        //private void LoadUI()
        //{
        //    //tb_name.Text = tool.name;
        //    //switch (tool.orientation)
        //    //{
        //    //    case Tool_in_action.Orientaions.Head:
        //    //        rb_head.IsChecked = true;
        //    //        break;
        //    //    case Tool_in_action.Orientaions.Left:
        //    //        rb_left.IsChecked = true;
        //    //        break;
        //    //    case Tool_in_action.Orientaions.Right:
        //    //        rb_right.IsChecked = true;
        //    //        break;
        //    //}
        //    //tb_position.Text = tool.position.ToString();
        //}



        //private void b_remove_Click(object sender, RoutedEventArgs e)
        //{
        //    viewmodels.VM_tools.toolBox.equipped.Remove(tool);
        //    viewmodels.VM_machine_table.instance.locate_tools();
        //    viewmodels.VM_tools.SaveToolBox();
        //    this.Close();
        //}
    }

}