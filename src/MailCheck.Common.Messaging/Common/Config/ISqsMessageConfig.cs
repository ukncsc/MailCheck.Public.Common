namespace MailCheck.Common.Messaging.Common.Config
{
    internal interface ISqsMessageConfig : ISqsConfig
    {
        int MaxNumberOfMessages { get; }
        int WaitTimeSeconds { get; }
    }
}