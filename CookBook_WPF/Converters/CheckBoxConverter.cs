using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CookBook_WPF.Converters
{
    public class CheckBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;
                if ((bool)value)
                    return true;
                return null;   //value is false
            }
            catch (Exception)
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Add the convert back if needed
            return value is bool ? (bool)value : false;
        }
    }
}
