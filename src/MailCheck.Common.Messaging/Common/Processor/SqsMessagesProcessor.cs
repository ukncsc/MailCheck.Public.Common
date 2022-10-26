using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using MailCheck.Common.Messaging.Common.Config;
using MailCheck.Common.Messaging.Common.Domain;
using MailCheck.Common.Messaging.Common.Exception;
using Microsoft.Extensions.Logging;

namespace MailCheck.Common.Messaging.Common.Processor
{
    internal interface ISqsMessagesProcessor
    {
        Task Process(List<SqsMessage> messages);
    }

    internal class SqsMessagesProcessor : ISqsMessagesProcessor
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly ISqsMessageProcessor _messageProcessor;
        private readonly ISqsConfig _sqsConfig;
        private readonly ILogger<SqsMessagesProcessor> _log;

        public SqsMessagesProcessor(IAmazonSQS sqsClient, 
            ISqsMessageProcessor messageProcessor,
            ISqsConfig sqsConfig,
            ILogger<SqsMessagesProcessor> log)
        {
            _sqsClient = sqsClient;
            _messageProcessor = messageProcessor;
            _sqsConfig = sqsConfig;
            _log = log;
        }

        public async Task Process(List<SqsMessage> messages)
        {
            List<ProcessSqsResult> processSqsResults = new List<ProcessSqsResult>();

            if (_sqsConfig.ProcessMessagesSerially)
            {
                foreach (var message in messages)
                {
                    ProcessSqsResult result = await _messageProcessor.Process(message);
                    processSqsResults.Add(result);
                }
            }
            else
            {
                List<Task<ProcessSqsResult>> tasks = messages.Select(_messageProcessor.Process).ToList();
                ProcessSqsResult[] results = await Task.WhenAll(tasks);
                processSqsResults.AddRange(results);

            }

            var successfulMessages = processSqsResults
                .Where(result => result.Success)
                .ToList();

            var failedMessages = processSqsResults
                .Where(result => !result.Success)
                .ToList();

            bool failures = false;
            if (failedMessages.Count > 0)
            {
                _log.LogInformation("Failed to process {MessageCount} sqs messages.", failedMessages.Count);
                failures = true;
            }

            if (successfulMessages.Count > 0)
            {
                var deleteMessageBatchRequestEntries = successfulMessages
                    .Select(message => new DeleteMessageBatchRequestEntry(message.MessageId, message.ReceiptHandle))
                    .ToList();

                try
                {
                    var response = await _sqsClient.DeleteMessageBatchAsync(_sqsConfig.SqsQueueUrl, deleteMessageBatchRequestEntries);

                    if (response.HttpStatusCode != HttpStatusCode.OK)
                    {
                        _log.LogError("Failed to delete messages with http status code {HttpStatusCode}.", response.HttpStatusCode);
                        failures = true;
                    }

                    if (response.Failed.Any())
                    {
                        string errorValues = string.Join(System.Environment.NewLine, response.Failed.Select(entry => $"Message: {entry.Message}, Code: {entry.Code}, Id: {entry.Message}, SenderFault: {entry.SenderFault}."));
                        _log.LogError("Failed to delete {DeleteFailureCount} messages with errors {Errors}.", response.Failed.Count, errorValues);
                        failures = true;
                    }
                }
                catch(System.Exception ex)
                {
                    _log.LogError(ex, "Failed to delete messages with exception.");
                    failures = true;
                }
            }

            if (failures)
            {
                throw new LambdaProcessingFailedException();
            }
        }
    }
}