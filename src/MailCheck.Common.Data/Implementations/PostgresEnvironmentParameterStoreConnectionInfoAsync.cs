using Amazon.SimpleSystemsManagement;
using MailCheck.Common.Data.Util;
using MailCheck.Common.Environment.Abstractions;

namespace MailCheck.Common.Data.Implementations
{
    public class PostgresEnvironmentParameterStoreConnectionInfoAsync : BaseEnvironmentParameterStoreConnectionInfoAsync
    {
        public PostgresEnvironmentParameterStoreConnectionInfoAsync(
            IAmazonSimpleSystemsManagement parameterStoreClient,
            IEnvironmentVariables environmentVariables) 
            : base(
                parameterStoreClient, 
                environmentVariables,
                _ => PostgresConnectionStringHelpers.GetUserId(_, false),
                _ => PostgresConnectionStringHelpers.GetPassword(_, false)
                )
        {
        }
    }
}