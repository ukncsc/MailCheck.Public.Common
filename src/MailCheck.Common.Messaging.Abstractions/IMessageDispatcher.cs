namespace MailCheck.Common.Messaging.Abstractions
{
    public interface IMessageDispatcher
    {
        void Dispatch(Message message, string topic);
        void Dispatch(Message message, string topic, string type);
    }
}