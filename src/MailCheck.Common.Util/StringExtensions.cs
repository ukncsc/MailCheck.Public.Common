using System;
using System.Linq;

namespace MailCheck.Common.Util
{
    public static class StringExtensionMethods
    {
        public static string Escape(this string value)
        {
            string Escape(char c)
            {
                if (char.IsControl(c) && c < 256)
                {
                    return $"\\{(int)c:000}";
                }

                if (c > 255)
                {
                    return $"\\u{((int)c).ToString()}";
                }

                return c.ToString();
            }

            return string.Concat(value.Select(Escape));
        }

        public static bool TryParseExactEnum<T>(this string value, out T t, bool caseInsensitive = true)
            where T : struct =>
            Enum.TryParse(value, caseInsensitive, out t) &&
            Enum.GetNames(typeof(T)).Any(_ => _.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
