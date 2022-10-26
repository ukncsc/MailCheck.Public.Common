namespace MailCheck.Common.Messaging.Common.Domain
{
    internal class ProcessSqsResult
    {
        public static readonly ProcessSqsResult FailedProcessSqsResult = new ProcessSqsResult(false, null, null);

        public ProcessSqsResult(string messageId, string receiptHandle)
            : this(true, messageId, receiptHandle)
        {

        }
        private ProcessSqsResult(bool success, string messageId, string receiptHandle)
        {
            Success = success;
            MessageId = messageId;
            ReceiptHandle = receiptHandle;
        }

        public bool Success { get; }
        public string MessageId { get; }
        public string ReceiptHandle { get; }

        protected bool Equals(ProcessSqsResult other)
        {
            return Success == other.Success && string.Equals(MessageId, other.MessageId) && string.Equals(ReceiptHandle, other.ReceiptHandle);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProcessSqsResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Success.GetHashCode();
                hashCode = (hashCode * 397) ^ (MessageId != null ? MessageId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ReceiptHandle != null ? ReceiptHandle.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}