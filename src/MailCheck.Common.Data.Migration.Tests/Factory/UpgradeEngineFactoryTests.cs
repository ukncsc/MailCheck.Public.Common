using MailCheck.Common.Data.Migration.Factory;
using MailCheck.Common.Data.Migration.UpgradeEngine;
using NUnit.Framework;

namespace MailCheck.Common.Data.Migration.Test.Factory
{
    [TestFixture]
    public class UpgradeEngineFactoryTests
    {
        [Test]
        public void CanSuccesfullyCreateMySqlUpgradeFactory()
        {
            System.Environment.SetEnvironmentVariable("ConnectionString", "ConnectionString");
            System.Environment.SetEnvironmentVariable("Environement", "dev");

            IUpgradeEngine upgradeEngine = UpgradeEngineFactory.Create(UpgradeEngineFactory.DatabaseType.MySql);

            Assert.That(upgradeEngine, Is.Not.Null);
        }

        [Test]
        public void CanSuccesfullyCreatePostgresqlUpgradeFactory()
        {
            System.Environment.SetEnvironmentVariable("ConnectionString", "ConnectionString");
            System.Environment.SetEnvironmentVariable("Environement", "dev");

            IUpgradeEngine upgradeEngine = UpgradeEngineFactory.Create(UpgradeEngineFactory.DatabaseType.PostgreSql);

            Assert.That(upgradeEngine, Is.Not.Null);
        }
    }
}
