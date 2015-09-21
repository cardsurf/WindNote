using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WindNote
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.LoadPropertiesFromUserSettings();
            this.Closing += this.SavePropertiesToUserSettings;
        }

        private void LoadPropertiesFromUserSettings()
        {
            this.Height = Properties.Settings.Default.AppHeight;
            this.Width = Properties.Settings.Default.AppWidth;
            if(Properties.Settings.Default.Maximized == true)
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        void SavePropertiesToUserSettings(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.AppHeight = this.ActualHeight;
            Properties.Settings.Default.AppWidth = this.ActualWidth;
            Properties.Settings.Default.Maximized = this.WindowState == WindowState.Maximized ? true : false;
            Properties.Settings.Default.Save();
        }
    }
}
