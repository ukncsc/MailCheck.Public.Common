using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using MailCheck.Common.Messaging.Common.Config;
using Microsoft.Extensions.Logging;

namespace MailCheck.Common.Messaging.Common.Processor
{
    internal interface ILambdaJobProcessor
    {
        Task Process(ILambdaContext context, Func<Task<bool>> job);
    }

    internal class LambdaJobProcessor : ILambdaJobProcessor
    {
        private readonly ILogger<LambdaJobProcessor> _log;
        private readonly ILambdaConfig _config;

        public LambdaJobProcessor(ILogger<LambdaJobProcessor> log, ILambdaConfig config)
        {
            _log = log;
            _config = config;
        }

        public async Task Process(ILambdaContext context, Func<Task<bool>> job)
        {
            bool continueProcessing = false;

            do
            {
                try
                {
                    continueProcessing = await job();
                }
                catch (System.Exception e)
                {
                    string formatString =
                        $"Error while processing lambda job {{ExceptionMessage}} {System.Environment.NewLine} {{StackTrace}}";
                    _log.LogError(formatString, e.Message, e.StackTrace);
                }

                _log.LogInformation("TimeRemaining for lambda: {TimeRemaining}, RemainingTimeThreshold: {RemainingTimeThreshold}, ContinueProcessing: {ContinueProcessing}",
                    context.RemainingTime, _config.RemainingTimeTheshold, continueProcessing);

            } while (context.RemainingTime >= _config.RemainingTimeTheshold && continueProcessing);

            _log.LogInformation("Completed processing for lambda");
        }
    }
}