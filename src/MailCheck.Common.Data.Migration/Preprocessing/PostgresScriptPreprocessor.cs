using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DbUp.Engine;

namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public class PostgresScriptPreprocessor : IScriptPreprocessor
    {
        private readonly IUsernameProcessor _usernameProcessor;
        private readonly IPasswordProcessor _passwordProcessor;
        private const string StatementSeparator = ";";
        private const string TokenSeparator = " ";
        private const string UserIdentifier = "{env}";
        private const string PasswordIdentifier = "{password}";

        public PostgresScriptPreprocessor(
            IUsernameProcessor usernameProcessor,
            IPasswordProcessor passwordProcessor)
        {
            _usernameProcessor = usernameProcessor;
            _passwordProcessor = passwordProcessor;
        }

        public string Process(string contents)
        {
            string[] statements = Regex.Split(contents, $@"(?<=[{StatementSeparator}])").Where(_ => !string.IsNullOrWhiteSpace(_)).ToArray();

            List<string> processedStatements = statements.Select(ProcessStatement).ToList();

            return string.Join(string.Empty, processedStatements);
        }

        private string ProcessStatement(string statement)
        {
            string[] tokens = statement.Split(new[] { TokenSeparator }, StringSplitOptions.RemoveEmptyEntries);
            string userName = string.Empty;

            List<string> processedToken = tokens.Select((v, i) =>
            {
                if (v.Contains(UserIdentifier))
                {
                    userName = _usernameProcessor.ProcessUsername(v);
                    return userName;
                }

                if (v.Contains(PasswordIdentifier))
                {
                    return _passwordProcessor.ProcessPassword(v, userName);
                }

                return v;
            }).ToList();

            return string.Join(TokenSeparator, processedToken);
        }
    }
}
