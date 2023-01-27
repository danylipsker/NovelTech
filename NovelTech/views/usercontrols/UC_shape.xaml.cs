using ManipulatedBBox;
using NovelTech.viewmodels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UC_shape.xaml
    /// </summary>
    public partial class UC_shape : UserControl
    {
        public static UC_shape instance;
        public UC_shape()
        {
            InitializeComponent();
            DataContext = VM_shape.instance;
            Panel.SetZIndex(this, 4);
            instance = this;
            //NovelTech.libraries.points.get_absolute(test.Items[0], Application.Current.MainWindow);
            //material.g_material.Children.Add(box);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VM_shape.instance.polygon = polygon;
            VM_shape.instance.uiShape = this;
        }

        public void UC_SHAPE_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (test == null) ;
            //Ellipse ellipse = new Ellipse() { Width = 5, Margin = new Thickness(10,10,0,0) };
            //UC_SHAPE.Content = ellipse;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }
    }
}
