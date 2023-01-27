using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NovelTech.libraries
{
    public static class Treeview
    {
        public static T GetParent<T>(RoutedEventArgs child)
        {
            TreeViewItem item = child.OriginalSource as TreeViewItem;
            if (item != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(item);
                while (!(parent is TreeViewItem || parent is TreeView))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }


                return (T)(parent as TreeViewItem).DataContext;
            }
            return default(T);
        }
    }
}
