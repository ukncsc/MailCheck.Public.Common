using System;
using System.Collections.Generic;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.Common.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MailCheck.Common.Messaging
{
    public interface IMessageExtractor
    {
        bool TryExtractMessageProperties(
            SqsMessage message,
            out string stringMessageType,
            out Type messageType,
            out string evntId,
            out string correlationId,
            out string causationId,
            out Message evnt);
    }

    /// <summary>
    /// Message extractor which expects a standard internal Mail Check message with 
    /// Type and Version SQS message attributes and default JsonConvert deserialization 
    /// of the message body.
    /// </summary>
    internal class InternalMessageExtractor : IMessageExtractor
    {
        private readonly ITypeLookUp _typeLookUp;
        private readonly ILogger<InternalMessageExtractor> _log;

        public InternalMessageExtractor(
            ITypeLookUp typeLookUp,
            ILogger<InternalMessageExtractor> log)
        {
            _typeLookUp = typeLookUp;
            _log = log;
        }

        public bool TryExtractMessageProperties(
            SqsMessage message,
            out string stringMessageType,
            out Type messageType,
            out string evntId,
            out string correlationId,
            out string causationId,
            out Message evnt)
        {
            stringMessageType = null;
            messageType = null;
            evntId = null;
            correlationId = null;
            causationId = null;
            evnt = null;

            string messageId = message.MessageId;
            string messageReceiptHandle = message.ReceiptHandle;

            Dictionary<string, string> caseInsensitiveDictionary = new Dictionary<string, string>(message.MessageAttributes, StringComparer.OrdinalIgnoreCase);

            if (!caseInsensitiveDictionary.TryGetValue(MessageAttributes.Type.ToLower(), out stringMessageType))
            {
                _log.LogError("SQS message attributes didnt contain type attribute. MessageId: {MessageId} MessageBody: {MessageBody}.", messageId, message.Body);
                return false;
            }

            if (!_typeLookUp.TryGetMessageType(stringMessageType, out messageType))
            {
                _log.LogError("Failed to find type of {MessageType}. MessageId: {MessageId}.", stringMessageType, messageId);
                return false;
            }

            try
            {
                evnt = (Message)JsonConvert.DeserializeObject(message.Body, messageType);
            }
            catch (System.Exception ex)
            {
                _log.LogError(ex, $"Error deserializing message body as type {messageType}");
                return false;
            }

            evnt.MessageId = messageId;
            evnt.Timestamp = message.Timestamp;

            evntId = (string)evnt.Id;
            correlationId = (string)evnt.CorrelationId;
            causationId = (string)evnt.CausationId;

            return true;
        }
    }
}