using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFLibrary.customAttachedProperties
{
    public class draggableAttachedProperty : DependencyObject
    {
        public static readonly DependencyProperty IsBlockDraggingProperty =
        DependencyProperty.RegisterAttached(
          "IsBlockDragging",
          typeof(bool),
          typeof(draggableAttachedProperty),
          new PropertyMetadata(false, new PropertyChangedCallback(OnPropertyChanged)
        ));
        public static void SetIsBlockDragging(DependencyObject d, bool value)
        {
            d.SetValue(IsBlockDraggingProperty, value);
        }
        public static bool GetIsBlockDragging(DependencyObject d)
        {
            return (bool)d.GetValue(IsBlockDraggingProperty);
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "IsBlockDragging")
                d.SetValue(IsBlockDraggingProperty, e.NewValue);
        }
    }
}
