using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class DeleteScheduledReminder : Message
    {
        public string Service { get; }
        
        public string ResourceId { get; }
        
        public DeleteScheduledReminder(string id, string service, string resourceId) : base(id)
        {
            Service = service;
            ResourceId = resourceId;
        }
    }
}