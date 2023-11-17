using ManipulatedBBox;
using ManipulatedBBox.Thumbs;
using NovelTech.viewmodels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


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

            VM_machine_table.instance.tools.Add(new models.tools.Tool_in_action(new models.tools.Tool("2", "2", 10, 10, 10, 99, 10, 10, 10,25)));
            VM_tools.toolBox.equipped[0] = (VM_machine_table.instance.tools[0]);

            VM_machine_table.instance.tools.Add(new models.tools.Tool_in_action(new models.tools.Tool("3", "3", 3, 3, 3, 3, 3, 3, 3,225)));
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DesignerItemDecorator.instance.resize == null)
                return;
            ChangeValue(this, e);
            ChangeHandPosition();
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
        #region hand transform

        //used to test the hand rotation 
        //disabled
        private void RepeatButton_ClickLeft(object sender, RoutedEventArgs e)
        {
            //firsthandimagerender.Angle += 10;
            //ChangeHandPosition();
        }
        //used to test the hand rotation 
        //disabled
        private void RepeatButton_ClickRight(object sender, RoutedEventArgs e)
        {
            //firsthandimagerender.Angle -= 10;
            //ChangeHandPosition();
        }
        /// <summary>
        /// change second hand position reletive to the first hand
        /// </summary>
        public void ChangeHandPosition()
        {
            double secondHandX = staticStand.Width / 2 + Canvas.GetLeft(staticStand) + firsthand.Width * Math.Cos(toRadians(firsthandimagerender.Angle));
            double secondHandY = staticStand.Height / 2 - firsthand.Height / 2 + Canvas.GetTop(staticStand) + firsthand.Width * Math.Sin(toRadians(firsthandimagerender.Angle));
            Canvas.SetLeft(secondhand, secondHandX);
            Canvas.SetTop(secondhand, secondHandY);
        }


        //public double FH_angle, SH_angle, EP_angle;
        /// <summary>
        /// update the hands angles and positions based on pincher x,y 0,0 is the pincher's starting position
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

            //claculates the pincher x,y relative to the statichand
            x = absolutePincherPostion.X - absoluteStaticStandPosition.X;
            y = absolutePincherPostion.Y - absoluteStaticStandPosition.Y;

            Vector staticStandVisualOffset = VisualTreeHelper.GetOffset(instance.staticStand);
            x -= staticStandVisualOffset.X;
            y -= staticStandVisualOffset.Y;

            //offset the x,y to make it be in the center of the pincher
            x -= UC_pincher.instance.e_pincher.Width/ 4;
            y -= UC_pincher.instance.e_pincher.Height/4;

            //Inverse Kinematics for a 2-Joint Robot Arm Using Geometry https://robotacademy.net.au/lesson/inverse-kinematics-for-a-2-joint-robot-arm-using-geometry/
            double second = ((x * x) + (y * y) - (firsthand.Width * firsthand.Width) - (secondhand.Width * secondhand.Width)) / (2 * firsthand.Width * secondhand.Width);
            //first we get the angle of the second hand
            secondhandimagerender.Angle = toDegrees(Math.Acos(second));
            //than we get the angle for the first hand
            firsthandimagerender.Angle = toDegrees(Math.Atan(y / x)) - toDegrees(Math.Atan(secondhand.Width * Math.Sin(toRadians(secondhandimagerender.Angle)) / (firsthand.Width + secondhand.Width * Math.Cos(toRadians(secondhandimagerender.Angle)))));
            //lastly we add the first hand angle to the second hand
            secondhandimagerender.Angle += firsthandimagerender.Angle;
            PincherMotorAngle.Text = "pincher motor should add " + Math.Round(secondhandimagerender.Angle, 3) + " dgrees";
            //and update the second hand position based on the new angles
            ChangeHandPosition();
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


    }
}
//toDegrees(Math.Atan(secondhand.Width * Math.Sin(toRadians(secondhandimagerender.Angle)) / (firsthand.Width + secondhand.Width * Math.Cos(toRadians(secondhandimagerender.Angle)))))