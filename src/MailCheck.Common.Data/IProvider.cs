using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MailCheck.Common.Data
{
    /// <summary>
    /// Super minimal database provider entrypoint. Just a factory for a <see cref="DbConnection"/>.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Creates a plain <see cref="DbConnection"/> - no connection string assigned and not opened.
        /// </summary>
        /// <returns></returns>
        Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
    }
}
