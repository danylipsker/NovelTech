using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace NovelTech.libraries.extensions
{
    public static class treeview
    {
        public static void ClearSelection(this TreeView tv)
        {
            var ic = tv.Items;
            var icg = tv.ItemContainerGenerator;
            if ((ic != null) && (icg != null))
                for (int i = 0; i < ic.Count; i++)
                {
                    TreeViewItem tvi = icg.ContainerFromIndex(i) as TreeViewItem;
                    if (tvi != null)
                    {
                        PreceedClearSelection(tvi.Items, tvi.ItemContainerGenerator);
                        tvi.IsSelected = false;
                    }
                }
            void PreceedClearSelection(ItemCollection ic, ItemContainerGenerator icg)
            {
                if ((ic != null) && (icg != null))
                    for (int i = 0; i < ic.Count; i++)
                    {
                        TreeViewItem tvi = icg.ContainerFromIndex(i) as TreeViewItem;
                        if (tvi != null)
                        {
                            PreceedClearSelection(tvi.Items, tvi.ItemContainerGenerator);
                            tvi.IsSelected = false;
                        }
                    }
            }
        }
    }
}
