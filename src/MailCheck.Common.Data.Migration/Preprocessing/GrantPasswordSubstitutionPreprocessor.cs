using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;

namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public class GrantPasswordSubstitutionPreprocessor : IScriptPreprocessor
    {
        private readonly IUsernameProcessor _usernameProcessor;
        private readonly IPasswordProcessor _passwordProcessor;
        
        public GrantPasswordSubstitutionPreprocessor(
            IUsernameProcessor usernameProcessor,
            IPasswordProcessor passwordProcessor)
        {
            _usernameProcessor = usernameProcessor;
            _passwordProcessor = passwordProcessor;
        }

        public string Process(string contents)
        {
            List<string> commands = contents.Split(new []{ ";" }, StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToList();

            return string.Join($"{System.Environment.NewLine}{System.Environment.NewLine}",commands.Select(_ => $"{ProcessGrants(_)};"));
        }

        private string ProcessGrants(string command)
        {
            if (command.Trim().ToLower().StartsWith("grant"))
            {
                List<string> subCommands = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToList();

                string templatedUserName = subCommands.SingleOrDefault(_ => _.ToLower().Contains("{env}"));
                if (templatedUserName == null)
                {
                    throw new InvalidOperationException(
                        "Username in GRANT command must exist in the format '{env}-<username>'.");
                }

                string username = _usernameProcessor.ProcessUsername(templatedUserName);

                subCommands[subCommands.FindIndex(_ => _ == templatedUserName)] = username;

                string templatedPassword = subCommands.SingleOrDefault(_ => _.ToLower().Contains("{password}"));
                if (templatedPassword == null)
                {
                    throw new InvalidOperationException(
                        "Password in GRANT command must exist in the format '{password}'.");
                }

                subCommands[subCommands.FindIndex(_ => _ == templatedPassword)] = 
                    _passwordProcessor.ProcessPassword(templatedPassword, username);

                return string.Join(" ", subCommands);
            }

            return command;
        }
    }
}