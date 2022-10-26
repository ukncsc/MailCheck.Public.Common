using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging.AssetManager
{
    public class SubscriptionStatusChanged : Message
    {
        public string AssetId { get; set; }
        public string AssetValue { get; set; }
        public string AssetType { get; set; }
        public string Status { get; set; }
        public string ChangedByUserId { get; set; }
        public string VerificationStatus { get; set; }

        public SubscriptionStatusChanged(string id) : base(id)
        {
        }
    }
}
