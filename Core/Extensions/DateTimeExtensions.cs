using Core.Constants;
using System;
using System.Globalization;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static String ToTimeStamp(this DateTime dt)
        {
            return dt.ToString(DateTimeConstants.DefaultTimeStampFormat);
        }
        public static DateTime ToCustomDateTime(this string s, string format = DateTimeConstants.DefaultDateFormat)
        {
            var date = DateTime.TryParseExact(s: s, format: format, provider: null, style: 0, out var dt)
                ? dt : DateTime.Now;
            return date;
        }

        public static string ToCustomFormatString(this DateTime dt, CultureInfo info, bool isTimeExist = false)
        {

            return isTimeExist ? dt.ToString(DateTimeConstants.DefaultDateAndTimeFormat, info) : dt.ToString(DateTimeConstants.DefaultDateFormat, info);
        }
        public static string ToCustomFormatString(this DateTime dt, string format, CultureInfo info)
        {

            return dt.ToString(format, info);
        }

        public static string ToCustomFormatString(this DateTime dt, bool isTimeExist = false)
        {
            return isTimeExist
                ? dt.ToString(DateTimeConstants.DefaultDateAndTimeFormat, CultureInfo.InvariantCulture)
                : dt.ToString(DateTimeConstants.DefaultDateFormat, CultureInfo.InvariantCulture);
        }

    }

}
