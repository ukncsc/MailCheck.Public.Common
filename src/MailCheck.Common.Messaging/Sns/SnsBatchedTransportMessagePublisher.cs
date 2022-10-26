using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace MailCheck.Common.Messaging.Sns
{
    internal class SnsBatchedTransportMessagePublisher : ITransportMessagePublisher
    {
        private const string Type = "Type";
        private const string Version = "Version";
        
        private readonly IAmazonSimpleNotificationService _simpleNotificationService;

        public SnsBatchedTransportMessagePublisher(IAmazonSimpleNotificationService simpleNotificationService)
        {
            _simpleNotificationService = simpleNotificationService;
        }

        public async Task Publish(IEnumerable<TransportMessage> items, string topicArn)
        {
            int sum = 0;
            var batch = new List<PublishBatchRequestEntry>();
            
            foreach (var message in items)
            {
                if (batch.Count == SnsConstants.BatchMessageCountLimit || (sum + message.Utf8ByteCount) > SnsConstants.MessageSizeLimitBytes)
                {
                    await PublishToSnsAndCheckResponse(topicArn, batch);

                    sum = 0;
                    batch.Clear();
                }

                var pubReq = new PublishBatchRequestEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = message.MessageData,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        [Type] = new MessageAttributeValue
                        {
                            StringValue = message.Type,
                            DataType = "String"
                        },
                        [Version] = new MessageAttributeValue
                        {
                            StringValue = message.Version,
                            DataType = "String"
                        },
                    }
                };

                sum += message.Utf8ByteCount;
                batch.Add(pubReq);
            }

            if (batch.Count > 0)
            {
                await PublishToSnsAndCheckResponse(topicArn, batch);
            }
        }

        private async Task PublishToSnsAndCheckResponse(string topicArn, List<PublishBatchRequestEntry> batch)
        {
            var publishRequest = new PublishBatchRequest
            {
                PublishBatchRequestEntries = batch.ToList(),
                TopicArn = topicArn
            };

            var response = await _simpleNotificationService.PublishBatchAsync(publishRequest);

            if (response.Failed.Count > 0)
            {
                throw new Exception($"Failed to publish messages to SNS topic {topicArn} with following errors:\n{string.Join("\n", response.Failed.Select(f => f.Message))}");
            }
        }
    }
}