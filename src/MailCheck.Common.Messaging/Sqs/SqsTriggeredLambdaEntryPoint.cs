using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.Sqs.Factory;
using MailCheck.Common.Messaging.Sqs.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace MailCheck.Common.Messaging.Sqs
{
    public abstract class SqsTriggeredLambdaEntryPoint
    {
        private readonly ISqsEventProcessor _sqsEventProcessor;

        protected SqsTriggeredLambdaEntryPoint(IStartUp startUp)
        {
            _sqsEventProcessor = SqsEventProcessorFactory.Create(startUp);
        }

        protected SqsTriggeredLambdaEntryPoint(IStartUp startUp, Action<IServiceCollection> eventProcessorOverrides)
        {
            _sqsEventProcessor = SqsEventProcessorFactory.Create(startUp, eventProcessorOverrides);
        }

        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            await _sqsEventProcessor.Process(evnt, context);
        }
    }
}
