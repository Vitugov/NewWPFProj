using System.Globalization;
using System.Windows.Data;

namespace WPFUsefullThings
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal number;
            if (decimal.TryParse(value as string, out number))
            {
                return number;
            }

            return System.Windows.DependencyProperty.UnsetValue;
        }
    }
}
