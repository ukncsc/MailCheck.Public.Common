using System;
using System.Collections.Generic;
using MailCheck.Common.Contracts.Findings;

namespace MailCheck.Common.Processors.Notifiers
{
    public class MessageEqualityComparer : IEqualityComparer<Finding>
    {
        public bool Equals(Finding x, Finding y)
        {
            return y != null && x != null
                && String.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(x.EntityUri, y.EntityUri, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Finding obj)
        {
            return (obj?.Name?.GetHashCode() ?? 0);
        }
    }
}
