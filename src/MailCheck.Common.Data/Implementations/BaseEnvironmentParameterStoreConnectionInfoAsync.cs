using System;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using MailCheck.Common.Data.Abstractions;
using MailCheck.Common.Environment.Abstractions;

namespace MailCheck.Common.Data.Implementations
{
    public abstract class BaseEnvironmentParameterStoreConnectionInfoAsync : IConnectionInfoAsync
    {
        private readonly IAmazonSimpleSystemsManagement _parameterStoreClient;
        private readonly Func<string, string> _usernameGetter;
        private readonly Func<string, string> _passwordGetter;
        private string _connectionString;

        protected BaseEnvironmentParameterStoreConnectionInfoAsync(
            IAmazonSimpleSystemsManagement parameterStoreClient,
            IEnvironmentVariables environmentVariables,
            Func<string, string> usernameGetter, 
            Func<string, string> passwordGetter)
        {
            _parameterStoreClient = parameterStoreClient;
            _usernameGetter = usernameGetter;
            _passwordGetter = passwordGetter;

            _connectionString = environmentVariables.Get("ConnectionString");
        }

        public async Task<string> GetConnectionStringAsync()
        {
            if (ShouldAppendString(_connectionString))
            {
                string username = _usernameGetter(_connectionString);

                GetParameterResponse response = await _parameterStoreClient
                    .GetParameterAsync(new GetParameterRequest { Name = username, WithDecryption = true });

                string password = response.Parameter.Value;

                if (_connectionString.TrimEnd().EndsWith(";"))
                {
                    _connectionString = $"{_connectionString} Pwd = {password};";
                }
                else
                {
                    _connectionString = $"{_connectionString}; Pwd = {password};";
                }
                
            }

            return _connectionString;
        }

        private bool ShouldAppendString(string connectionString)
        {
            return string.IsNullOrWhiteSpace(_passwordGetter(connectionString));
        }
    }
}