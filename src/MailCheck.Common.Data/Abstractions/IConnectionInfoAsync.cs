using System.Threading.Tasks;

namespace MailCheck.Common.Data.Abstractions
{
    public interface IConnectionInfoAsync
    {
        Task<string> GetConnectionStringAsync();
    }
}