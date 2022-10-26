using System;

namespace MailCheck.Common.Contracts.Advisories
{
    public class AdvisoryMessage : IEquatable<AdvisoryMessage>
    {
        public AdvisoryMessage(Guid id, MessageType messageType, string text, string markDown, MessageDisplay messageDisplay = MessageDisplay.Standard)
        {
            Id = id;
            MessageType = messageType;
            Text = text;
            MarkDown = markDown;
            MessageDisplay = messageDisplay;
        }

        public Guid Id { get; }
        public MessageType MessageType { get; }
        public string Text { get; }
        public string MarkDown { get; }
        public MessageDisplay MessageDisplay { get; }

        public bool Equals(AdvisoryMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other != null && this != null && Id.Equals(other.Id); ;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AdvisoryMessage)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ MessageType.GetHashCode();
                hashCode = (hashCode * 397) ^ Text.GetHashCode();
                hashCode = (hashCode * 397) ^ (MarkDown != null ? MarkDown.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ MessageDisplay.GetHashCode();
                return hashCode;
            }
        }
    }
}