using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MailCheck.Common.Data
{
    public class SqlBuilder : ISqlBuilder
    {
        private static readonly Regex tokenRegex = new Regex("{(?'tokenKey'\\w+)}", RegexOptions.Compiled);
        private readonly Dictionary<string, string> _tokens = new Dictionary<string, string>();

        public void SetToken(string key, string value)
        {
            _tokens[key] = value;
        }

        public string Build(string tokenisedCommand)
        {
            return ReplaceTokens(tokenisedCommand, _tokens);
        }

        private string ReplaceTokens(string input, Dictionary<string, string> tokenValues)
        {
            return tokenRegex.Replace(input, match =>
            {
                var tokenKey = match.Groups["tokenKey"].Value;
                string tokenValue;
                if (tokenValues.TryGetValue(tokenKey, out tokenValue))
                {
                    return tokenValue;
                }
                throw new Exception($"Token with name {tokenKey} was not provided.");
            });
        }
    }
}