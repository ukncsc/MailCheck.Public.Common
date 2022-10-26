using System.Linq;

namespace MailCheck.Common.Util
{
    public static class StringEncodingExtensionMethods
    {
        public static string EncodeControlChars(this string value)
        {
            return string.Concat(value.Select(_ => char.IsControl(_) ? $"\\{(int) _:000}" : _.ToString()));
        }
    }
}