using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using MailCheck.Common.Messaging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MailCheck.Common.Messaging.Sns
{
    public class SnsMessagePublisher : IMessagePublisher
    {
        private const string Type = "Type";
        private const string Version = "Version";

        private readonly IAmazonSimpleNotificationService _simpleNotificationService;

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public SnsMessagePublisher(IAmazonSimpleNotificationService simpleNotificationService)
        {
            _simpleNotificationService = simpleNotificationService;
        }

        public Task Publish(Message message, string topic)
        {
            return Publish(message, topic, null);
        }

        public async Task Publish(Message message, string topic, string type)
        {
            string stringMessage = JsonConvert.SerializeObject(message, _serializerSettings);

            PublishRequest publishRequest = new PublishRequest(topic, stringMessage)
            {
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        Type, new MessageAttributeValue
                        {
                            StringValue = type ?? message.GetType().Name,
                            DataType = "String"
                        }
                    },
                    {
                        Version, new MessageAttributeValue
                        {
                            StringValue = GetVersion(message.GetType().GetTypeInfo().Assembly.GetName().Version),
                            DataType = "String"
                        }
                    }
                }
            };

            await _simpleNotificationService.PublishAsync(publishRequest);
        }

        private string GetVersion(Version version)
        {
            return $"{version.Major}.{version.Minor}.{version.Revision}";
        }
    }
}