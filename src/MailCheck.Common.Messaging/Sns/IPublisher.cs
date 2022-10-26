using System.Threading.Tasks;

namespace MailCheck.Common.Messaging.Sns
{
    internal interface IPublisher
    {
        void BeginPublish(string causationId, string correlationId);
        Task EndPublish();
    }
}