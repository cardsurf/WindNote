using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WindNote.Gui.UserControls
{
    public class DragableRowHeaderMouseMoveHandler
    {
        public void StartDragAndDrop(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DependencyObject dragSource = GetDragSource(e);
                DataObject dragData = GetDragDataObject(dragSource);
                DragDrop.DoDragDrop(dragSource, dragData, DragDropEffects.Move);
            }
        }

        private DependencyObject GetDragSource(MouseEventArgs e)
        {
            return (DependencyObject)e.OriginalSource;
        }

        private DataObject GetDragDataObject(DependencyObject dragSource)
        {
            String format = (String)Application.Current.Resources["DragAndDropRowHeaderFormat"];
            DataObject data = new DataObject();
            data.SetData(format, dragSource);
            return data;
        }
    }
}
