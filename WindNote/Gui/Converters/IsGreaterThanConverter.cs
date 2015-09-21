using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WindNote.Gui.Converters
{
    public class IsGreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double valueNumber = System.Convert.ToDouble(value);
            double parameterNumber = System.Convert.ToDouble(parameter);
            return valueNumber > parameterNumber ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
