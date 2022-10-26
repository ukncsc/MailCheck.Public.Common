using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging.AssetManager
{
    public class AssetTeamAssignmentChanged : Message
    {
        public string AssetId { get; set; }
        public string AssetValue { get; set; }
        public string AssetType { get; set; }
        public string[] TeamIds { get; set; }
        public string TeamStatus { get; set; }
        public string ChangedByUserId { get; set; }

        public AssetTeamAssignmentChanged(string id) : base(id)
        {
        }
    }
}
