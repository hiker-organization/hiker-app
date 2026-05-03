using System.Text.RegularExpressions;

namespace App_Hiker.Utils
{
    public class SpecialCharacters
    {
        public static string Remove(string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9\_\-]", "");
        }
    }
}
