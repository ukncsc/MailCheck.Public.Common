using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class CreateScheduledReminder : Message
    {
        public string Service { get; }

        public string ResourceId { get; }

        public DateTime ScheduledTime { get; }

        public CreateScheduledReminder(string id, string service, string resourceId, DateTime scheduledTime)
            : base(id)
        {
            Service = service;
            ResourceId = resourceId;
            ScheduledTime = scheduledTime;
        }
    }
}