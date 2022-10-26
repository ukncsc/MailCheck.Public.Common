using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging.AssetManager
{
    public class AssetVerificationStatusChanged : Message
    {
        public string AssetId { get; set; }
        public string AssetValue { get; set; }
        public string AssetType { get; set; }
        public string AssetVerificationStatus { get; set; }
        public string ChangedByUserId { get; set; }

        public AssetVerificationStatusChanged(string id) : base(id)
        {
        }
    }
}
