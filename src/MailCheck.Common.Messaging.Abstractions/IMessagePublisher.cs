using System.Threading.Tasks;

namespace MailCheck.Common.Messaging.Abstractions
{
    public interface IMessagePublisher
    {
        Task Publish(Message message, string topic);
        Task Publish(Message message, string topic, string type);
    }
}