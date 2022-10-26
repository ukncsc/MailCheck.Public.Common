using Amazon.XRay.Recorder.Handlers.AwsSdk;

namespace MailCheck.Common.Logging.Telemetry
{
    public static class AwsSdkInstrumentation
    {
        public static TelemetryConfig InstrumentAwsSdk(this TelemetryConfig telemetry)
        {
            telemetry.ApplyInstrumentation(() =>
            {
                AWSSDKHandler.RegisterXRayForAllServices();
            });
            return telemetry;
        }
    }
}
