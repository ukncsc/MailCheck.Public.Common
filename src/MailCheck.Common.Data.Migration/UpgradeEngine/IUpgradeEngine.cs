using System.Threading.Tasks;

namespace MailCheck.Common.Data.Migration.UpgradeEngine
{
    public interface IUpgradeEngine
    {
        Task<int> PerformUpgrade();
    }
}