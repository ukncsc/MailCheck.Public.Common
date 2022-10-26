using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class ReminderSuccessful : Message
    {
        public string Service { get; }
        
        public string ResourceId { get; }
        
        public DateTime PollTime { get; }
        
        public ReminderSuccessful(string id, string service, string resourceId, DateTime pollTime) : base(id)
        {
            Service = service;
            ResourceId = resourceId;
            PollTime = pollTime;
        }
    }
}