using System;
using System.Collections.Generic;
using System.Text;

namespace MailCheck.Common.Messaging.Common.Exception
{
    public class MailCheckException : System.Exception
    {
        public MailCheckException()
        {
        }

        public MailCheckException(string formatString, params object[] values)
            : base(string.Format(formatString, values))
        {
        }

        public MailCheckException(string message)
            : base(message)
        {
        }
    }
}
