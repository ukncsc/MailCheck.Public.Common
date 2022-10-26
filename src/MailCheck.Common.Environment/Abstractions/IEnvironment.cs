namespace MailCheck.Common.Environment.Abstractions
{
    public interface IEnvironment
    {
        string GetEnvironmentVariable(string variableName);
    }
}