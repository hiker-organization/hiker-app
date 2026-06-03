using System.Globalization;

namespace App_Hiker.Converters
{
    // Inverte um booleano. Útil para IsEnabled="{Binding IsBusy, Converter=...}".
    public class InverseBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }
    }
}
