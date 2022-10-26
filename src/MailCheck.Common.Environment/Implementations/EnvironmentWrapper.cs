using MailCheck.Common.Environment.Abstractions;

namespace MailCheck.Common.Environment.Implementations
{
    public class EnvironmentWrapper : IEnvironment
    {
        public string GetEnvironmentVariable(string variableName)
        {
            return System.Environment.GetEnvironmentVariable(variableName);
        }
    }
}