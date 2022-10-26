using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using FakeItEasy;
using MailCheck.Common.Data.Implementations;
using MailCheck.Common.Environment.Abstractions;
using NUnit.Framework;

namespace MailCheck.Common.Data.Test.Implementations
{
    [TestFixture]
    public class PostgresEnvironmentParameterStoreConnectionInfoAsyncTests
    {
        private const string ConnectionString =
            "User ID=db_user; Host=localhost; Port=5432; Database=ip_intelligence; Pooling=true; Timeout=30; Command Timeout=1800;";

        private const string ConnectionStringWithPassword =
                "User ID=db_user; Host=localhost; Port=5432; Database=ip_intelligence; Pooling=true; Timeout=30; Command Timeout=1800; Pwd = db_pwd;";

        [Test]
        public async Task ConnectionStringContainsPasswordConnectionStringReturned()
        {
            IAmazonSimpleSystemsManagement amazonSimpleSystemsManagement = A.Fake<IAmazonSimpleSystemsManagement>();
            IEnvironmentVariables environmentVariables = A.Fake<IEnvironmentVariables>();

            A.CallTo(() => environmentVariables.Get(A<string>._, true)).Returns(ConnectionStringWithPassword);

            PostgresEnvironmentParameterStoreConnectionInfoAsync connectionInfoAsync 
                = new PostgresEnvironmentParameterStoreConnectionInfoAsync(amazonSimpleSystemsManagement, environmentVariables);

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

            PostgresEnvironmentParameterStoreConnectionInfoAsync connectionInfoAsync
                = new PostgresEnvironmentParameterStoreConnectionInfoAsync(amazonSimpleSystemsManagement, environmentVariables);

            string connectionString = await connectionInfoAsync.GetConnectionStringAsync();

            Assert.That(connectionString, Is.EqualTo(ConnectionStringWithPassword));

            A.CallTo(() => amazonSimpleSystemsManagement.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}
