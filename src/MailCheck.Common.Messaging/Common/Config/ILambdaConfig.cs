using System;

namespace MailCheck.Common.Messaging.Common.Config
{
    internal interface ILambdaConfig
    {
        TimeSpan RemainingTimeTheshold { get; }
    }
}