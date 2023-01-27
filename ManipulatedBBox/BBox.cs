using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

namespace ManipulatedBBox
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ManipulatedBBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ManipulatedBBox;assembly=ManipulatedBBox"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:BBox/>
    ///
    /// </summary>
    public class BBox : ContentControl
    {
        
        #region Dependency Properties
        public double SizeRatio
        {
            get { return (double)GetValue(SizeRatioProperty); }
            set { SetValue(SizeRatioProperty, value); }
        }

        public static readonly DependencyProperty SizeRatioProperty =
            DependencyProperty.Register("SizeRatio", typeof(double), typeof(BBox), new PropertyMetadata(1.0));

        #endregion
        #region Properties
        ManipulatedContentControl control;
        #endregion
        #region Constructor
        static BBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BBox), new FrameworkPropertyMetadata(typeof(BBox)));
        }
        #endregion

        #region Methods

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }
        #endregion

        #region Enums
        public enum ContainerTypes
        {
            Grid,
            Canvas
        }
        #endregion
    }
}
