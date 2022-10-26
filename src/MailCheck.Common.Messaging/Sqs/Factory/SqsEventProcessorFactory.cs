using System;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using MailCheck.Common.Environment;
using MailCheck.Common.Environment.FeatureManagement;
using MailCheck.Common.Logging;
using MailCheck.Common.Logging.Telemetry;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.Common.Config;
using MailCheck.Common.Messaging.Common.Processor;
using MailCheck.Common.Messaging.Common.Utils;
using MailCheck.Common.Messaging.Sns;
using MailCheck.Common.Messaging.Sqs.Config;
using MailCheck.Common.Messaging.Sqs.Processor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MailCheck.Common.Messaging.Sqs.Factory
{
    internal class SqsEventProcessorFactory
    {
        public static ISqsEventProcessor Create(IStartUp startUp, Action<IServiceCollection> eventProcessorOverrides = null)
        {
            IServiceCollection collection = new ServiceCollection();

            startUp.ConfigureServices(collection);

            collection.ExpandConcreteHandlers();

            collection
                .AddEnvironment()
                .AddConditionally(
                    "BatchPublishToSns", 
                    srv => 
                    {
                        srv.AddScoped<IMessageDispatcher, PublishingMessageDispatcherBatched>();
                    }, 
                    srv =>
                    {
                        srv.AddScoped<IMessageDispatcher, PublishingMessageDispatcher>();
                    })
                .AddScoped(_ => (IPublisher)_.GetRequiredService<IMessageDispatcher>()) //this and the line below are supposed to be like this so that the same instance is accessed in the same scope in client and host code
                .AddTransient<IMessagePublisher, SnsMessagePublisher>()
                .AddTransient<IMessageFormatter, MessageFormatter>()
                .AddTransient<ITransportMessagePublisher, SnsBatchedTransportMessagePublisher>()
                .AddTransient<ISqsEventProcessor, SqsEventProcessor>()
                .AddTransient<ISqsConfig, SqsConfig>()
                .AddTransient<IAmazonSQS>(_ => new AmazonSQSClient(new EnvironmentVariablesAWSCredentials()))
                .AddTransient<ISqsMessagesProcessor, SqsMessagesProcessor>()
                .AddTransient<ISqsMessageProcessor, SqsMessageProcessor>()
                .AddTransient<ITypeLookUp, TypeLookUp>()
                .AddTransient<IMessageExtractor, InternalMessageExtractor>()
                .AddSerilogLogging();

            new TelemetryConfig()
                .InstrumentAwsSdk()
                .InstrumentFlurlHttp();

            if (eventProcessorOverrides != null)
            {
                eventProcessorOverrides(collection);
            }

            return collection.BuildServiceProvider()
                .GetRequiredService<ISqsEventProcessor>();
        }
    }
}
