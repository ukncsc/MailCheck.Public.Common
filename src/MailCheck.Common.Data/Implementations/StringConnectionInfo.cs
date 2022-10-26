using MailCheck.Common.Data.Abstractions;

namespace MailCheck.Common.Data.Implementations
{
    public class StringConnectionInfo : IConnectionInfo
    {
        public StringConnectionInfo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}