using Message = MailCheck.Common.Messaging.Abstractions.Message;

namespace MailCheck.Common.Messaging.Sns
{
    /// <summary>
    /// Serializes a <see cref="Message"/> as a <see cref="TransportMessage"/>
    /// </summary>
    internal interface IMessageFormatter
    {
        TransportMessage Serialize(Message message);
    }
}
