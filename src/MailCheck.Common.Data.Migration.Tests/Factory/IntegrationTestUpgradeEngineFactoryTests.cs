using System.Reflection;
using MailCheck.Common.Data.Migration.Factory;
using MailCheck.Common.Data.Migration.UpgradeEngine;
using NUnit.Framework;

namespace MailCheck.Common.Data.Migration.Test.Factory
{
    [TestFixture]
    public class IntegrationTestUpgradeEngineFactoryTests
    {
        [Test]
        public void CanSuccesfullyCreateUpgradeFactory()
        {
            IUpgradeEngine upgradeEngine = IntegrationTestUpgradeEngineFactory.Create("connection string", Assembly.GetEntryAssembly());

            Assert.That(upgradeEngine, Is.Not.Null);
        }
    }
}