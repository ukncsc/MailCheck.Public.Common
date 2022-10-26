using System;
using MailCheck.Common.Data.Util;
using NUnit.Framework;

namespace MailCheck.Common.Data.Test.Util
{
    [TestFixture]
    public class PostgresConnectionStringHelpersTests
    {
        private const string ValidConnectionString = "User ID=db_user; Host=localhost; Port=5432; Database=db_name; Pooling=true; Timeout=30; Command Timeout=1800; Pwd = db_pwd;";
        private const string ConnectionStringMissingValues = "Host=localhost; Port=5432; Timeout=30;";
        private const string MalformedConnectionString = "fd;lgfdkglfd;gfdkg;lfdg";

        [Test]
        public void GetDbNameCorrectlyReturnsDbName()
        {
            string value = PostgresConnectionStringHelpers.GetDatabaseName(ValidConnectionString);
            Assert.That(value, Is.EqualTo("db_name"));
        }

        [Test]
        public void GetDbNameFromConnectionStringMissingValueThrows()
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionStringHelpers.GetDatabaseName(ConnectionStringMissingValues));
        }

        [Test]
        public void GetDbNameFromConnectionStringMissingReturnsNullIfThrowingDisabled()
        {
            string value = PostgresConnectionStringHelpers.GetDatabaseName(ConnectionStringMissingValues, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetDbNameWithMalformedStringThrows()
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionStringHelpers.GetDatabaseName(MalformedConnectionString));
        }

        [Test]
        public void GetDbNameWithMalformedStringReturnNullIfThrowingDisabled()
        {
            string value = PostgresConnectionStringHelpers.GetDatabaseName(MalformedConnectionString, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetDbPasswordCorrectlyReturnsDbPassword()
        {
            string value = PostgresConnectionStringHelpers.GetPassword(ValidConnectionString);
            Assert.That(value, Is.EqualTo("db_pwd"));
        }

        [Test]
        public void GetDbPasswordFromConnectionStringMissingValueThrows()
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionStringHelpers.GetPassword(ConnectionStringMissingValues));
        }

        [Test]
        public void GetDbPasswordFromConnectionStringMissingReturnsNullIfThrowingDisabled()
        {
            string value = PostgresConnectionStringHelpers.GetPassword(ConnectionStringMissingValues, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetDbPasswordWithMalformedStringThrows()
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionStringHelpers.GetPassword(MalformedConnectionString));
        }

        [Test]
        public void GetDbPasswordWithMalformedStringReturnNullIfThrowingDisabled()
        {
            string value = PostgresConnectionStringHelpers.GetPassword(MalformedConnectionString, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetUserIdCorrectlyReturnsUserId()
        {
            string value = PostgresConnectionStringHelpers.GetUserId(ValidConnectionString);
            Assert.That(value, Is.EqualTo("db_user"));
        }

        [Test]
        public void GetUserIdFromConnectionStringMissingValueThrows()
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionStringHelpers.GetUserId(ConnectionStringMissingValues));
        }

        [Test]
        public void GetUserIdFromConnectionStringMissingReturnsNullIfThrowingDisabled()
        {
            string value = PostgresConnectionStringHelpers.GetUserId(ConnectionStringMissingValues, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetUserIdWithMalformedStringThrows()
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionStringHelpers.GetUserId(MalformedConnectionString));
        }

        [Test]
        public void GetUserIdWithMalformedStringReturnNullIfThrowingDisabled()
        {
            string value = PostgresConnectionStringHelpers.GetUserId(MalformedConnectionString, false);
            Assert.That(value, Is.Null);
        }
    }
}
