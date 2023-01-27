using NovelTech.viewmodels.PolygonCreator;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovelTech.views.windows.PolygonCreator
{
    /// <summary>
    /// Interaction logic for PolygonCreatorEdgeView.xaml
    /// </summary>
    public partial class EdgeView : UserControl
    {
        public EdgeView()
        {
            InitializeComponent();
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                (DataContext as EdgeViewModel).AddEdge(e.GetPosition(this));

            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverLine.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            HoverLine.Visibility = Visibility.Hidden;
        }
    }
}
