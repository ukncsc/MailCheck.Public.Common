using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using DbUp.Engine.Output;
using FakeItEasy;
using MailCheck.Common.Data.Migration.Preprocessing;
using NUnit.Framework;

namespace MailCheck.Common.Data.Migration.Test.Preprocessing
{
    [TestFixture]
    public class ParameterStorePasswordProcessorTests
    {
        private ParameterStorePasswordProcessor _parameterStorePasswordProcessor;
        private IAmazonSimpleSystemsManagement _parameterStoreClient;
        private IPasswordGenerator _passwordGenerator;

        [SetUp]
        public void SetUp()
        {
            _parameterStoreClient = A.Fake<IAmazonSimpleSystemsManagement>();
            _passwordGenerator = A.Fake<IPasswordGenerator>();

            _parameterStorePasswordProcessor = new ParameterStorePasswordProcessor(_parameterStoreClient,
                _passwordGenerator, A.Fake<IUpgradeLog>());
        }

        [Test]
        public void PasswordExistsInParameterStorePasswordFromParameterStoreUsed()
        {
            A.CallTo(() => _parameterStoreClient.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .Returns(Task.FromResult(new GetParameterResponse {Parameter = new Parameter {Value = "password"}}));

            string password = _parameterStorePasswordProcessor.ProcessPassword("\'{password}\'", "'dev-test-user'");

            Assert.That(password, Is.EqualTo("\'password\'"));

            A.CallTo(() => _parameterStoreClient.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _passwordGenerator.GeneratePassword()).MustNotHaveHappened();

            A.CallTo(() => _parameterStoreClient.PutParameterAsync(A<PutParameterRequest>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }

        [Test]
        public void PasswordDoesntExistInParameterStorePasswordGeneratedAndAddedToParameterStore()
        {
            A.CallTo(() => _parameterStoreClient.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .Throws(_ => new ParameterNotFoundException(""));

            A.CallTo(() => _passwordGenerator.GeneratePassword()).Returns("password");

            A.CallTo(() => _parameterStoreClient.PutParameterAsync(A<PutParameterRequest>._, A<CancellationToken>._))
                .Returns(Task.FromResult(new PutParameterResponse {HttpStatusCode = HttpStatusCode.OK}));

            string password = _parameterStorePasswordProcessor.ProcessPassword("\'{password}\'", "'dev-test-user'");

            Assert.That(password, Is.EqualTo("\'password\'"));

            A.CallTo(() => _parameterStoreClient.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _passwordGenerator.GeneratePassword()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _parameterStoreClient.PutParameterAsync(A<PutParameterRequest>._, A<CancellationToken>._))
                .WhenArgumentsMatch(_ => (_[0] as PutParameterRequest)?.Value == "password")
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FailureToAddPasswordToParameterStoreThrowsException()
        {
            A.CallTo(() => _parameterStoreClient.GetParameterAsync(A<GetParameterRequest>._, A<CancellationToken>._))
                .Throws(_ => new ParameterNotFoundException(""));

            A.CallTo(() => _passwordGenerator.GeneratePassword()).Returns("password");

            A.CallTo(() => _parameterStoreClient.PutParameterAsync(A<PutParameterRequest>._, A<CancellationToken>._))
                .Returns(Task.FromResult(new PutParameterResponse { HttpStatusCode = HttpStatusCode.BadRequest }));

            Assert.Throws<Exception>(() => _parameterStorePasswordProcessor.ProcessPassword("\'{password}\'", "'dev-test-user'"));
        }
    }
}
