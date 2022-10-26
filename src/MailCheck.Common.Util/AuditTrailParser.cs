using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MailCheck.Common.Util
{
    public interface IAuditTrailParser
    {
         List<AuditParseResult> Parse(string auditTrail);
    }

    public class AuditTrailParser : IAuditTrailParser
    {
        public List<AuditParseResult> Parse(string auditTrail)
        {
            List<AuditParseResult> result = new List<AuditParseResult>();

            string pattern = @";;\s->>HEADER<<-\s(?:(?:.|\n|\r)*?)(?:(?:;;\sERROR:\s(?'Error'.*?)(?:\r\n|\r|\n|\\n|\\r|\\n\\r))|(;;\sQUESTION SECTION:))(?:(?:.|\n|\r)*?);;\sQuery\stime:\s(?'QueryTime'.*?)(?:\r\n|\r|\n|\\n|\\r|\\n\\r);;\sSERVER:\s(?'NameServer'.*?)#53(?:(?:.|\n|\r)*?);;\sMSG\sSIZE(?:\s+)rcvd:\s+(?'MessageSize'\d+)";

            foreach (Match m in
                Regex.Matches(auditTrail, pattern))
            {
                AuditParseResult auditParseResult = new AuditParseResult
                {
                    NameServer = m.Groups["NameServer"].Value,
                    Error = m.Groups["Error"].Value == string.Empty ? "No Error" : m.Groups["Error"].Value,
                    QueryTime = m.Groups["QueryTime"].Value,
                    MessageSize = int.Parse(m.Groups["MessageSize"].Value)
                };
                
                result.Add(auditParseResult);
            }

            return result;
        }
    }
}
