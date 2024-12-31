using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlideFun.Stores
{

    public class StringConverter : IValueConverter
    {
        //change status Selected Option (string) into  IsChecked (bool)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }

        // change status IsChecked (bool) into Selected Option (string)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked)
            {
                return parameter?.ToString();
            }
            return Binding.DoNothing;
        }
    }

}
