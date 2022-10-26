using NUnit.Framework;

namespace MailCheck.Common.Data
{
    [TestFixture]
    public class FieldsetExtensionsTests
    {
        [TestCase(new string[] { }, "")]
        [TestCase(new[] { "One" }, "One")]
        [TestCase(new[] { "One", "Two" }, "One, Two")]
        [TestCase(new[] { "One", "Two", "Three" }, "One, Two, Three")]
        public void ToFieldListSql_WithFields_RightStuff(string[] fields, string expected)
        {
            var fieldset = new Fieldset(fields);
            Assert.That(fieldset.ToFieldListSql(), Is.EqualTo(expected));
        }

        [TestCase(new string[] { }, 2, "")]
        [TestCase(new string[] { "one" }, 0, "")]
        [TestCase(new[] { "one" }, 1, "( @one_0 )")]
        [TestCase(new[] { "one" }, 2, "( @one_0 ), ( @one_1 )")]
        [TestCase(new[] { "one", "two" }, 1, "( @one_0, @two_0 )")]
        [TestCase(new[] { "one", "two" }, 2, "( @one_0, @two_0 ), ( @one_1, @two_1 )")]
        public void ToValuesParameterListSql_WithFields_RightStuff(string[] fields, int numRecords, string expected)
        {
            var fieldset = new Fieldset(fields);
            Assert.That(fieldset.ToValuesParameterListSql(numRecords), Is.EqualTo(expected));
        }

        [TestCase(new string[] { }, "")]
        [TestCase(new string[] { "one" }, "one as col0")]
        [TestCase(new[] { "one", "two" }, "one as col0, two as col1")]
        public void ToAliasedFieldListSql_Fields_RightStuff(string[] fields, string expected)
        {
            var fieldset = new Fieldset(fields);
            Assert.That(fieldset.ToAliasedFieldListSql(), Is.EqualTo(expected));
        }
    }
}
