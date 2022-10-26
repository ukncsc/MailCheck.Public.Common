using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.Common.Domain;
using MailCheck.Common.Messaging.Common.Exception;
using MailCheck.Common.Messaging.Sns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MailCheck.Common.Messaging.Common.Processor
{
    internal interface ISqsMessageProcessor
    {
        Task<ProcessSqsResult> Process(SqsMessage message);
    }

    internal class SqsMessageProcessor : ISqsMessageProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITypeLookUp _typeLookUp;
        private readonly IMessageExtractor _messageExtractor;
        private readonly ILogger<SqsMessageProcessor> _log;

        public SqsMessageProcessor(IServiceProvider serviceProvider,
            ITypeLookUp typeLookUp,
            IMessageExtractor messageExtractor,
            ILogger<SqsMessageProcessor> log)
        {
            _serviceProvider = serviceProvider;
            _typeLookUp = typeLookUp;
            _messageExtractor = messageExtractor;
            _log = log;
        }

        public async Task<ProcessSqsResult> Process(SqsMessage message)
        {
            string messageId = message.MessageId;
            string messageReceiptHandle = message.ReceiptHandle;

            using (_log.BeginScope(new Dictionary<string, string>
            {
                ["MessageId"] = messageId,
            }))
            {
                if (!_messageExtractor.TryExtractMessageProperties(message,
                    out string stringMessageType,
                    out Type messageType,
                    out string evntId,
                    out string correlationId,
                    out string causationId,
                    out Message evnt))
                {
                    return ProcessSqsResult.FailedProcessSqsResult;
                }

                Stopwatch stopwatch = Stopwatch.StartNew();

                using (_log.BeginScope(new Dictionary<string, string>
                {
                    ["MessageType"] = stringMessageType,
                    ["CausationId"] = causationId,
                    ["CorrelationId"] = correlationId,
                    ["EventId"] = evntId
                }))
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        try
                        {
                            Type handlerType = typeof(IHandle<>).MakeGenericType(messageType);
                            dynamic handler = scope.ServiceProvider.GetService(handlerType);

                            if (handler == null)
                            {
                                _log.LogError("Failed to find handler for type {MessageType}. MessageId: {MessageId}.", messageType, messageId);
                                return ProcessSqsResult.FailedProcessSqsResult;
                            }

                            _log.LogInformation(
                                "Processing message of type {MessageType} with id {Id}. MessageId: {MessageId} CorrelationId: {CorrelationId} CausationId: {CausationId}",
                                messageType, evntId, messageId, correlationId, causationId);

                            IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                            publisher.BeginPublish(messageId, correlationId);

                            // TODO: Would be good to remove this dynamic stuff
                            await handler.Handle((dynamic)evnt);

                            await publisher.EndPublish();

                            _log.LogInformation(
                                "Processed message of type {MessageType} with id {Id} in {Milliseconds} ms. MessageId: {MessageId} CorrelationId: {CorrelationId} CausationId: {CausationId}",
                                messageType, evntId, stopwatch.Elapsed.TotalMilliseconds, messageId,
                                correlationId, causationId);

                            return new ProcessSqsResult(messageId, messageReceiptHandle);
                        }
                        catch (MailCheckException mailCheckException)
                        {
                            string formatString = $"Error occured processing message with MessageId: {messageId}";
                            _log.LogError(mailCheckException, formatString);
                            return new ProcessSqsResult(messageId, messageReceiptHandle);
                        }
                        catch (System.Exception e)
                        {
                            string formatString = $"Error occured processing message with MessageId: {messageId}";
                            _log.LogError(e, formatString);
                        }

                        return ProcessSqsResult.FailedProcessSqsResult;
                    }
                }

            }
        }
    }
}