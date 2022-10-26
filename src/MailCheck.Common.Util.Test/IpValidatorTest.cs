using NUnit.Framework;

namespace MailCheck.Common.Util.Test
{
    [TestFixture]
    public class IpValidatorTest
    {
        private IpValidator _ipValidator;

        [SetUp]
        public void SetUp()
        {
            _ipValidator = new IpValidator();
        }

        [TestCase("127.0.0.1", true, TestName = "IPv4 localhost (loopback) is valid")]
        [TestCase("192.168.0.1", true, TestName = "IPv4 local address is valid")]
        [TestCase(null, false, TestName = "Null is invalid")]
        [TestCase("", false, TestName = "Empty string is invalid")]
        [TestCase("192.256.0.1", false, TestName = "Octet greater than 255 is invalid")]
        [TestCase("2001:0db8:85a3:0000:0000:8a2e:0370:7334", true, TestName = "IPv6 address is valid")]
        [TestCase("2001:0db8:85a3:0:0:8a2e:0370:7334", true, TestName = "IPv6 address with omitted leading zeros is valid")]
        [TestCase("2001:0db8:85a3::8a2e:0370:7334", true, TestName = "IPv6 address with empty groups is valid")]
        [TestCase("0:0:0:0:0:0:0:1", true, TestName = "IPv6 localhost (loopback) is valid")]
        [TestCase("::1", true, TestName = "IPv6 condensed localhost (loopback) is valid")]
        [TestCase("::", true, TestName = "IPv6 unspecified address is valid")]
        [TestCase("0:0:0:0:0:0:0:0", true, TestName = "IPv6 condensed unspecified address is valid")]
        [TestCase("123.1-3.123.123", false, TestName = "hyphens in an element invalid")]
        [TestCase("123.123.1a3.123", false, TestName = "alphabet in IPv4 element invalid")]
        [TestCase("1.2.3", true, TestName = "Only three octets is valid")] // Limitation of IPAddress.TryParse, equates to 0.1.2.3
        [TestCase("123", true, TestName = "Single octet is valid")] // Limitation of IPAddress.TryParse, equates to 0.0.0.123
        [TestCase("1", true, TestName = "Single digit is valid")] // Limitation of IPAddress.TryParse, equates to 0.0.0.1
        public void TestValidIps(string ip, bool isValid)
        {
            Assert.That(_ipValidator.IsValidIp(ip), Is.EqualTo(isValid));
        }
    }
}