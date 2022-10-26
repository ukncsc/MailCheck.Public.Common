using System;
using MailCheck.Common.Environment;
using MailCheck.Common.Environment.Abstractions;
using MailCheck.Common.Messaging.Common.Config;

namespace MailCheck.Common.Messaging.CloudWatch.Config
{
    internal class CloudWatchConfig : ILambdaConfig
    {
        public CloudWatchConfig(IEnvironmentVariables environment)
        {
            RemainingTimeTheshold = TimeSpan.FromSeconds(environment.GetAsInt(ConfigConstants.RemainingTimeThresholdSeconds));
        }

        public TimeSpan RemainingTimeTheshold { get; }
    }
}