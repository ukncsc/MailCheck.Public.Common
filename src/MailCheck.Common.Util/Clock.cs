using System;
using System.Collections.Generic;
using System.Text;

namespace MailCheck.Common.Util
{
    public interface IClock
    {
        DateTime GetDateTimeUtc();
    }

    public class Clock : IClock
    {
        public DateTime GetDateTimeUtc()
        {
            return DateTime.UtcNow;
        }
    }
}
