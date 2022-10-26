using System;
using FakeItEasy;
using MailCheck.Common.Data.Migration.Preprocessing;
using NUnit.Framework;

namespace MailCheck.Common.Data.Migration.Test.Preprocessing
{
    [TestFixture]
    public class GrantPasswordSubstitutionPreprocessorTests
    {
        private GrantPasswordSubstitutionPreprocessor _grantPasswordSubstitutionPreprocessor;
        private IUsernameProcessor _usernameProcessor;
        private IPasswordProcessor _passwordProcessor;

        [SetUp]
        public void SetUp()
        {
            _usernameProcessor = A.Fake<IUsernameProcessor>();
            _passwordProcessor = A.Fake<IPasswordProcessor>();

            _grantPasswordSubstitutionPreprocessor = new GrantPasswordSubstitutionPreprocessor(_usernameProcessor, _passwordProcessor);
        }

        [Test]
        public void UserNameAndPasswordCorrectlySubstitutedForGrants()
        {
            string statement = "GRANT SELECT ON `table` TO \'{env}-test-user\' IDENTIFIED BY \'{password}\';";

            A.CallTo(() => _usernameProcessor.ProcessUsername(A<string>._)).Returns("\'dev-test-user\'");

            A.CallTo(() => _passwordProcessor.ProcessPassword(A<string>._, A<string>._)).Returns("\'password\'");

            string processedStatement = _grantPasswordSubstitutionPreprocessor.Process(statement);

            Assert.That(processedStatement, Is.EqualTo("GRANT SELECT ON `table` TO \'dev-test-user\' IDENTIFIED BY \'password\';"));
        }

        [Test]
        public void UserNameAndPasswordCorrectlySubstitutedForGrantsWhenMultipleStatements()
        {
            string statement = "CREATE TABLE IF NOT EXISTS `reverse_lookup_results` (\r\n`ip_address` VARCHAR(255) NOT NULL,\r\n`date` DATETIME NOT NULL,\r\n`data` TEXT,\r\nPRIMARY KEY (`ip_address`,`date`)\r\n) ENGINE = InnoDB;\r\n\r\nGRANT SELECT, INSERT, UPDATE ON `reverse_lookup_results` TO \'{env}-rdns-proc\' IDENTIFIED BY \'{password}\';\r\n\r\nGRANT SELECT ON `reverse_lookup_results` TO \'{env}-rdns-api\' IDENTIFIED BY \'{password}\';";

            A.CallTo(() => _usernameProcessor.ProcessUsername(A<string>._)).Returns("\'dev-test-user\'");

            A.CallTo(() => _passwordProcessor.ProcessPassword(A<string>._, A<string>._)).Returns("\'password\'");

            string processedStatement = _grantPasswordSubstitutionPreprocessor.Process(statement);

            Assert.That(processedStatement, Is.EqualTo("CREATE TABLE IF NOT EXISTS `reverse_lookup_results` (\r\n`ip_address` VARCHAR(255) NOT NULL,\r\n`date` DATETIME NOT NULL,\r\n`data` TEXT,\r\nPRIMARY KEY (`ip_address`,`date`)\r\n) ENGINE = InnoDB;\r\n\r\nGRANT SELECT, INSERT, UPDATE ON `reverse_lookup_results` TO \'dev-test-user\' IDENTIFIED BY \'password\';\r\n\r\nGRANT SELECT ON `reverse_lookup_results` TO \'dev-test-user\' IDENTIFIED BY \'password\';"));
        }

        [Test]
        public void NoChangesAppliedForNonGrants()
        {
            string statement =
                "CREATE TABLE IF NOT EXISTS `reverse_lookup_results` (\r\n`ip_address` VARCHAR(255) NOT NULL,\r\n`date` DATETIME NOT NULL,\r\n`data` TEXT,\r\nPRIMARY KEY (`ip_address`,`date`)\r\n) ENGINE = InnoDB;";

            A.CallTo(() => _usernameProcessor.ProcessUsername(A<string>._)).Returns("\'dev-test-user\'");

            A.CallTo(() => _passwordProcessor.ProcessPassword(A<string>._, A<string>._)).Returns("\'password\'");

            string processedStatement = _grantPasswordSubstitutionPreprocessor.Process(statement);

            Assert.That(processedStatement, Is.EqualTo(statement));
        }

        [Test]
        public void ThrowsIfEnvTemplateNotInGrantStatement()
        {
            string statement = "GRANT SELECT ON `table` TO \'{blah}-test-user\' IDENTIFIED BY \'{password}\';";

            A.CallTo(() => _usernameProcessor.ProcessUsername(A<string>._)).Returns("\'dev-test-user\'");

            A.CallTo(() => _passwordProcessor.ProcessPassword(A<string>._, A<string>._)).Returns("\'password\'");

            Assert.Throws<InvalidOperationException>(() => _grantPasswordSubstitutionPreprocessor.Process(statement));
        }

        [Test]
        public void ThrowsIfPasswordTemplateNotInGrantStatement()
        {
            string statement = "GRANT SELECT ON `table` TO \'{env}-test-user\' IDENTIFIED BY \'{blah}\';";

            A.CallTo(() => _usernameProcessor.ProcessUsername(A<string>._)).Returns("\'dev-test-user\'");

            A.CallTo(() => _passwordProcessor.ProcessPassword(A<string>._, A<string>._)).Returns("\'password\'");

            Assert.Throws<InvalidOperationException>(() => _grantPasswordSubstitutionPreprocessor.Process(statement));
        }
    }
}
