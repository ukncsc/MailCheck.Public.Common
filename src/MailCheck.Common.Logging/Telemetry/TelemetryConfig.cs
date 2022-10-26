using Amazon.XRay.Recorder.Core.Strategies;
using Amazon.XRay.Recorder.Core;
using System;

namespace MailCheck.Common.Logging.Telemetry
{
    public class TelemetryConfig
    {
        public TelemetryConfig()
        {
            AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;
            bool.TryParse(Environment.GetEnvironmentVariable("EnableXray"), out bool enableXray);
            IsEnabled = enableXray;
        }

        public bool IsEnabled { get; }

        /// <summary>
        /// Conditionally apply instrumentation based on config
        /// </summary>
        /// <param name="applyInstrumentation"></param>
        /// <returns></returns>
        public TelemetryConfig ApplyInstrumentation(Action applyInstrumentation)
        {
            if (!IsEnabled) return this;

            applyInstrumentation();

            return this;
        }
    }
}
