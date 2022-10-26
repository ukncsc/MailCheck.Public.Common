using System.Threading.Tasks;
using Amazon.Lambda.Core;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.CloudWatch.Factory;
using MailCheck.Common.Messaging.Common.Processor;

namespace MailCheck.Common.Messaging.CloudWatch
{
    public abstract class CloudWatchTriggeredLambdaEntryPoint
    {
        private readonly ICloudWatchProcessor _cloudWatchProcessor;

        protected CloudWatchTriggeredLambdaEntryPoint(IStartUp startUp)
        {
            _cloudWatchProcessor = CloudWatchProcessorFactory.Create(startUp);
        }

        public async Task FunctionHandler(ScheduledEvent evnt, ILambdaContext context)
        {
            await _cloudWatchProcessor.Process(context);
        }
    }
}