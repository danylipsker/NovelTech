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
using NovelTech.viewmodels.PolygonCreator;
using System.Collections.ObjectModel;
using MethodsLibrary;
using NovelTech.viewmodels;

namespace NovelTech.views.windows.PolygonCreator
{
    /// <summary>
    /// Interaction logic for PolygonCreatorView.xaml
    /// </summary>
    public partial class PolygonCreatorView : Window
    {
        public static PolygonCreatorView instance;
        bool drill = false;
        VertexViewModel curDrillPoint;

        public ObservableCollection<VertexViewModel> drillPoints { get; set; }
        public PolygonCreatorView()
        {
            drillPoints = new ObservableCollection<VertexViewModel>();
            instance = this;
           // drillpointscontrol.ItemsSource = drillPoints;
            InitializeComponent();
        }

        public List<double> getGetDrillPointsDepths(Point offset)
        {
            List<double> result = new List<double>();

            foreach (VertexViewModel v in drillPoints)
                result.Add(v.depth);

            return result;
        }

        public PointCollection GetDrillPointsAsPoints(Point offset)
        {
            PointCollection result = new PointCollection();
            foreach (VertexViewModel v in drillPoints)
                result.Add(new Point(v.X, v.Y));


            Point[] result1 = new Point[drillPoints.Count];
            for (int i = 0; i < drillPoints.Count; i++)
            {
                //the Ellipsis are draw from the upper left corner thus you need to decrease the position by thier width and height  
                result1[i] = new Point((drillPoints[i].X * VM_main.instance.dimensionRatio)-offset.X-10, (drillPoints[i].Y * VM_main.instance.dimensionRatio)-offset.Y-10);
            }

            return new PointCollection(result1);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            drill = true;
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            drill = false;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!drill)
                return;
            drillPoints.Add(new VertexViewModel() { X = e.GetPosition(this).X, Y = e.GetPosition(this).Y,depth = 1 });
            DrillPointsList.Items.Add(drillPoints[drillPoints.Count - 1]);
            DrillPointsList.SelectedItem = DrillPointsList.Items[DrillPointsList.Items.Count - 1];
            curDrillPoint = drillPoints[drillPoints.Count - 1];

            //drillPoints.Add(new VertexViewModel() { X = e.GetPosition(this).X, Y = e.GetPosition(this).Y });
            //PolygonCreatorViewModel.instance.Vertices.Insert(PolygonCreatorViewModel.instance.Vertices.IndexOf(oldEdge.P2), newVertex);
            //PolygonCreatorViewModel.instance.AddDrill(e.GetPosition(this));
        }

        private void DrillPointsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            curDrillPoint = e.AddedItems[0] as VertexViewModel;
            DrillPointX.Text = curDrillPoint.X.ToString();
            DrillPointY.Text = curDrillPoint.Y.ToString();
            Depth.Text = curDrillPoint.depth.ToString();

        }

        private void DrillPointY_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (!double.TryParse(DrillPointY.Text, out temp))
                return;
            foreach (VertexViewModel vertex in drillPoints)
            {
                if (vertex == curDrillPoint)
                    vertex.Y = double.Parse(DrillPointY.Text);
            }
        }

        private void DrillPointX_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (!double.TryParse(DrillPointX.Text, out temp))
                return;
            foreach (VertexViewModel vertex in drillPoints)
            {
                if (vertex == curDrillPoint)
                    vertex.X = double.Parse(DrillPointX.Text);
            }
        }
        public void UpdateText(VertexViewModel sender)
        {
            foreach (object vertex in DrillPointsList.Items)
                if (vertex == sender)
                    DrillPointsList.SelectedItem = vertex as VertexViewModel;
            curDrillPoint = sender;
            DrillPointX.Text = curDrillPoint.X.ToString();
            DrillPointY.Text = curDrillPoint.Y.ToString();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DrillPointsList.Items.Remove(curDrillPoint);
            drillPoints.Remove(curDrillPoint);
        }

        private void Depth_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (!double.TryParse(Depth.Text, out temp))
                return;
            curDrillPoint.depth = double.Parse(Depth.Text);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (curDrillPoint == null)
                return;
            switch (CurveSelection.SelectedIndex)
            {
                default:
                    //point
                    break;
                case 1:
                    ChangeToCircular(1, 1, 1);
                    break;
            }
        }
        void ChangeToCircular(float a,float b, float c)
        {
            //ArcSegment arc = new ArcSegment(new Point(100, 100), new Size(50, 25), 0, true, SweepDirection.Clockwise, true); ;

            //canvas.Children.Add(arc); 
            //Canvas.SetTop(arc, 20);
            //Canvas.SetLeft(arc, 20);



            Point[] points = {
   new Point(0, 100),
   new Point(50, 80),
   new Point(100, 20),
   new Point(150, 80),
   new Point(200, 100)};

            Pen pen = new Pen(Brushes.Black,1d);
            //e.Graphics.DrawCurve(pen, points);

        }
    }
}
