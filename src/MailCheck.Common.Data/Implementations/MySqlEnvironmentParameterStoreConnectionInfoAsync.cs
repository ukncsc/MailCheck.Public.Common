using Amazon.SimpleSystemsManagement;
using MailCheck.Common.Data.Util;
using MailCheck.Common.Environment.Abstractions;

namespace MailCheck.Common.Data.Implementations
{
    public class MySqlEnvironmentParameterStoreConnectionInfoAsync : BaseEnvironmentParameterStoreConnectionInfoAsync
    {
        public MySqlEnvironmentParameterStoreConnectionInfoAsync(
            IAmazonSimpleSystemsManagement parameterStoreClient,
            IEnvironmentVariables environmentVariables)
            : base(
                parameterStoreClient,
                environmentVariables,
                _ => MySqlConnectionStringHelpers.GetUserId(_, false),
                _ => MySqlConnectionStringHelpers.GetPassword(_, false)
            )
        {
        }
    }
}