using ManipulatedBBox;
using NovelTech.viewmodels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml;


namespace NovelTech.views.usercontrols
{
    /// <summary>
    /// Interaction logic for UC_machine_table.xaml
    /// </summary>
    public partial class UC_machine_table : UserControl
    {
        public static UC_machine_table instance;
        float RPMPercentValue = 100,feedratePercentValue=100;
        public int lToolIndex=0;
        public UC_tool[] uC_Tools = new UC_tool[6];

        public string checkedToolPosition = "Inside";
        public UC_machine_table()
        {
            InitializeComponent();
            instance = this;
            HeightValue.Visibility = Visibility.Collapsed;
            WidthValue.Visibility = Visibility.Collapsed;
            ratio.Visibility = Visibility.Collapsed;
            FeedratePercent.Visibility = Visibility.Collapsed;
            Feedrate.Visibility = Visibility.Collapsed;
            RPM.Visibility = Visibility.Collapsed;
            RPMpercent.Visibility = Visibility.Collapsed;


            string novelTechProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            string filePath = Path.Combine(novelTechProjectDirectory, "ToolProperties.xml");
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);


            


            VM_machine_table.instance.tools.Add(new models.tools.Tool_in_action(new models.tools.Tool(ReadXMLValue(filePath, "ToolOne", "Name"),
            ReadXMLValue(filePath, "ToolOne", "Manufacturer"),
            ReadXMLFloatValue(filePath, "ToolOne", "Length"),
            ReadXMLFloatValue(filePath, "ToolOne", "Thickness"),
            ReadXMLFloatValue(filePath, "ToolOne", "Tpi"),
            ReadXMLFloatValue(filePath, "ToolOne", "Rpm"),
            ReadXMLFloatValue(filePath, "ToolOne", "FeedRate"),
            ReadXMLFloatValue(filePath, "ToolOne", "PlungeRate"), 10,25)));
            VM_tools.toolBox.equipped[0] = (VM_machine_table.instance.tools[0]);


            VM_machine_table.instance.tools.Add(new models.tools.Tool_in_action(new models.tools.Tool(ReadXMLValue(filePath, "ToolTwo", "Name"),
            ReadXMLValue(filePath, "ToolTwo", "Manufacturer"),
            ReadXMLFloatValue(filePath, "ToolTwo", "Length"),
            ReadXMLFloatValue(filePath, "ToolTwo", "Thickness"),
            ReadXMLFloatValue(filePath, "ToolTwo", "Tpi"),
            ReadXMLFloatValue(filePath, "ToolTwo", "Rpm"),
            ReadXMLFloatValue(filePath, "ToolTwo", "FeedRate"),
            ReadXMLFloatValue(filePath, "ToolTwo", "PlungeRate"), 3,225)));
            VM_tools.toolBox.equipped[1] = (VM_machine_table.instance.tools[1]);

            uC_Tools[0] = Saw;
            uC_Tools[1] = Drill;
            uC_Tools[0].position = (float)uC_Tools[0].Margin.Left;
            uC_Tools[1].position = (float)uC_Tools[1].Margin.Left;
            uC_Tools[0].bit.Visibility = Visibility.Hidden;
            uC_Tools[1].blade.Visibility = Visibility.Hidden;

            InfoTextUpdate(1);
            InfoTextUpdate(0);
            UpdateRPMFeedrateValues();
        }


        /// <summary>
        /// used to read a value from xml file 
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string ReadXMLValue(string file,string outerNodeName, string nodeName)
        {
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.DocumentElement.SelectSingleNode(outerNodeName).SelectSingleNode(nodeName);
            string text = node.InnerText;
            return text;
        }


        /// <summary>
        /// used to read a value of the type double from xml file 
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private float ReadXMLFloatValue(string file, string outerNodeName, string nodeName)
        {
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.DocumentElement.SelectSingleNode(outerNodeName).SelectSingleNode(nodeName);
            string text = node.InnerText;

            if (float.TryParse(text, out var value)) {
                return value;
            }
            else
            {
                throw new Exception("tried to read a non float value, check your XML file");
            }
        }


        /// <summary>
        /// when called updates the info text of the tool in toolIndex position in VM_machine_table.instance.tools
        /// </summary>
        /// <param name="toolIndex">index of tool you want to update</param>
        public void InfoTextUpdate(int toolIndex)
        {
            lToolIndex = toolIndex;
            string info = "name: " + VM_machine_table.instance.tools[toolIndex].tool.name + " manufacturer: " +
                VM_machine_table.instance.tools[toolIndex].tool.manufacturer + " length: " + VM_machine_table.instance.tools[toolIndex].tool.length
                + " thickness: " + VM_machine_table.instance.tools[toolIndex].tool.thickness + " tpi: " + VM_machine_table.instance.tools[toolIndex].tool.tpi
                + " rpm: " + VM_machine_table.instance.tools[toolIndex].tool.rpm + " feedrate: " + VM_machine_table.instance.tools[toolIndex].tool.feed_rate 
                + " plunge rate: " + VM_machine_table.instance.tools[toolIndex].tool.plunge_rate
                + " work material: "+VM_machine_table.instance.tools[toolIndex].tool.work_material
                + " inner diameter: "+ VM_machine_table.instance.tools[toolIndex].tool.inner_diameter
                + " outer diameter: " + VM_machine_table.instance.tools[toolIndex].tool.outer_diameter
                + " cutting length: " + VM_machine_table.instance.tools[toolIndex].tool.cutting_length;

            foreach(UC_tool tool in uC_Tools)
            {
                if (tool == null)
                    continue;
                tool.ColorElement.Background = Brushes.Red;
            }
            uC_Tools[toolIndex].ColorElement.Background = Brushes.Green;

            switch (toolIndex)
            {
                case 0:
                    Saw.blade.Width = VM_machine_table.instance.tools[toolIndex].tool.thickness;
                    SawInfo.Text = info;
                    break;
                case 1:
                    Drill.bit.Width = VM_machine_table.instance.tools[toolIndex].tool.thickness;
                    Drill.bit.Height = Drill.bit.Width;
                    DrillInfo.Text = info;
                    break;
            }

            UpdateRPMFeedrateValues();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            (DataContext as VM_machine_table).uiMachineTable = instance;
            (DataContext as VM_machine_table).uiTools = noNeedUiTools;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }

        #region Shape size
        public void ChangeValue(object sender, EventArgs e)
        {
            SetVisibiltyHeightWidth();
            HeightSB.Value = DesignerItemDecorator.instance.resize.shape.Height;
            HeightValue.Text = Math.Round(HeightSB.Value).ToString();
            WidthSB.Value = DesignerItemDecorator.instance.resize.shape.Width;
            WidthValue.Text = Math.Round(WidthSB.Value).ToString();
            CheckRatio();
        }
        private void ChangeWidth(object sender, ScrollEventArgs e)
        {
            if (DesignerItemDecorator.instance.resize == null)
                return;
            SetVisibiltyHeightWidth();
            if (e.ScrollEventType.ToString() == "SmallDecrement") DesignerItemDecorator.instance.resize.shape.Width += 1;
            else
                DesignerItemDecorator.instance.resize.shape.Width -= 1;

            ChangeValue(this, e);
        }
        private void ChangeHeight(object sender, ScrollEventArgs e)
        {
            if (DesignerItemDecorator.instance.resize == null)
                return;
            SetVisibiltyHeightWidth();
            if (e.ScrollEventType.ToString() == "SmallDecrement") DesignerItemDecorator.instance.resize.shape.Height += 1;
            else
                DesignerItemDecorator.instance.resize.shape.Height -= 1;

            ChangeValue(this, e);
        }
        void CheckRatio()
        {
            ratio.Text = Math.Round(DesignerItemDecorator.instance.resize.shape.Width / DesignerItemDecorator.instance.originalX, 3) + "/" + Math.Round(DesignerItemDecorator.instance.resize.shape.Height / DesignerItemDecorator.instance.originalY, 3);
        }
        void SetVisibiltyHeightWidth()
        {
            if (HeightValue.Visibility == Visibility.Collapsed)
            {
                HeightValue.Visibility = Visibility.Visible;
                WidthValue.Visibility = Visibility.Visible;
                ratio.Visibility = Visibility.Visible;
            }
        }
        public void SetStartValuesHeightWidth(double x, double y)
        {
            SetVisibiltyHeightWidth();
            HeightSB.Value = y;
            HeightValue.Text = Math.Round(HeightSB.Value).ToString();
            WidthSB.Value = x;
            WidthValue.Text = Math.Round(WidthSB.Value).ToString();
            ratio.Text = "1/1";
        } 
        #endregion
        #region show distance between each point in shape
        public void AddPointsToList(PointCollection shapePoints)
        {
            points.Items.Clear();
            for (int i = 1; i < shapePoints.Count; i++)
            {
                points.Items.Add(i + "-" + (i + 1) + ": " + DistanceBetweenPoints(shapePoints[i - 1], shapePoints[i]));
            }
            points.Items.Add(shapePoints.Count + "-" + 1 + ": " + DistanceBetweenPoints(shapePoints[shapePoints.Count - 1], shapePoints[0]));
        }
        double DistanceBetweenPoints(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        } 
        #endregion
        #region feedrate and rpm
        void ChangeRPM(object sender, ScrollEventArgs e)
        {
            if (RPM.Visibility == Visibility.Collapsed)
                return;
            if (e.ScrollEventType.ToString() == "SmallDecrement")
                RPMPercentValue += 10;
            else
                RPMPercentValue -= 10;
            RPMpercent.Text = RPMPercentValue + "%";
            RPM.Text = MathF.Round(VM_tools.toolBox.equipped[lToolIndex].tool.rpm * (RPMPercentValue / 100),3) + "";
        }
        void ChangeFeedrate(object sender, ScrollEventArgs e)
        {
            if (RPM.Visibility == Visibility.Collapsed)
                return;
            if (e.ScrollEventType.ToString() == "SmallDecrement")
                feedratePercentValue += 10;
            else
                feedratePercentValue -= 10;
            FeedratePercent.Text = feedratePercentValue + "%";
            Feedrate.Text = MathF.Round(VM_tools.toolBox.equipped[lToolIndex].tool.feed_rate * (feedratePercentValue / 100),3) + "";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            checkedToolPosition = temp.Content.ToString();
        }

        //for initial values beefore the scrollbar is pressed
        public void UpdateRPMFeedrateValues()
        {
            RPMpercent.Text = RPMPercentValue + "%";
            RPM.Text = MathF.Round(viewmodels.VM_tools.toolBox.equipped[lToolIndex].tool.rpm * (RPMPercentValue / 100),3) + "";

            FeedratePercent.Text = feedratePercentValue + "%";
            Feedrate.Text = MathF.Round(viewmodels.VM_tools.toolBox.equipped[lToolIndex].tool.feed_rate * (feedratePercentValue / 100),3) + "";

            FeedratePercent.Visibility = Visibility.Visible;
            Feedrate.Visibility = Visibility.Visible;
            RPM.Visibility = Visibility.Visible;
            RPMpercent.Visibility = Visibility.Visible;
        }
        #endregion
        #region arm transform
        /// <summary>
        /// used to test the arm rotation 
        /// disabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeatButton_ClickLeft(object sender, RoutedEventArgs e)
        {
            //armOneimagerender.Angle += 10;
            //ChangeArmPosition();
        }

        /// <summary>
        /// used to test the arm rotation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeatButton_ClickRight(object sender, RoutedEventArgs e)
        {
            //armOneimagerender.Angle -= 10;
            //ChangeArmPosition();
        }

        /// <summary>
        /// change second arm position reletive to the first arm
        /// </summary>
        public void ChangeArmPosition()
        {
            double armOneX = armOne.Width * Math.Cos(toRadians(armOneimagerender.Angle));
            double armTwoX = staticStand.Width / 2 + Canvas.GetLeft(staticStand) + armOneX;

            double armOneY = +armOne.Width * Math.Sin(toRadians(armOneimagerender.Angle));
            double armTwoY = staticStand.Height / 2 - armOne.Height / 2 + Canvas.GetTop(staticStand) + armOneY;


            Canvas.SetLeft(armTwo, armTwoX);
            Canvas.SetTop(armTwo, armTwoY);
        }

        /// <summary>
        /// update the arms angles and positions based on pincher x,y 0,0 is the pincher's starting position
        /// </summary>
        /// <param name="x">relative pincher x</param>
        /// <param name="y">relative pincher x</param>
        public void ChangeAngles()
        {
            double x=0, y=0;
            //get the position of pincher relative to screen
            Point absolutePincherPostion = UC_pincher.instance.PointToScreen(new Point(UC_pincher.instance.e_pincher.Margin.Left, UC_pincher.instance.e_pincher.Margin.Top));

            //get position of staticStand relative to screen
            Point absoluteStaticStandPosition = instance.PointToScreen(new Point(instance.staticStand.Margin.Left, instance.staticStand.Margin.Top));

            //claculates the pincher x,y relative to the staticarm
            x = absolutePincherPostion.X - absoluteStaticStandPosition.X;
            y = absolutePincherPostion.Y - absoluteStaticStandPosition.Y;

            //make the positioning start from the staticstand
            Vector staticStandVisualOffset = VisualTreeHelper.GetOffset(instance.staticStand);
            x -= staticStandVisualOffset.X;
            y -= staticStandVisualOffset.Y ;

            //calculate in the value of the stand size
            x -= instance.staticStand.Width / 2;
            y -= instance.staticStand.Height / 2;

            //offset the x,y to make it be in the center of the pincher
            x += UC_pincher.instance.e_pincher.Width / 2;
            y += UC_pincher.instance.e_pincher.Height / 2;


            //Inverse Kinematics for a 2-Joint Robot Arm Using Geometry https://robotacademy.net.au/lesson/inverse-kinematics-for-a-2-joint-robot-arm-using-geometry/
            double second = ((x * x) + (y * y) - (armOne.Width * armOne.Width) - (armTwo.Width * armTwo.Width)) / (2 * armOne.Width * armTwo.Width);
            //first we get the angle of the second arm
            armTwoimagerender.Angle = toDegrees(Math.Acos(second));

            //than we get the angle for the first arm
            armOneimagerender.Angle = toDegrees(Math.Atan(y / x)) - toDegrees(Math.Atan(armTwo.Width * Math.Sin(toRadians(armTwoimagerender.Angle)) / (armOne.Width + armTwo.Width * Math.Cos(toRadians(armTwoimagerender.Angle)))));

            //lastly we add the first arm angle to the second arm
            armTwoimagerender.Angle += armOneimagerender.Angle;
            PincherMotorAngle.Text = "pincher motor should add " + Math.Round(armTwoimagerender.Angle, 3) + " dgrees";

            //and update the second arm position based on the new angles
            ChangeArmPosition();
        }

        /// <summary>
        /// dgrees to radians
        /// </summary>
        /// <param name="angle">in dgrees</param>
        /// <returns></returns>
        double toRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        /// <summary>
        /// radians to dgrees
        /// </summary>
        /// <param name="rad">in radians</param>
        /// <returns></returns>
        double toDegrees(double rad)
        {
            return rad * (180 / Math.PI);
        }
        #endregion


        /// <summary>
        /// calculate arms positioning on mouse move to reset arm at the start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            string propertiesPath = Path.Combine(projectDirectory, "properties.xml");

            #region SetArmLength
            //get the width values from the properties file
            double armOneLength = ReadXMLDoubleValue(propertiesPath, "ArmOneLength");
            double armTwoLength = ReadXMLDoubleValue(propertiesPath, "ArmTwoLength");

            //set first arm width
            armOne.Width = armOneLength;
            armOneimagerender.CenterX = -armOne.Width / 2;

            //set second arm width
            armTwo.Width = armTwoLength;
            armTwoimagerender.CenterX = -armTwo.Width / 2; 
            #endregion

            #region set arm width
            //get arms width 
            double armOneWidth = ReadXMLDoubleValue(propertiesPath, "ArmOneWidth");
            double armTwoWidth = ReadXMLDoubleValue(propertiesPath, "ArmTwoWidth");
            //set arms width
            armOne.Height = armOneWidth;
            armTwo.Height = armTwoWidth; 
            #endregion

            //reset arm to the pincher position
            instance.ChangeAngles();



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