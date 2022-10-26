namespace MailCheck.Common.Messaging.Abstractions
{
    public abstract class VersionedMessage : Message
    {
        protected VersionedMessage(string id, int version) : base(id)
        {
            Version = version;
        }

        public int Version { get; }
    }
}