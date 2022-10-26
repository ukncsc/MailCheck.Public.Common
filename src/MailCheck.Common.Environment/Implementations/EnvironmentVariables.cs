using System;
using System.Net;
using MailCheck.Common.Environment.Abstractions;

namespace MailCheck.Common.Environment.Implementations
{
    public class EnvironmentVariables : IEnvironmentVariables
    {
        private readonly IEnvironment _environment;

        public EnvironmentVariables(IEnvironment environment)
        {
            _environment = environment;
        }

        public string Get(string variableName, bool throwIfNotFound = true)
        {
            string variable = _environment.GetEnvironmentVariable(variableName);

            if (!throwIfNotFound || !string.IsNullOrWhiteSpace(variable))
            {
                return variable;
            }
            
            throw new ArgumentException($"No environment variable exists with the name { variableName }");
        }

        public bool GetAsBoolOrDefault(string variableName, bool defaultValue = false)
        {
            string variable = Get(variableName, false);
            return bool.TryParse(variable, out var value) ? value : defaultValue;
        }

        public int GetAsInt(string variableName)
        {
            string variable = Get(variableName);
            if (int.TryParse(variable, out var value))
            {
                return value;
            }
            throw new ArgumentException($"{variableName} with value {variable} is not a valid int.");
        }

        public int GetAsIntOrDefault(string variableName, int fallback)
        {
            string variable = Get(variableName, false);
            return int.TryParse(variable, out var value) ? value : fallback;
        }

        public double GetAsDouble(string variableName)
        {
            string variable = Get(variableName);
            if (double.TryParse(variable, out var value))
            {
                return value;
            }
            throw new ArgumentException($"{variableName} with value {variable} is not a valid double.");
        }

        public long GetAsLong(string variableName)
        {
            string variable = Get(variableName);
            if (long.TryParse(variable, out var value))
            {
                return value;
            }
            throw new ArgumentException($"{variableName} with value {variable} is not a valid long.");
        }

        public IPAddress GetAsIpAddress(string variableName)
        {
            string variable = Get(variableName);
            if (IPAddress.TryParse(variable, out var address))
            {
                return address;
            }
            throw new ArgumentException($"{variableName} with value {variable} is not a valid ip address.");
        }
    }
}
