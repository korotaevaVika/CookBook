using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CookBook_WPF.Converters
{
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value is bool)
                    if ((bool)value)
                    {
                        return Visibility.Collapsed;
                    }
                    else { return Visibility.Visible; }
                return null;
            }
            catch (Exception)
            {
                return null ;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Add the convert back if needed
            //return value is bool ? (bool)value : false;
            throw new NotImplementedException();
        }
    }
}
