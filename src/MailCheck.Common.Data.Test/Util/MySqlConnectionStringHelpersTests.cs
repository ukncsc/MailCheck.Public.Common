using System;
using MailCheck.Common.Data.Util;
using NUnit.Framework;

namespace MailCheck.Common.Data.Test.Util
{
    [TestFixture]
    public class MySqlConnectionStringHelpersTests
    {
        private const string ValidConnectionString = "Server = localhost; Port = 3306; Database = db_name; Pwd = db_pwd; Uid = db_user; Connection Timeout=5;";
        private const string ConnectionStringMissingValues = "Server = localhost; Port = 3306; Connection Timeout=5;";
        private const string MalformedConnectionString = "fd;lgfdkglfd;gfdkg;lfdg";

        [Test]
        public void GetDbNameCorrectlyReturnsDbName()
        {
            string value = MySqlConnectionStringHelpers.GetDatabaseName(ValidConnectionString);
            Assert.That(value, Is.EqualTo("db_name"));
        }

        [Test]
        public void GetDbNameFromConnectionStringMissingValueThrows()
        {
            Assert.Throws<InvalidOperationException>(() => MySqlConnectionStringHelpers.GetDatabaseName(ConnectionStringMissingValues));
        }

        [Test]
        public void GetDbNameFromConnectionStringMissingReturnsNullIfThrowingDisabled()
        {
            string value = MySqlConnectionStringHelpers.GetDatabaseName(ConnectionStringMissingValues, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetDbNameWithMalformedStringThrows()
        {
            Assert.Throws<InvalidOperationException>(() => MySqlConnectionStringHelpers.GetDatabaseName(MalformedConnectionString));
        }

        [Test]
        public void GetDbNameWithMalformedStringReturnNullIfThrowingDisabled()
        {
            string value = MySqlConnectionStringHelpers.GetDatabaseName(MalformedConnectionString, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetDbPasswordCorrectlyReturnsDbPassword()
        {
            string value = MySqlConnectionStringHelpers.GetPassword(ValidConnectionString);
            Assert.That(value, Is.EqualTo("db_pwd"));
        }

        [Test]
        public void GetDbPasswordFromConnectionStringMissingValueThrows()
        {
            Assert.Throws<InvalidOperationException>(() => MySqlConnectionStringHelpers.GetPassword(ConnectionStringMissingValues));
        }

        [Test]
        public void GetDbPasswordFromConnectionStringMissingReturnsNullIfThrowingDisabled()
        {
            string value = MySqlConnectionStringHelpers.GetPassword(ConnectionStringMissingValues, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetDbPasswordWithMalformedStringThrows()
        {
            Assert.Throws<InvalidOperationException>(() => MySqlConnectionStringHelpers.GetPassword(MalformedConnectionString));
        }

        [Test]
        public void GetDbPasswordWithMalformedStringReturnNullIfThrowingDisabled()
        {
            string value = MySqlConnectionStringHelpers.GetPassword(MalformedConnectionString, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetUserIdCorrectlyReturnsUserId()
        {
            string value = MySqlConnectionStringHelpers.GetUserId(ValidConnectionString);
            Assert.That(value, Is.EqualTo("db_user"));
        }

        [Test]
        public void GetUserIdFromConnectionStringMissingValueThrows()
        {
            Assert.Throws<InvalidOperationException>(() => MySqlConnectionStringHelpers.GetUserId(ConnectionStringMissingValues));
        }

        [Test]
        public void GetUserIdFromConnectionStringMissingReturnsNullIfThrowingDisabled()
        {
            string value = MySqlConnectionStringHelpers.GetUserId(ConnectionStringMissingValues, false);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void GetUserIdWithMalformedStringThrows()
        {
            Assert.Throws<InvalidOperationException>(() => MySqlConnectionStringHelpers.GetUserId(MalformedConnectionString));
        }

        [Test]
        public void GetUserIdWithMalformedStringReturnNullIfThrowingDisabled()
        {
            string value = MySqlConnectionStringHelpers.GetUserId(MalformedConnectionString, false);
            Assert.That(value, Is.Null);
        }
    }
}
