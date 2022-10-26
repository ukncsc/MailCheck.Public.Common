using System.Threading.Tasks;

namespace MailCheck.Common.Messaging.Abstractions
{
    public interface IHandle<in T> where T : Message
    {
        Task Handle(T message);
    }
}
