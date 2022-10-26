using System;
using NUnit.Framework;

namespace MailCheck.Common.Util.Test
{
    [TestFixture]
    public class StringEncodingExtensionMethodTests
    {
        [TestCase(0, "\\000", Description = "null char correctly encoded")]
        [TestCase(1, "\\001", Description = "start of heading char correctly encoded")]
        [TestCase(2, "\\002", Description = "start of text char correctly encoded")]
        [TestCase(3, "\\003", Description = "end of text char correctly encoded")]
        [TestCase(4, "\\004", Description = "end of transmission char correctly encoded")]
        [TestCase(5, "\\005", Description = "enquiry char correctly encoded")]
        [TestCase(6, "\\006", Description = "acknowledgement char correctly encoded")]
        [TestCase(7, "\\007", Description = "bell char correctly encoded")]
        [TestCase(8, "\\008", Description = "backspace char correctly encoded")]
        [TestCase(9, "\\009", Description = "horizontal tab char correctly encoded")]
        [TestCase(10, "\\010", Description = "new line char correctly encoded")]
        [TestCase(11, "\\011", Description = "vertical tab char correctly encoded")]
        [TestCase(12, "\\012", Description = "form feed char correctly encoded")]
        [TestCase(13, "\\013", Description = "carriage return char correctly encoded")]
        [TestCase(14, "\\014", Description = "shift out char correctly encoded")]
        [TestCase(15, "\\015", Description = "shift in char correctly encoded")]
        [TestCase(16, "\\016", Description = "data link escape char correctly encoded")]
        [TestCase(17, "\\017", Description = "device control 1 char correctly encoded")]
        [TestCase(18, "\\018", Description = "device control 2 char correctly encoded")]
        [TestCase(19, "\\019", Description = "device control 3 char correctly encoded")]
        [TestCase(20, "\\020", Description = "device control 4 char correctly encoded")]
        [TestCase(21, "\\021", Description = "negative acknowledgement char correctly encoded")]
        [TestCase(22, "\\022", Description = "synchronous idle char correctly encoded")]
        [TestCase(23, "\\023", Description = "end of transmission char correctly encoded")]
        [TestCase(24, "\\024", Description = "cancel char correctly encoded")]
        [TestCase(25, "\\025", Description = "end of medium char correctly encoded")]
        [TestCase(26, "\\026", Description = "substitute char correctly encoded")]
        [TestCase(27, "\\027", Description = "escape char correctly encoded")]
        [TestCase(28, "\\028", Description = "file separator char correctly encoded")]
        [TestCase(29, "\\029", Description = "group separator char correctly encoded")]
        [TestCase(30, "\\030", Description = "record separator char correctly encoded")]
        [TestCase(31, "\\031", Description = "unit separator char correctly encoded")]
        [TestCase(127, "\\127", Description = "del char correctly encoded")]
        [TestCase(128, "\\128", Description = "extended control chars (start) correctly encoded")]
        [TestCase(159, "\\159", Description = "extended control chars (end) correctly encoded")]
        public void TestNonPrintablesAsciiAndExtendedAscii(int charValue, string result)
        {
            string value = Convert.ToChar(charValue).ToString();

            string encodedValue = value.Escape();

            Assert.That(encodedValue, Is.EqualTo(result));
        }

        [TestCase(256, "\\u256", Description = "unicode chars correctly encoded")]
        public void TestUnicodeChars(int charValue, string result)
        {
            string value = Convert.ToChar(charValue).ToString();

            string encodedValue = value.Escape();

            Assert.That(encodedValue, Is.EqualTo(result));
        }

        [Test]
        public void PrintablesAsciiAndExtendedAsciiCorrectlyEncoded()
        {
            string value = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ";

            string encodedValue = value.Escape();

            Assert.That(encodedValue, Is.EqualTo(value));
        }
    }
}
