using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace MailCheck.Common.Messaging.Common.Processor
{
    public interface ICloudWatchProcessor
    {
        Task Process(ILambdaContext context);
    }
}