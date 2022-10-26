using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MailCheck.Common.Data
{
    public interface IDatabase
    {
        Task<DbConnection> CreateAndOpenConnectionAsync(CancellationToken cancellationToken);
    }
}
