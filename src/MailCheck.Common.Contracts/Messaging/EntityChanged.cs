using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class EntityChanged : Message
    {
        public string RecordType { get; set; }
        public object NewEntityDetail { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ReasonForChange { get; set; }

        public EntityChanged(string id) : base(id)
        {
        }
    }
}
