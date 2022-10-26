using System.Collections.Generic;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Findings
{
    public class FindingsChanged : Message
    {
        public FindingsChanged(string id) : base(id) { }

        public string Domain { get; set; }
        // Same as in DomainStatusEvaluation message
        public string RecordType { get; set; }
        public IList<Finding> Added { get; set; }
        public IList<Finding> Sustained { get; set; }
        public IList<Finding> Removed { get; set; }
    }
}