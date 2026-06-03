using System.Globalization;

namespace App_Hiker.Converters
{
    // Colore as estrelas: amarelo quando a posição (parameter) é <= nota selecionada (value), cinza caso contrário.
    public class RatingToColorConverter : IValueConverter
    {
        private static readonly Color Filled = Color.FromArgb("#FFC107");

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int rating = ToInt(value);
            int position = ToInt(parameter);

            return position <= rating ? Filled : Colors.LightGray;
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
                _ => 0
            };
        }
    }
}
