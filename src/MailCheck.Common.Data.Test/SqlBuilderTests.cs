using System;
using NUnit.Framework;

namespace MailCheck.Common.Data
{
    [TestFixture]
    public class SqlBuilderTests
    {
        [Test]
        public void EmptySqlBuilder_NoTokensInSql_ReturnsUnchangedSql()
        {
            var builder = new SqlBuilder();
            var tokenisedSql = "SELECT * FROM Table";
            var expected = "SELECT * FROM Table";
            var actual = builder.Build(tokenisedSql);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmptySqlBuilder_TokensInSql_ThrowsException()
        {
            var builder = new SqlBuilder();
            var tokenisedSql = "SELECT {Missing} FROM Table";
            var exception = Assert.Throws<Exception>(() => builder.Build(tokenisedSql));
            Assert.That(exception, Has.Message.EqualTo("Token with name Missing was not provided."));
        }

        [Test]
        public void SqlBuilder_TokensInSqlWithoutMatchingToken_ThrowsException()
        {
            var builder = new SqlBuilder();
            builder.SetToken("Unwanted", "NOPE");
            var tokenisedSql = "SELECT {Missing} FROM Table";
            var exception = Assert.Throws<Exception>(() => builder.Build(tokenisedSql));
            Assert.That(exception, Has.Message.EqualTo("Token with name Missing was not provided."));
        }

        [TestCase("SELECT {FieldName} FROM {TableName}", "SELECT Hello FROM World")]
        [TestCase("SELECT {TableName} FROM {FieldName}", "SELECT World FROM Hello")]
        public void SqlBuilder_TokensInSqlWithMatchingToken_ReplacedTokens(string tokenisedSql, string expected)
        {
            var builder = new SqlBuilder();
            builder.SetToken("FieldName", "Hello");
            builder.SetToken("TableName", "World");
            builder.SetToken("Unused", "NOPE");
            var actual = builder.Build(tokenisedSql);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
