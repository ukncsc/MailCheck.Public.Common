using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MailCheck.Common.Data
{
    public static class IDatabaseExtensions
    {
        public static Task<DbConnection> CreateAndOpenConnectionAsync(this IDatabase database)
        {
            return database.CreateAndOpenConnectionAsync(CancellationToken.None);
        }
    }
}
