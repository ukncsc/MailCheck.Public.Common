using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging.AssetManager
{
    public class SubscriptionDeleted : Message
    {
        public string AssetId { get; set; }
        public string AssetValue { get; set; }
        public string AssetType { get; set; }
        public string DeletedByUserId { get; set; }

        public SubscriptionDeleted(string id) : base(id)
        {
        }
    }
}
