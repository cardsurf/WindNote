using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindNote.Gui.UserControls
{
    public partial class TransparentTitleBarWindow : UserControl
    {
        private Window Window;

        public TransparentTitleBarWindow()
        {
            InitializeComponent();
            this.Loaded += InitWindow;
        }

        private void InitWindow(object sender, RoutedEventArgs e)
        {
            this.Window = Window.GetWindow(this);
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    this.AdjustWindowSize();
                }
                else
                {
                    this.Window.DragMove();
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.AdjustWindowSize();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Window.WindowState = WindowState.Minimized;
        }

        private void AdjustWindowSize()
        {
            if (this.Window.WindowState == WindowState.Maximized)
            {
                this.Window.WindowState = WindowState.Normal;
            }
            else
            {
                this.Window.WindowState = WindowState.Maximized;
            }
        }
    }

}
