namespace MailCheck.Common.Messaging.Sns
{
    public static class SnsConstants
    {
        /// <summary>
        /// Maximum SNS message size 256KB
        /// https://docs.aws.amazon.com/sns/latest/dg/large-message-payloads.html
        /// </summary>
        public const int MessageSizeLimitBytes = 256 * 1024;

        /// <summary>
        /// Maximum number of messages in PublishBatchRequest: 10 (default)
        /// https://docs.aws.amazon.com/general/latest/gr/sns.html
        /// </summary>
        public const int BatchMessageCountLimit = 10;
    }
}
