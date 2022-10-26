namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public interface IPasswordProcessor
    {
        string ProcessPassword(string templatedPassword, string username);
    }
}