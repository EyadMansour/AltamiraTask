using Core.Constants;
using System.Text.RegularExpressions;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string s)
        {
            var emailRegex = RegexConstants.EmailRegex;
            var re = new Regex(emailRegex);
            return re.IsMatch(s);

        }

        public static bool IsUsername(this string s)
        {
            var usernameRegex = RegexConstants.UserNameRegex;
            var re = new Regex(usernameRegex);
            return re.IsMatch(s);
        }
        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
