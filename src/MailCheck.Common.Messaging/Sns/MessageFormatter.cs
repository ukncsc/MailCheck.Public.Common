using System;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Message = MailCheck.Common.Messaging.Abstractions.Message;

namespace MailCheck.Common.Messaging.Sns
{
    internal class MessageFormatter : IMessageFormatter
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public TransportMessage Serialize(Message message)
        {
            string stringMessage = JsonConvert.SerializeObject(message, _serializerSettings);

            int byteCount = Encoding.UTF8.GetByteCount(stringMessage);

            if (byteCount > SnsConstants.MessageSizeLimitBytes)
            {
                throw new Exception($"Message {message.GetType().Name} is too big for SNS ({byteCount} exceeds {SnsConstants.MessageSizeLimitBytes} byte limit)");
            }

            return new TransportMessage
            {
                Type = message.GetType().Name,
                Version = GetVersion(message.GetType().GetTypeInfo().Assembly.GetName().Version),
                MessageData = stringMessage,
                Utf8ByteCount = byteCount,
            };
        }

        private string GetVersion(Version version)
        {
            return $"{version.Major}.{version.Minor}.{version.Revision}";
        }
    }
}
