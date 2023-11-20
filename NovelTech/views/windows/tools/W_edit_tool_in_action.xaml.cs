using NovelTech.viewmodels.tools;
using System;
using System.Windows;
using System.Windows.Controls;
using NovelTech.viewmodels;
using NovelTech.views.usercontrols;
using IronXL;
using System.IO;
using System.Xml;

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
            //get project location 
            string novelTechDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            //get the file containing tool info
            string toolDataPath = Path.Combine(novelTechDirectory, "views", "windows", "tools", "ToolData.xlsx");


            WorkBook workbook = WorkBook.Load(toolDataPath);
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

        /// <summary>
        /// happens when confirm is clicked updates the tool info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_confirm_click(object sender, RoutedEventArgs e)
        {

            string novelTechProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            string filePath = Path.Combine(novelTechProjectDirectory, "ToolProperties.xml");
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);


            SetXMLValue(filePath, "Name", nameTX.Text);
            SetXMLValue(filePath, "Manufacturer", manufacturerTX.Text);
            SetXMLValue(filePath, "Length", lengthTX.Text);
            SetXMLValue(filePath, "Thickness", thicknessTX.Text);
            SetXMLValue(filePath, "Tpi", tpiTX.Text);
            SetXMLValue(filePath, "Rpm", rpmTX.Text);
            SetXMLValue(filePath, "FeedRate", feedrateTX.Text);
            SetXMLValue(filePath, "PlungeRate", plungeRateTX.Text);
            SetXMLValue(filePath, "WorkMaterial", workMaterialTX.Text);
            SetXMLValue(filePath, "InnerDiameter", innerDiameterTX.Text);
            SetXMLValue(filePath, "OuterDiameter", outerDiameterTX.Text);
            SetXMLValue(filePath, "CuttingLength", cuttingLengthTX.Text);




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


        /// <summary>
        /// used to read a value of the type double from xml file 
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private void SetXMLValue(string file, string nodeName, string value)
        {
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            string toolNode = "";
            switch (toolIndex)
            {
                case 0:
                     toolNode = "ToolOne";
                    break;
                case 1:
                    toolNode = "ToolTwo";
                    break;

            }

            XmlNode node = doc.DocumentElement.SelectSingleNode(toolNode).SelectSingleNode(nodeName);
            node.InnerText = value;

            doc.Save(file);
        }


    }

}