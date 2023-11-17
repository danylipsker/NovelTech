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
    }
}
