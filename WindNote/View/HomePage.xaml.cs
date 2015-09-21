using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using WindNote.Events;
using WindNote.Gui.AttachedProperties;
using WindNote.Gui.UserControls;
using WindNote.Gui.Utilities;
using WindNote.Model;

namespace WindNote.View
{

    public partial class HomePage : Page, IView
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }

}
