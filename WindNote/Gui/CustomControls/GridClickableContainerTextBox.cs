using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WindNote.Gui.Utilities;

namespace WindNote.Gui.CustomControls
{
    class GridClickableContainerTextBox : Grid
    {
        public String TextBoxName
        {
            get { return (String)GetValue(TextBoxNameProperty); }
            set { SetValue(TextBoxNameProperty, value); }
        }
        public static readonly DependencyProperty TextBoxNameProperty =
        DependencyProperty.Register("TextBoxName", typeof(String), typeof(GridClickableContainerTextBox));

        public TextBox TextBox;

        public GridClickableContainerTextBox()
        {
            this.Init();
            this.Loaded += this.InitTextBox;
            this.MouseDown += this.FocusTextBoxMouseDown;
        }

        private void Init()
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.Cursor = Cursors.IBeam;
        }

        private void InitTextBox(object sender, RoutedEventArgs e)
        {
            this.TextBox = (TextBox) VisualTreeTraverser.FindFirstDescendantWithName(this, this.TextBoxName);
        }

        private void FocusTextBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                TextBox.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.FocusTextBox));
            }
            else
            {
                TextBox.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.SelectAllTextBox));
            }
        }

        private void FocusTextBox()
        {
            this.TextBox.Focus();
            this.TextBox.SelectionStart = TextBox.Text.Length;
        }

        private void SelectAllTextBox()
        {
            this.FocusTextBox();
            this.TextBox.SelectAll();
        }

    }
}
