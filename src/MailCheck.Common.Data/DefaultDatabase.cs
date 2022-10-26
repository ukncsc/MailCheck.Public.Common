using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using MailCheck.Common.Data.Abstractions;

namespace MailCheck.Common.Data
{
    /// <summary>
    /// Entrypoint to the default database. The connection string is retrieved from <see cref="IConnectionInfoAsync"/>.
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    public class DefaultDatabase<TProvider> : IDatabase
        where TProvider : IProvider, new()
    {
        private static readonly TProvider Provider = new TProvider();

        private readonly IConnectionInfoAsync _connectionInfoAsync;

        public DefaultDatabase(IConnectionInfoAsync connectionInfoAsync)
        {
            _connectionInfoAsync = connectionInfoAsync;
        }

        public async Task<DbConnection> CreateAndOpenConnectionAsync(CancellationToken cancellationToken)
        {
            var connectionString = await _connectionInfoAsync.GetConnectionStringAsync();
            var connection = await Provider.CreateConnectionAsync(cancellationToken);
            connection.ConnectionString = connectionString;
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
