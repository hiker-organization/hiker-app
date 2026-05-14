using System.Text.RegularExpressions;

namespace App_Hiker.Utils
{
    public class SpecialCharacters
    {
        public static bool Verify(string value, string pattern)
        {
            return Regex.IsMatch(value, pattern);
        }

        public static string Remove(string value, string pattern)
        {
            return Regex.Replace(value, pattern, "");
        }
    }
}
