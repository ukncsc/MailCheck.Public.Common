using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging.AssetManager
{
    public class AssetDeleted : Message
    {
        public string AssetId { get; set; }
        public string AssetValue { get; set; }
        public string AssetType { get; set; }
        public string DeletedByUserId { get; set; }

        public AssetDeleted(string id) : base(id)
        {
        }
    }
}
