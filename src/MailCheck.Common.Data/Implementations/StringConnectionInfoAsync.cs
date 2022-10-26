using System.Threading.Tasks;
using MailCheck.Common.Data.Abstractions;

namespace MailCheck.Common.Data.Implementations
{
    public class StringConnectionInfoAsync : IConnectionInfoAsync
    {
        private readonly string _connectionString;

        public StringConnectionInfoAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<string> GetConnectionStringAsync()
        {
            return Task.FromResult(_connectionString);
        }
    }
}