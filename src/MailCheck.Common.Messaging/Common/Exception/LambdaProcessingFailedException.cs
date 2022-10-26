namespace MailCheck.Common.Messaging.Common.Exception
{
    internal class LambdaProcessingFailedException : System.Exception
    {
        public LambdaProcessingFailedException(){}

        public LambdaProcessingFailedException(string formatString, params object[] values)
         : base(string.Format(formatString, values)){}

        public LambdaProcessingFailedException(string message)
            : base(message){}
    }
}
