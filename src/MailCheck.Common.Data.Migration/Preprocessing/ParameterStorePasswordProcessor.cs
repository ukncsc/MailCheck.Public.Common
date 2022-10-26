using System;
using System.Net;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using DbUp.Engine.Output;

namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public class ParameterStorePasswordProcessor : IPasswordProcessor
    {
        private readonly IAmazonSimpleSystemsManagement _parameterStoreClient;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IUpgradeLog _log;

        public ParameterStorePasswordProcessor(IAmazonSimpleSystemsManagement parameterStoreClient,
            IPasswordGenerator passwordGenerator,
            IUpgradeLog log)
        {
            _parameterStoreClient = parameterStoreClient;
            _passwordGenerator = passwordGenerator;
            _log = log;
        }

        public string ProcessPassword(string templatedPassword, string username)
        {
            string rawUsername = username.Replace("'", string.Empty);
            string password = null;

            try
            {
                GetParameterResponse getParameterResponse = _parameterStoreClient
                    .GetParameterAsync(new GetParameterRequest { Name = rawUsername, WithDecryption = true }).GetAwaiter().GetResult();

                password = getParameterResponse.Parameter.Value;
            }
            catch (ParameterNotFoundException)
            {
                _log.WriteInformation("Didnt find password in parameter store for {UserName}", username);
            }

            if (string.IsNullOrEmpty(password))
            {
                string newPassword = _passwordGenerator.GeneratePassword();

                _log.WriteInformation("Creating password in parameter store for {UserName}", username);
                PutParameterResponse setParameterResponse = _parameterStoreClient
                    .PutParameterAsync(new PutParameterRequest { Name = rawUsername, Value = newPassword, Type = ParameterType.SecureString }).GetAwaiter().GetResult();

                if (setParameterResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(
                        $"Unexpected response from parameter store when creating password for {username}.");
                }

                _log.WriteInformation("Created password in parameter store for {UserName}", username);

                password = newPassword;
            }

            return templatedPassword.Replace("{password}", password);
        }
    }
}