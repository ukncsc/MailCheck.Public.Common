namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public class DummyUsernameProcessor : IUsernameProcessor
    {
        public string ProcessUsername(string templatedUsername)
        {
            return templatedUsername.Replace("{env}", "test");
        }
    }
}