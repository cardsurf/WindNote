using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WindNote.Gui.AttachedProperties
{

    public class GridViewColumnProperties
    {
        public static readonly DependencyProperty FillWidthProperty =
            DependencyProperty.RegisterAttached("RegisteredNameFillWidthProperty",
            typeof(bool), typeof(GridViewColumnProperties), new FrameworkPropertyMetadata(null));

        public static bool GetFillWidth(DependencyObject element)
        {
            return (bool) element.GetValue(FillWidthProperty);
        }

        public static void SetFillWidth(DependencyObject element, bool value)
        {
            element.SetValue(FillWidthProperty, value);
        }

    }

}
