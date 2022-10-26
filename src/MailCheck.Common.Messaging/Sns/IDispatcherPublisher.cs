using System.Threading.Tasks;

namespace MailCheck.Common.Messaging.Sns
{
    internal interface IDispatcherPublisher
    {
        Task Publish();
    }
}