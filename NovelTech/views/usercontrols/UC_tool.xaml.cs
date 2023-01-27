using NovelTech.models.tools;
using NovelTech.viewmodels;
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
    /// Interaction logic for UC_tool.xaml
    /// </summary>
    public partial class UC_tool : UserControl
    {
        public static UC_tool instance;
        public float position = 0;
        public UC_tool()
        {
            InitializeComponent();
            instance = this;
        }
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }
    }
}
