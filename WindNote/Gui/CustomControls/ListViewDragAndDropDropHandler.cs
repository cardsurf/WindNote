using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindNote.Gui.CustomControls;

namespace WindNote.Gui.CustomControls
{
    public class ListViewDragAndDropDropHandler
    {
        private ListViewDragAndDrop ListView;

        public ListViewDragAndDropDropHandler(ListViewDragAndDrop ListView)
        {
            this.ListView = ListView;
        }

        public void SwapListViewItems(object sender, DragEventArgs e)
        {
            String format = (String)Application.Current.Resources["DragAndDropRowHeaderFormat"];
            if (e.Data.GetDataPresent(format))
            {
                int dragItemIndex = GetListViewSelectedItemIndex();
                int dropItemIndex = GetListViewDropItemIndex(e);
                if (dragItemIndex > -1)
                {
                    this.ListView.ObservableItems.Move(dragItemIndex, dropItemIndex);
                }
            }
        }

        private int GetListViewSelectedItemIndex()
        {
            return this.ListView.SelectedIndex;
        }

        private int GetListViewDropItemIndex(DragEventArgs e)
        {
            int row;
            for (int itemIndex = 0; itemIndex < this.ListView.ObservableItems.Count; itemIndex++)
            {
                if (IsDropItem(itemIndex, e))
                {
                    row = itemIndex;
                    return row;
                }
            }
            row = this.ListView.ObservableItems.Count - 1;
            return row;
        }

        public bool IsDropItem(int itemIndex, DragEventArgs e)
        {
            ListViewItem item = GetListViewItem(itemIndex);
            return IsMouseOverTarget(item, e);
        }

        private ListViewItem GetListViewItem(int index)
        {
            return this.ListView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        private bool IsMouseOverTarget(Visual target, DragEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePosition = e.GetPosition((IInputElement)target);
            return bounds.Contains(mousePosition);
        }

    }
}
