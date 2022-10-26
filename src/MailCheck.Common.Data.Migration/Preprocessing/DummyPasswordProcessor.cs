namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public class DummyPasswordProcessor : IPasswordProcessor
    {
        public string ProcessPassword(string templatedPassword, string username)
        {
            return templatedPassword.Replace("{password}", "password");
        }
    }
}