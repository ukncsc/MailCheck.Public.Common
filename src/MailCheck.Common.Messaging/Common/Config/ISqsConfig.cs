using System;

namespace MailCheck.Common.Messaging.Common.Config
{
    internal interface ISqsConfig
    {
        string SqsQueueUrl { get; }
        TimeSpan TimeoutSqs { get; }
        bool ProcessMessagesSerially { get; }
    }
}