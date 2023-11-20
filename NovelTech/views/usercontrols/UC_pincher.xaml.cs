using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Xml;
using NovelTech.viewmodels;

namespace NovelTech.views.usercontrols
{
    /// <summary>
    /// Interaction logic for UC_pincher.xaml
    /// </summary>
    public partial class UC_pincher : UserControl
    {
        public static UC_pincher instance;
        public UC_pincher()
        {
             instance = this;
            InitializeComponent();
        }
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }

        /// <summary>
        /// calculate arms positioning on mouse move to reset arm at the start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pincher_Loaded(object sender, RoutedEventArgs e)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            string propertiesPath = Path.Combine(projectDirectory, "properties.xml");

            //get the size value from the properties file
            double pincherSize = ReadXMLDoubleValue(propertiesPath, "PincherSize");

            //Set pincher size
            e_pincher.Width = pincherSize;
            e_pincher.Height = e_pincher.Width;

            UC_machine_table.instance.ChangeAngles();
        }


        /// <summary>
        /// used to read a value of the type double from xml file 
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private double ReadXMLDoubleValue(string file, string nodeName)
        {
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.DocumentElement.SelectSingleNode(nodeName);
            string text = node.InnerText;

            if (double.TryParse(text, out var value)) return value * VM_main.instance.dimensionRatio;
            else
            {
                throw new Exception("tried to read a non double value, check your XML file");
            }
        }

    }
}
