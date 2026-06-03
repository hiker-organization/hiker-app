using System.Globalization;

namespace App_Hiker.Converters
{
    // Destaca o item ativo (aba) comparando o índice atual (value) com o índice do item (parameter).
    public class IndexToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int current = ToInt(value);
            int target = ToInt(parameter);

            string resourceKey = current == target ? "Primary" : "BaseContent";

            if (Application.Current?.Resources.TryGetValue(resourceKey, out object? color) == true && color is Color resolved)
            {
                return resolved;
            }

            return Colors.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static int ToInt(object? raw)
        {
            return raw switch
            {
                int i => i,
                string s when int.TryParse(s, out int parsed) => parsed,
                _ => -1
            };
        }
    }
}
