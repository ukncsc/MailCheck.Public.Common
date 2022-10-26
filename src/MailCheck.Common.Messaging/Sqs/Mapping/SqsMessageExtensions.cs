using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.SQSEvents;
using MailCheck.Common.Messaging.Common.Domain;
using MailCheck.Common.Messaging.Common.Utils;

namespace MailCheck.Common.Messaging.Sqs.Mapping
{
    public static class SqsMessageExtensions
    {
        public static SqsMessage Map(this SQSEvent.SQSMessage message)
        {
            Dictionary<string, string> attributes = message.MessageAttributes.Where(_ => !string.IsNullOrWhiteSpace(_.Value.StringValue))
                .ToDictionary(_ => _.Key, _ => _.Value.StringValue);

            Dictionary<string, string> attributesDictionary =

            message.Attributes.ToDictionary(_ => _.Key.ToLowerInvariant(), _ => _.Value);

            DateTime timeStamp = attributesDictionary["senttimestamp"].MillisecondTimestampToDateTime();

            return new SqsMessage(
                message.MessageId,
                message.ReceiptHandle,
                timeStamp,
                attributes,
                message.Body);
        }
    }
}