using System;

namespace RememberTheMilkApi.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToString(this DateTime? dateTime, string format = null)
        {
            if (!dateTime.HasValue)
            {
                return string.Empty;
            }
            return format == null ? dateTime.Value.ToString() : dateTime.Value.ToString(format);
        }
    }
}