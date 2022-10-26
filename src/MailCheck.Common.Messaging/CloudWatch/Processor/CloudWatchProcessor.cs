using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.Common.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace MailCheck.Common.Messaging.CloudWatch.Processor
{
    internal class CloudWatchProcessor : ICloudWatchProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILambdaJobProcessor _lambdaJobProcessor;

        public CloudWatchProcessor(IServiceProvider serviceProvider,
            ILambdaJobProcessor lambdaJobProcessor)
        {
            _serviceProvider = serviceProvider;
            _lambdaJobProcessor = lambdaJobProcessor;
        }

        public async Task Process(ILambdaContext context)
        {
            async Task<bool> Func()
            {
                IProcess processor = _serviceProvider.GetRequiredService<IProcess>();

                ProcessResult processResult = await processor.Process();

                return processResult.ContinueProcessing;
            }

            await _lambdaJobProcessor.Process(context, Func);
        }
    }
}
