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
    /// Interaction logic for UC_material.xaml
    /// </summary>
    public partial class UC_material : UserControl
    {
        public static UC_material instance;
        public UC_material()
        {
            DataContext =  VM_material.instance;
            instance = this;
            //this.Margin = libraries.margins.merge_centers(pincher, this);
            InitializeComponent();

        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VM_material.instance.elpMargin = elpMargin;
            VM_material.instance.uiMaterial = this;
        }
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            UC_machine_table.instance.ChangeAngles();
            return ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }
    }
}
