using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using FakeItEasy;
using MailCheck.Common.Data.Implementations;
using MailCheck.Common.Environment;
using MailCheck.Common.Environment.Abstractions;
using NUnit.Framework;

namespace MailCheck.Common.Data.Test.Implementations
{
    [TestFixture]
    public class MySqlEnvironmentParameterStoreConnectionInfoAsyncTests
    {
        private const string ConnectionString =
            "Server = localhost; Port = 3306; Database = dmarc; Uid = db_user; Connection Timeout=5;";

        private const string ConnectionStringWithPassword =
            "Server = localhost; Port = 3306; Database = dmarc; Uid = db_user; Connection Timeout=5; Pwd = db_pwd;";

        [Test]
        public async Task ConnectionStringContainsPasswordConnectionStringReturned()
        {
            IAmazonSimpleSystemsManagement amazonSimpleSystemsManagement = A.Fake<IAmazonSimpleSystemsManagement>();
            IEnvironmentVariables environmentVariables = A.Fake<IEnvironmentVariables>();

            A.CallTo(() => environmentVariables.Get(A<string>._, true)).Returns(ConnectionStringWithPassword);

            MySqlEnvironmentParameterStoreConnectionInfoAsync connectionInfoAsync 
                = new MySqlEnvironmentParameterStoreConnectionInfoAsync(amazonSimpleSystemsManagement, environmentVariables);

            string connectionString = await connectionInfoAsync.GetConnectionStringAsync();

            Assert.That(connectionString, Is.EqualTo(ConnectionStringWithPassword));

            A.CallTo(() => amazonSimpleSystemsManagement.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }

        [Test]
        public async Task ConnectionStringDoesntContainsPasswordConnectionStringReturned()
        {
            IAmazonSimpleSystemsManagement amazonSimpleSystemsManagement = A.Fake<IAmazonSimpleSystemsManagement>();
            IEnvironmentVariables environmentVariables = A.Fake<IEnvironmentVariables>();

            A.CallTo(() => environmentVariables.Get(A<string>._, A<bool>._)).Returns(ConnectionString);

            A.CallTo(() => amazonSimpleSystemsManagement.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .Returns(new GetParameterResponse{Parameter = new Parameter{Value = "db_pwd"}});

            MySqlEnvironmentParameterStoreConnectionInfoAsync connectionInfoAsync
                = new MySqlEnvironmentParameterStoreConnectionInfoAsync(amazonSimpleSystemsManagement, environmentVariables);

            string connectionString = await connectionInfoAsync.GetConnectionStringAsync();

            Assert.That(connectionString, Is.EqualTo(ConnectionStringWithPassword));

            A.CallTo(() => amazonSimpleSystemsManagement.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}
