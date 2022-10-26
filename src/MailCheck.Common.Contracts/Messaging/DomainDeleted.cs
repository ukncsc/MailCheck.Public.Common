using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class DomainDeleted : Message
    {
        public DomainDeleted(string id) : base(id)
        {
            CausationId = null;
            CorrelationId = Guid.NewGuid().ToString();
        }
    }
}
