namespace MailCheck.Common.Data
{
    public interface ISqlBuilder
    {
        string Build(string tokenisedCommand);
        void SetToken(string key, string value);
    }
}