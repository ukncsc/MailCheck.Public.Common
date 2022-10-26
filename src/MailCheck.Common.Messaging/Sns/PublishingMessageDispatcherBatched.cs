using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailCheck.Common.Messaging.Abstractions;
using Message = MailCheck.Common.Messaging.Abstractions.Message;

namespace MailCheck.Common.Messaging.Sns
{
    internal class PublishingMessageDispatcherBatched : IMessageDispatcher, IPublisher
    {
        private readonly IMessageFormatter _formatter;
        private readonly ITransportMessagePublisher _messagePublisher;
        
        private readonly List<MessageDispatchItem> _messages = new List<MessageDispatchItem>();

        public string CorrelationId { get; private set; }
        public string CausationId { get; private set; }

        public PublishingMessageDispatcherBatched(
            IMessageFormatter formatter,
            ITransportMessagePublisher messagePublisher)
        {
            _formatter = formatter;
            _messagePublisher = messagePublisher;
        }

        public void Dispatch(Message message, string topic)
        {
            Dispatch(message, topic, null);
        }

        public void Dispatch(Message message, string topic, string type)
        {
            message.CorrelationId = CorrelationId;
            message.CausationId = CausationId;

            var transportMessage = _formatter.Serialize(message);

            if (type != null)
            {
                transportMessage.Type = type;
            }

            var item = new MessageDispatchItem
            {
                Message = transportMessage,
                Topic = topic,
            };

            _messages.Add(item);
        }

        public void BeginPublish(string causationId, string correlationId)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public async Task EndPublish()
        {
            foreach (var group in _messages.GroupBy(m => m.Topic))
            {
                var topicArn = group.Key;
                var items = group.Select(mdi => mdi.Message);

                await _messagePublisher.Publish(items, topicArn);
            }
        }

        class MessageDispatchItem
        {
            public TransportMessage Message { get; set; }
            public string Topic { get; set; }
        }
    }
}
