using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WindNote.Gui.Converters
{
    public class YesNoToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String booleanText = value.ToString().ToLower();
            switch (booleanText)
            {
                case "yes":
                    return true;
                case "no":
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                bool booleanValue = (bool)value;
                if (booleanValue == true)
                    return "yes";
                else
                    return "no";
            }
            return "no";
        }
    }
}
