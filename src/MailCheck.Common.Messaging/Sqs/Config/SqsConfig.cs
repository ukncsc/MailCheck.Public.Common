using System;
using MailCheck.Common.Environment;
using MailCheck.Common.Environment.Abstractions;
using MailCheck.Common.Messaging.Common.Config;

namespace MailCheck.Common.Messaging.Sqs.Config
{
    internal class SqsConfig : ISqsConfig
    {
        public SqsConfig(IEnvironmentVariables environment)
        {
            SqsQueueUrl = environment.Get(ConfigConstants.SqsQueueUrl);
            TimeoutSqs = TimeSpan.FromSeconds(environment.GetAsInt(ConfigConstants.TimeoutSqsSeconds));
            ProcessMessagesSerially = environment.GetAsBoolOrDefault(ConfigConstants.ProcessMessagesSerially, false);
        }

        public string SqsQueueUrl { get; }
        public TimeSpan TimeoutSqs { get; }
        public bool ProcessMessagesSerially { get; }
    }
}