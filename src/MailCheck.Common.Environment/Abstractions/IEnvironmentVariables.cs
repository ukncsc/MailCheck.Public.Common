using System.Net;

namespace MailCheck.Common.Environment.Abstractions
{
    public interface IEnvironmentVariables
    {
        string Get(string variableName, bool throwIfNotFound = true);
        bool GetAsBoolOrDefault(string variableName, bool defaultValue = false);
        int GetAsInt(string variableName);
        int GetAsIntOrDefault(string variableName, int defaultValue);
        double GetAsDouble(string variableName);
        long GetAsLong(string variableName);
        IPAddress GetAsIpAddress(string variableName);
    }
}