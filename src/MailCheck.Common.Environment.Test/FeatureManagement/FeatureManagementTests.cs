using NUnit.Framework;

namespace MailCheck.Common.Environment.Test.FeatureManagement
{
    [TestFixture]
    public class FeatureManagementTests
    {
        [TestCase("abc")]
        [TestCase("abc,def")]
        [TestCase("abc, def")]
        [TestCase("abc , def")]
        [TestCase(" abc , def")]
        public void IsEnabledIsTrueWhenSet(string activeFeatures)
        {
            System.Environment.SetEnvironmentVariable("ACTIVE_FEATURES", activeFeatures);
            Assert.True(Environment.FeatureManagement.FeatureManagement.IsEnabled("abc"));
        }

        [Test]
        public void IsEnabledIsFalseWhenNotSet()
        {
            Assert.False(Environment.FeatureManagement.FeatureManagement.IsEnabled("abc"));
        }

        [TearDown]
        public void TearDown()
        {
            System.Environment.SetEnvironmentVariable("ACTIVE_FEATURES", null);
        }
    }
}
