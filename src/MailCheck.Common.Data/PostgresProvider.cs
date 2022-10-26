using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MailCheck.Common.Data
{
    public class PostgresProvider : IProvider
    {
        public Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = Npgsql.NpgsqlFactory.Instance.CreateConnection();
            return Task.FromResult(connection);
        }
    }
}
