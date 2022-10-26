using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;

namespace MailCheck.Common.Data.Migration.Logging
{
    public class LogAdaptor : IUpgradeLog
    {
        private readonly ILogger _log;

        public LogAdaptor(ILogger<LogAdaptor> log)
        {
            _log = log;
        }

        public void WriteInformation(string format, params object[] args)
        {
            _log.LogInformation(format, args);
        }

        public void WriteError(string format, params object[] args)
        {
            _log.LogError(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _log.LogWarning(format, args);
        }
    }
}