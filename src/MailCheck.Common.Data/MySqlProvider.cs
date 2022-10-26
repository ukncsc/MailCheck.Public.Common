using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MailCheck.Common.Data
{
    public class MySqlProvider : IProvider
    {
        public Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = MySqlClientFactory.Instance.CreateConnection();
            return Task.FromResult(connection);
        }
    }
}
