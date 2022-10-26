using System.Collections.Generic;
using System.Threading.Tasks;
using MailCheck.Common.Messaging.Abstractions;
using Message = MailCheck.Common.Messaging.Abstractions.Message;

namespace MailCheck.Common.Messaging.Sns
{
    internal class PublishingMessageDispatcher : IMessageDispatcher, IPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        private readonly List<MessageDispatchItem> _messages = new List<MessageDispatchItem>();

        private string _causationId, _correlationId;

        public PublishingMessageDispatcher(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Dispatch(Message message, string topic)
        {
            Dispatch(message, topic, null);
        }

        public void Dispatch(Message message, string topic, string type)
        {
            _messages.Add(new MessageDispatchItem(message, topic, type));
        }

        public void BeginPublish(string causationId, string correlationId)
        {
            _causationId = causationId;
            _correlationId = correlationId;
        }

        public async Task EndPublish()
        {
            foreach (var message in _messages)
            {
                Message currentMessage = message.Message;

                currentMessage.CorrelationId = _correlationId;
                currentMessage.CausationId = _causationId;

                await _messagePublisher.Publish(currentMessage, message.Topic, message.Type);
            }
        }

        private class MessageDispatchItem
        {
            public MessageDispatchItem(Message message, string topic, string type = null)
            {
                Message = message;
                Topic = topic;
                Type = type;
            }

            public Message Message { get; }
            public string Topic { get; }
            public string Type { get; }
        }
    }
}
