using MailCheck.Common.Data.Migration.Preprocessing;
using NUnit.Framework;

namespace MailCheck.Common.Data.Migration.Test.Preprocessing
{
    [TestFixture]
    public class PostgresScriptPreprocessorTests
    {
        private PostgresScriptPreprocessor _postgresScriptPreprocessor;
        private IUsernameProcessor _usernameProcessor;
        private IPasswordProcessor _passwordProcessor;

        [SetUp]
        public void SetUp()
        {
            _usernameProcessor = new DummyUsernameProcessor();
            _passwordProcessor = new DummyPasswordProcessor();
            _postgresScriptPreprocessor = new PostgresScriptPreprocessor(_usernameProcessor, _passwordProcessor);
        }

        [TestCase("CREATE USER {env}_test WITH PASSWORD '{password}';", "CREATE USER test_test WITH PASSWORD 'password';", TestName = "Correctly substitutes {env} and {password} when creating user.")]
        [TestCase("GRANT CONNECT ON DATABASE test_db TO {env}_test;", "GRANT CONNECT ON DATABASE test_db TO test_test;", TestName = "Correctly substitutes {env} when executing grants.")]
        [TestCase("ALTER MATERIALIZED VIEW test_view OWNER TO {env}_test;", "ALTER MATERIALIZED VIEW test_view OWNER TO test_test;", TestName = "Correctly substitutes {env} changing ownership of materialized views.")]
        [TestCase("GRANT SELECT, UPDATE, INSERT, DELETE ON ip_address_to_asn TO {env}_test;\r\nGRANT SELECT, UPDATE, INSERT, DELETE ON ip_address_to_asn_mv TO {env}_test;", "GRANT SELECT, UPDATE, INSERT, DELETE ON ip_address_to_asn TO test_test;\r\nGRANT SELECT, UPDATE, INSERT, DELETE ON ip_address_to_asn_mv TO test_test;", TestName = "Test multiline substitutions work as expected.")]
        public void Test(string input, string expected)
        {
            string actual = _postgresScriptPreprocessor.Process(input);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
