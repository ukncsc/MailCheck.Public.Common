using System;
using System.Linq;

namespace MailCheck.Common.Util
{
    public static class DomainNameUtils
    {
        public static string ReverseDomainName(string domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return String.Join(".", domain.Split('.').Reverse().ToArray());
        }

        public static string ToCanonicalDomainName(string domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return domain.Trim(' ', '.').ToLower();
        }
    }
}