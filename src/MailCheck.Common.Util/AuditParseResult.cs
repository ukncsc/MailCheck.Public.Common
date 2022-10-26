using System;
namespace MailCheck.Common.Util
{
    public class AuditParseResult
    {
        public string NameServer { get; set; }
        public int MessageSize { get; set; }
        public string QueryTime { get; set; }
        public string Error { get; set; }
    }
}
