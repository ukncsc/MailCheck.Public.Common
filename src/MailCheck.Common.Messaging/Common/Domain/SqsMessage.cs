using System;
using System.Collections.Generic;

namespace MailCheck.Common.Messaging.Common.Domain
{
    public class SqsMessage
    {
        public SqsMessage(string messageId, string receiptHandle, DateTime timestamp, Dictionary<string,string> messageAttributes, string body)
        {
            MessageId = messageId;
            ReceiptHandle = receiptHandle;
            Timestamp = timestamp;
            MessageAttributes = messageAttributes;
            Body = body;
        }

        public string MessageId { get; }
        public string ReceiptHandle { get; }
        public DateTime Timestamp { get; }
        public Dictionary<string, string> MessageAttributes { get; }
        public string Body { get; }
    }
}