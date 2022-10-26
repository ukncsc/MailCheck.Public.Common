using MailCheck.Common.Environment;
using MailCheck.Common.Environment.Abstractions;

namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public class EnvironmentUsernameProcessor : IUsernameProcessor
    {
        private readonly IEnvironmentVariables _environmentVariables;

        public EnvironmentUsernameProcessor(IEnvironmentVariables environmentVariables)
        {
            _environmentVariables = environmentVariables;
        }

        public string ProcessUsername(string templatedUsername)
        {
            string environment = _environmentVariables.Get("Environment").ToLower();

            return templatedUsername.Replace("{env}", environment);
        }
    }
}