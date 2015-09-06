using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sanet.Kniffel.Converters
{
    public class LocalizeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string typedValue = value as string;
            return !String.IsNullOrEmpty(typedValue) && bool.Parse(typedValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToLower();
        }
    }
}