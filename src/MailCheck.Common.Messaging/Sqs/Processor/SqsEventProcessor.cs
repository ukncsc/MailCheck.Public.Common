using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MailCheck.Common.Messaging.Common.Domain;
using MailCheck.Common.Messaging.Common.Processor;
using MailCheck.Common.Messaging.Sqs.Mapping;

namespace MailCheck.Common.Messaging.Sqs.Processor
{
    internal interface ISqsEventProcessor
    {
        Task Process(SQSEvent evnt, ILambdaContext context);
    }

    internal class SqsEventProcessor : ISqsEventProcessor
    {
        private readonly ISqsMessagesProcessor _sqsMessagesProcessor;

        public SqsEventProcessor(ISqsMessagesProcessor sqsMessagesProcessor)
        {
            _sqsMessagesProcessor = sqsMessagesProcessor;
        }

        public Task Process(SQSEvent evnt, ILambdaContext context)
        {
            List<SqsMessage> sqsMessages = evnt.Records.Select(_ => _.Map()).ToList();
            return _sqsMessagesProcessor.Process(sqsMessages);
        }
    }
}