using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class DomainMissing : Message
    {
        public DomainMissing(string id) : base(id)
        {
        }
    }
}