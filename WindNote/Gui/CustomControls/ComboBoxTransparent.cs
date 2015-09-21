using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WindNote.Gui.Utilities;
using WindNote.Model;
using WindNote.MvvmBase;

namespace WindNote.Gui.CustomControls
{
    class ComboBoxTransparent : ComboBox
    {

        public IBindableObservableCollection ObservableItems
        {
            get { return (IBindableObservableCollection)GetValue(ObservableItemsProperty); }
            set { SetValue(ObservableItemsProperty, value); }
        }
        public static readonly DependencyProperty ObservableItemsProperty =
            DependencyProperty.Register("ObservableItems", typeof(IBindableObservableCollection), typeof(ComboBoxTransparent));

        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }
        public static readonly DependencyProperty ButtonWidthProperty =
                                DependencyProperty.Register("ButtonWidth", typeof(int), typeof(ComboBoxTransparent));

        public String ImageButtonUri
        {
            get { return (String)GetValue(ImageButtonUriProperty); }
            set { SetValue(ImageButtonUriProperty, value); }
        }
        public static readonly DependencyProperty ImageButtonUriProperty =
                                DependencyProperty.Register("ImageButtonUri", typeof(String), typeof(ComboBoxTransparent));

        public String HintText
        {
            get { return (String)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }
        public static readonly DependencyProperty HintTextProperty =
        DependencyProperty.Register("HintText", typeof(String), typeof(ComboBoxTransparent));

        private TextBoxHint TextBoxSelectedItem;
        private bool HighlightedItemMovedDown;
        private bool HighlightedItemMovedUp;
        private int ExpectedHighlightedIndex;

        public ComboBoxTransparent()
        {
            this.SetTransparentStyle();
            this.IsEditable = true;
            this.Loaded += InitTextBoxAndButton;
        }

        private void SetTransparentStyle()
        {
            Style style = (Style)Application.Current.FindResource("ComboBoxTransparentStyle") as Style;
            this.Style = style;
        }

        private void InitTextBoxAndButton(object sender, RoutedEventArgs e)
        {
            this.InitTextBoxSelectedItem();
            this.SetButtonImage();
            this.SetButtonContainerWidthToButtonWidth();
        }

        private void InitTextBoxSelectedItem()
        {
            this.TextBoxSelectedItem = (TextBoxHint)VisualTreeTraverser.FindFirstDescendantWithName(this, "PART_EditableTextBox");
            this.TextBoxSelectedItem.HintText = this.HintText;
        }

        private void SetButtonImage()
        {
            Image image = (Image)VisualTreeTraverser.FindFirstDescendantWithName(this, "ImageButton");
            image.Source = ImageFactory.CreateBitmapImageFromUri(this.ImageButtonUri);
        }

        private void SetButtonContainerWidthToButtonWidth()
        {
            Border buttonBorder = (Border)VisualTreeTraverser.FindFirstDescendantWithName(this, "splitBorder");
            ColumnDefinition buttonColumn = (ColumnDefinition)this.Template.FindName("ButtonColumnEditableTemplate", this);
            buttonBorder.Width = this.ButtonWidth;
            buttonColumn.Width = new GridLength(this.ButtonWidth);
        }

        public void ToggleOpened()
        {
            this.IsDropDownOpen = this.IsDropDownOpen == true ? false : true;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.FocusAndSetCaretToTextBox();
        }

        private void FocusAndSetCaretToTextBox()
        {
            this.Focus();
            this.TextBoxSelectedItem.CaretIndex = this.TextBoxSelectedItem.Text.Length;
        }

        private void SetCaretEndTextBox()
        {
            this.TextBoxSelectedItem.CaretIndex = this.TextBoxSelectedItem.Text.Length;
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.FocusAndSetCaretToTextBox();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    this.MoveHighlightedItemDown();
                    return;
                }
                if (e.Key == Key.Up)
                {
                    e.Handled = true;
                    this.MoveHighlightedItemUp();
                    return;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        public void MoveHighlightedItemDown()
        {
            int index = this.GetHighlightedIndex();
            int newIndex = index + 1;
            if (index >= 0 && newIndex < this.ObservableItems.Count)
            {
                this.ObservableItems.Move(index, newIndex);
                this.ScrollToHighlightedItem(newIndex);
                this.ExpectedHighlightedIndex = newIndex;
                this.HighlightedItemMovedDown = true;
            }
        }

        public void MoveHighlightedItemUp()
        {
            int index = this.GetHighlightedIndex();
            int newIndex = index - 1;
            if (index >= 0 && newIndex >= 0)
            {
                this.ObservableItems.Move(index, newIndex);
                this.ScrollToHighlightedItem(newIndex);
                this.ExpectedHighlightedIndex = newIndex;
                this.HighlightedItemMovedUp = true;
            }
        }

        private void ScrollToHighlightedItem(int HighlightedIndex)
        {
            object item = this.Items[HighlightedIndex];
            ComboBoxItem comboBoxItem = (ComboBoxItem)this.ItemContainerGenerator.ContainerFromItem(item);
            comboBoxItem.BringIntoView();
        }

        private int GetHighlightedIndex()
        {
            for (int i = 0; i < this.Items.Count; ++i)
            {
                ComboBoxItem item = (ComboBoxItem)this.ItemContainerGenerator.ContainerFromItem(this.Items[i]);
                if (item != null && item.IsHighlighted)
                {
                    return i;
                }
            }
            return -1;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (this.HighlightedItemMovedDown)
            {
                this.HighlightedItemMovedDown = false;
                this.FixComboBoxBugIncorrectItemHighlightedWhenMouseOverItemAndItemMovedDown();
            }
            if (this.HighlightedItemMovedUp)
            {
                this.HighlightedItemMovedUp = false;
                this.FixComboBoxBugIncorrectItemHighlightedWhenMouseOverItemAndItemMovedUp();
            }
        }

        private void FixComboBoxBugIncorrectItemHighlightedWhenMouseOverItemAndItemMovedDown()
        {
            if (this.IncorrectItemHighlighted())
            {
                this.SwapHighlightedItemDown();
                this.MoveHighlightedItemDown();
                this.SetCaretEndTextBox();
            }
        }

        private void FixComboBoxBugIncorrectItemHighlightedWhenMouseOverItemAndItemMovedUp()
        {
            if (this.IncorrectItemHighlighted())
            {
                this.SwapHighlightedItemUp();
                this.MoveHighlightedItemUp();
                this.SetCaretEndTextBox();
            }
        }

        private bool IncorrectItemHighlighted()
        {
            int actualHighlightedIndex = this.GetHighlightedIndex();
            return actualHighlightedIndex != this.ExpectedHighlightedIndex;
        }

        private void SwapHighlightedItemDown()
        {
            int index = this.GetHighlightedIndex();
            int newIndex = index + 1;
            if (index >= 0 && newIndex < this.ObservableItems.Count)
            {
                this.SwapItemsAndRestoreText(index, newIndex);
            }
        }

        private void SwapHighlightedItemUp()
        {
            int index = this.GetHighlightedIndex();
            int newIndex = index - 1;
            if (index >= 0 && newIndex >= 0)
            {
                this.SwapItemsAndRestoreText(index, newIndex);
            }
        }

        private void SwapItemsAndRestoreText(int index1, int index2)
        {
            String text = this.TextBoxSelectedItem.Text;
            this.SwapItems(index1, index2);
            this.TextBoxSelectedItem.Text = text;
        }

        private void SwapItems(int index1, int index2)
        {
            object temp = this.ObservableItems[index1];
            this.ObservableItems[index1] = this.ObservableItems[index2];
            this.ObservableItems[index2] = temp;
        }
    }

}
