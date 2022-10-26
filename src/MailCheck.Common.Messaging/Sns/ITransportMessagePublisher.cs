using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailCheck.Common.Messaging.Sns
{
    internal interface ITransportMessagePublisher
    {
        Task Publish(IEnumerable<TransportMessage> items, string topicArn);
    }
}