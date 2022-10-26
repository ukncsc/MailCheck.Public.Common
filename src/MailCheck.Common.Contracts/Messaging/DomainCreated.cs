using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class DomainCreated : Message
    {
        public DomainCreated(string id, string createdBy, DateTime creationDate)
            : base(id)
        {
            CausationId = null;
            CorrelationId = Guid.NewGuid().ToString();
            CreatedBy = createdBy;
            CreationDate = creationDate;
        }

        public string CreatedBy { get; }

        public DateTime CreationDate { get; }
    }
}
