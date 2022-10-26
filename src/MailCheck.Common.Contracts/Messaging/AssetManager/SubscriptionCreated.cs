using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging.AssetManager
{
    public class SubscriptionCreated : Message
    {
        public string AssetId { get; set; }
        public string AssetValue { get; set; }
        public string AssetType { get; set; }
        public string OwnerOrgId { get; set; }
        public string[] TeamIds { get; set; }
        public string SubscriptionStatus { get; set; }
        public string SubscriptionId { get; set; }
        public string CreatedByUserId { get; set; }
        public string VerificationStatus { get; set; }

        public SubscriptionCreated(string id) : base(id)
        {
        }
    }
}
