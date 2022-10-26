using Amazon.XRay.Recorder.Handlers.AwsSdk;
using MailCheck.Common.Environment;
using MailCheck.Common.Logging;
using MailCheck.Common.Logging.Telemetry;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Messaging.CloudWatch.Config;
using MailCheck.Common.Messaging.CloudWatch.Processor;
using MailCheck.Common.Messaging.Common.Config;
using MailCheck.Common.Messaging.Common.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace MailCheck.Common.Messaging.CloudWatch.Factory
{
    internal class CloudWatchProcessorFactory
    {
        public static ICloudWatchProcessor Create(IStartUp startUp)
        {
            IServiceCollection collection = new ServiceCollection();

            startUp.ConfigureServices(collection);

            collection
                .AddTransient<ICloudWatchProcessor, CloudWatchProcessor>()
                .AddTransient<ILambdaJobProcessor, LambdaJobProcessor>()
                .AddTransient<ILambdaConfig, CloudWatchConfig>()
                .AddEnvironment()
                .AddSerilogLogging();

            new TelemetryConfig()
                .InstrumentAwsSdk()
                .InstrumentFlurlHttp();

            return collection.BuildServiceProvider()
                .GetRequiredService<ICloudWatchProcessor>();
        }
    }
}
