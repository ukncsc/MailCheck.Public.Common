using System.Net.Http;
using Amazon.XRay.Recorder.Handlers.System.Net;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace MailCheck.Common.Logging.Telemetry
{
    public static class FlurlHttpInstrumentation
    {
        public static TelemetryConfig InstrumentFlurlHttp(this TelemetryConfig telemetry)
        {
            return telemetry.ApplyInstrumentation(() =>
            {
                FlurlHttp.Configure(settings =>
                {
                    settings.HttpClientFactory = new DefaultHttpClientFactoryWithTracing();
                });
            });
        }
    }

    class DefaultHttpClientFactoryWithTracing : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            var innerHandler = base.CreateMessageHandler();
            return new HttpClientXRayTracingHandler(innerHandler);
        }
    }
}
