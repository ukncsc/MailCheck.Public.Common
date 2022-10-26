using System;

namespace MailCheck.Common.Messaging.Common.Utils
{
    public static class TimestampConversionExtensions
    {
        public static DateTime MillisecondTimestampToDateTime(this string millisecondTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(long.Parse(millisecondTimestamp));
        }
    }
}
