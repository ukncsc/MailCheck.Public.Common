namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public interface IUsernameProcessor
    {
        string ProcessUsername(string templatedUsername);
    }
}