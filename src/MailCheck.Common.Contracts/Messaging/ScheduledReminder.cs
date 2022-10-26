using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class ScheduledReminder : Message
    {
        public string ResourceId { get; }

        public ScheduledReminder(string id, string resourceId)
            : base(id)
        {
            ResourceId = resourceId;
        }
    }
}