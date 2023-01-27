using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace NovelTech
{
    public static class Extensions
    {
        #region Thickness
        public static void MoveStep(this Thickness margin, string direction, double step = 1)
        {
            switch (direction)
            {
                case "right":
                    margin.Left += step;
                    break;
                case "left":
                    margin.Left -= step;
                    break;
                case "up":
                    margin.Bottom += step;
                    break;
                case "down":
                    margin.Bottom -= step;
                    break;

            }
        }
        #endregion

        #region Collection
        public static int Filter<T>(this System.Collections.ObjectModel.Collection<T> collection, Predicate<object> predicate)
        {
            System.Windows.Data.CollectionView itemsViewOriginal = (System.Windows.Data.CollectionView)
            System.Windows.Data.CollectionViewSource.GetDefaultView(collection);

            itemsViewOriginal.Filter = predicate;
            itemsViewOriginal.Refresh();
            return itemsViewOriginal.Count;
        }


        #endregion

        #region Dependency Object
        public static childItem FindVisualChild<childItem>(this DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion
    }
}
