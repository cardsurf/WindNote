using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WindNote.Gui.AttachedProperties;

namespace WindNote.Gui.CustomControls
{
    public class ListViewDragAndDropSizeChangedHandler
    {

        public void ResizeColumnsFillWidth(object sender, RoutedEventArgs e)
        {
            this.ResizeColumnsFillWidth(sender, null);
        }

        public void ResizeColumnsFillWidth(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;
            double remainingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            List<GridViewColumn> columnsFillWidth = new List<GridViewColumn>();
            foreach (GridViewColumn column in gridView.Columns)
            {
                bool fillWidth = GridViewColumnProperties.GetFillWidth(column);
                if (fillWidth == true)
                {
                    columnsFillWidth.Add(column);
                }
                else
                {
                    remainingWidth -= column.ActualWidth;
                }
            }
            this.ResizeColumns(columnsFillWidth, remainingWidth);
        }

        private void ResizeColumns(List<GridViewColumn> columnsFillWidth, double remainingWidth)
        {
            remainingWidth = remainingWidth / columnsFillWidth.Count;
            if (remainingWidth > 0)
            {
                columnsFillWidth.ForEach(column => column.Width = remainingWidth);
            }
        }
    }
}
