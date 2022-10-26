namespace MailCheck.Common.Messaging.Common.Config
{
    internal class ConfigConstants
    {
        public const string SqsQueueUrl = "SqsQueueUrl";
        public const string TimeoutSqsSeconds = "TimeoutSqsSeconds";
        public const string RemainingTimeThresholdSeconds = "RemainingTimeThresholdSeconds";
        public const string MaxNumberOfMessages = "MaxNumberOfMessages";
        public const string WaitTimeSeconds = "WaitTimeSeconds";
        public const string ProcessMessagesSerially = "ProcessMessagesSerially";
    }
}