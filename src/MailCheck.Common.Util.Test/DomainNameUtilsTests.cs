using System;
using NUnit.Framework;

namespace MailCheck.Common.Util.Test
{
    [TestFixture]
    public class DomainNameUtilsTests
    {
        [TestCase(" ncsc.gov.uk ")]
        [TestCase(".ncsc.gov.uk.")]
        [TestCase(" .ncsc.gov.uk. ")]
        [TestCase(" . ncsc.gov.uk . ")]
        public void FormatDomainShouldFormatDomainCorrectly(string domain)
        {
            string expectedDomain = "ncsc.gov.uk";

            string formattedDomain = DomainNameUtils.ToCanonicalDomainName(domain);

            Assert.AreEqual(expectedDomain, formattedDomain);
        }

        [TestCase("ncsc.gov.uk")]
        public void ReverseDomainShouldReverseDomainCorrectly(string domain)
        {
            string expectedReverseDomain = "uk.gov.ncsc";

            string reverseDomain = DomainNameUtils.ReverseDomainName(domain);

            Assert.AreEqual(expectedReverseDomain, reverseDomain);
        }

        [Test]
        public void FormatNullDomainShouldThrowError()
        {
            Assert.Throws<ArgumentNullException>(() => DomainNameUtils.ToCanonicalDomainName(null));
        }

        [Test]
        public void ReverseNullDomainShouldThrowError()
        {
            Assert.Throws<ArgumentNullException>(() => DomainNameUtils.ReverseDomainName(null));
        }
    }
}