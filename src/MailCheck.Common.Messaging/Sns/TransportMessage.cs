namespace MailCheck.Common.Messaging.Sns
{
    internal class TransportMessage
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public string MessageData { get; set; }
        public int Utf8ByteCount { get; set; }
    }
}
