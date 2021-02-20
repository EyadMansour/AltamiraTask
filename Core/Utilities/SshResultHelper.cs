using System;
using System.Text.RegularExpressions;

namespace Core.Utilities
{
    public static class SshResultHelper
    {

        public static string ClearSqlCountResult(string s)
        {
            var r = Regex.Replace(s, @"^\s*$\n|\r|\s|(COUNT\(\*\))|\-", String.Empty, RegexOptions.Multiline);
            return r;
        }
        public static string ClearSqlOpenModeResult(string s)
        {
            var r = Regex.Replace(s, @"^\s*$\n|\r|\s|OPEN_MODE|\-", String.Empty, RegexOptions.Multiline);
            return r;
        }
        public static string ClearSqlForceLoggingResult(string s)
        {
            var r = Regex.Replace(s, @"^\s*$\n|\r|\s|FORCE_LOGGING|\-", String.Empty, RegexOptions.Multiline);
            return r;
        }
        public static string ClearSqlScnResult(string s)
        {
            var r = Regex.Replace(s, @"^\s*$\n|\r|\s|CURRENT_SCN|MAX\(FIRST_CHANGE#\)|\-", String.Empty, RegexOptions.Multiline);
            return r;
        }
    }
}
