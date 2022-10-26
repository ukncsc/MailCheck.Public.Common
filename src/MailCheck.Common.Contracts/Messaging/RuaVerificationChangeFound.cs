using System;
using MailCheck.Common.Messaging.Abstractions;

namespace MailCheck.Common.Contracts.Messaging
{
    public class RuaVerificationChangeFound : Message
    {
        public string Service { get; }

        public string RuaTag { get; }
        
        public RuaVerificationChangeFound(string id, string service, string ruaTag)
            : base(id)
        {
            Service = service;
            RuaTag = ruaTag;
        }
    }
}