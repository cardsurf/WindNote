using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using WindNote.Gui.AttachedProperties;
using WindNote.Gui.CustomControls;
using WindNote.Gui.Utilities;
using WindNote.Mvvm;
using WindNote.MvvmBase;

namespace WindNote.Gui.CustomControls
{
    public class ListViewDragAndDrop : ListView
    {
        private bool FirstTimeItemsAdded;
        public IBindableObservableCollection ObservableItems
        {
            get { return (IBindableObservableCollection)GetValue(ObservableItemsProperty); }
            set { SetValue(ObservableItemsProperty, value); }
        }
        public static readonly DependencyProperty ObservableItemsProperty =
            DependencyProperty.Register("ObservableItems", typeof(IBindableObservableCollection), typeof(ListViewDragAndDrop));

        public ListViewDragAndDrop()
        {
            this.FirstTimeItemsAdded = false;
            this.AllowDrop = true;
            this.SelectionChanged += ScrollToSelectedItem;
            this.AddDropHandler();
            this.AddSizeChangedHandler();
            this.AddLoadedHandler();
            this.AddPreviewMouseWheelHandler();
        }

        private void AddDropHandler()
        {
            ListViewDragAndDropDropHandler handler = new ListViewDragAndDropDropHandler(this);
            this.Drop += handler.SwapListViewItems;
        }

        private void AddSizeChangedHandler()
        {
            ListViewDragAndDropSizeChangedHandler handler = new ListViewDragAndDropSizeChangedHandler();
            this.SizeChanged += handler.ResizeColumnsFillWidth;
        }

        private void AddLoadedHandler()
        {
            ListViewDragAndDropSizeChangedHandler handler = new ListViewDragAndDropSizeChangedHandler();
            this.Loaded += handler.ResizeColumnsFillWidth;
        }

        private void AddPreviewMouseWheelHandler()
        {
            ListViewDragAndDropPreviewMouseWheelHandler handler = new ListViewDragAndDropPreviewMouseWheelHandler();
            this.PreviewMouseWheel += handler.SendScrollEventToParent;
        }

        private void ScrollToSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            this.ScrollIntoView(this.SelectedItem);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (this.Items.Count > 0 && !this.FirstTimeItemsAdded)
            {
                this.FirstTimeItemsAdded = true;
                this.ResizeAfterItemsRendered();
            }
        }

        public void ResizeAfterItemsRendered()
        {
            this.Dispatcher.BeginInvoke(new Action(this.Resize), DispatcherPriority.ContextIdle);
        }

        public void Resize()
        {
            ListViewDragAndDropSizeChangedHandler handler = new ListViewDragAndDropSizeChangedHandler();
            handler.ResizeColumnsFillWidth(this, null);
        }

    }

}
