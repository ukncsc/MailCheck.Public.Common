using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MailCheck.Common.Util.Test
{
    [TestFixture]
    public class AuditTrailParserTest
    {
        private AuditTrailParser _auditTrailParser;

        [SetUp]
        public void SetUp()
        {
            _auditTrailParser = new AuditTrailParser();
        }

        [Test]
        public void TestAuditTrailParsingAllErrors()
        {
            string auditTrail =
@"; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 36238
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1

;; ERROR: Non-Existent Domain
;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 512; code: NoError
;; QUESTION SECTION:
test1.test.gov.uk.IN TXT

;; AUTHORITIES SECTION:
test.gov.uk. 000 IN SOA ns - 0000.awsdns - 00.co.uk.awsdns - hostmaster.amazon.com. 0 0000 900 0000000 00000

;; Query time: 23 msec
;; SERVER: 8.8.8.8#53
;; WHEN: Wed Aug 25 18:33:22 Z 2021
;; MSG SIZE rcvd: 137

; Trying next server.
; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 21977
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1

;; ERROR: Non-Existent Domain
;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 4096; code: NoError
;; QUESTION SECTION:
test1.test.gov.uk.IN TXT

;; AUTHORITIES SECTION:
test.gov.uk. 000 IN SOA ns - 0000.awsdns - 00.co.uk.awsdns - hostmaster.amazon.com. 0 0000 900 0000000 00000

;; Query time: 2 msec
;; SERVER: 208.67.222.222#53
;; WHEN: Wed Aug 25 18:33:22 Z 2021
;; MSG SIZE rcvd: 137

; Trying next server.
; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 9211
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1

;; ERROR: Non-Existent Domain
;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 512; code: NoError
;; QUESTION SECTION:
test1.test.gov.uk.IN TXT

;; AUTHORITIES SECTION:
test.gov.uk. 000 IN SOA ns - 0000.awsdns - 00.co.uk.awsdns - hostmaster.amazon.com. 0 0000 900 0000000 00000

;; Query time: 33 msec
;; SERVER: 9.9.9.9#53
;; WHEN: Wed Aug 25 18:33:22 Z 2021
;; MSG SIZE rcvd: 137

; Trying next server.
; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: Query Refused, id: 15505
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 0, ADDITIONAL: 0

;; ERROR: Query Refused
;; QUESTION SECTION:
test1.test.gov.uk.IN TXT

;; Query time: 2 msec
;; SERVER: 1.1.1.1#53
;; WHEN: Wed Aug 25 18:33:22 Z 2021
;; MSG SIZE rcvd: 41";

            List<AuditParseResult> expectedResult = new List<AuditParseResult>
            {
                new AuditParseResult { NameServer = "8.8.8.8", MessageSize = 137, QueryTime = "23 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "208.67.222.222", MessageSize = 137, QueryTime = "2 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "9.9.9.9", MessageSize = 137, QueryTime = "33 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "1.1.1.1", MessageSize = 41, QueryTime = "2 msec", Error = "Query Refused" }
            };

            List<AuditParseResult> result = _auditTrailParser.Parse(auditTrail);

            Assert.AreEqual(expectedResult.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].NameServer, result[i].NameServer);
                Assert.AreEqual(expectedResult[i].MessageSize, result[i].MessageSize);
                Assert.AreEqual(expectedResult[i].QueryTime, result[i].QueryTime);
                Assert.AreEqual(expectedResult[i].Error, result[i].Error);
            }
        }

        [Test]
        public void TestAuditTrailParsingAllErrorsWithNewLineCharacter()
        {
            string auditTrail = @"; (4 server found)\n;; Got answer:\n;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 26634\r\n;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1\n\n;; ERROR: Non-Existent Domain\n;; OPT PSEUDOSECTION:\n; EDNS: version: 0, flags:; UDP: 4096; code: NoError\n;; QUESTION SECTION:\ntest1.test.gov.uk.uk.        \tIN \tTXT\n\n;; AUTHORITIES SECTION:\test.gov.uk.uk.               \t900 \tIN \tSOA \tns-885.awsdns-46.net. awsdns-hostmaster.amazon.com. 1 7200 900 1209600 86400\n\n;; Query time: 3 msec\n;; SERVER: 208.67.222.222#53\n;; WHEN: Tue Sep 14 11:54:09 Z 2021\n;; MSG SIZE  rcvd: 137\n\n; Trying next server.\n; (4 server found)\n;; Got answer:\n;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 25828\r\n;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1\n\n;; ERROR: Non-Existent Domain\n;; OPT PSEUDOSECTION:\n; EDNS: version: 0, flags:; UDP: 512; code: NoError\n;; QUESTION SECTION:\ntest1.test.gov.uk.uk.        \tIN \tTXT\n\n;; AUTHORITIES SECTION:\test.gov.uk.uk.               \t900 \tIN \tSOA \tns-885.awsdns-46.net. awsdns-hostmaster.amazon.com. 1 7200 900 1209600 86400\n\n;; Query time: 94 msec\n;; SERVER: 9.9.9.9#53\n;; WHEN: Tue Sep 14 11:54:09 Z 2021\n;; MSG SIZE  rcvd: 137\n\n; Trying next server.\n; (4 server found)\n;; Got answer:\n;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 58062\r\n;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1\n\n;; ERROR: Non-Existent Domain\n;; OPT PSEUDOSECTION:\n; EDNS: version: 0, flags:; UDP: 512; code: NoError\n;; QUESTION SECTION:\ntest1.test.gov.uk.uk.        \tIN \tTXT\n\n;; AUTHORITIES SECTION:\test.gov.uk.uk.               \t900 \tIN \tSOA \tns-885.awsdns-46.net. awsdns-hostmaster.amazon.com. 1 7200 900 1209600 86400\n\n;; Query time: 62 msec\n;; SERVER: 8.8.8.8#53\n;; WHEN: Tue Sep 14 11:54:09 Z 2021\n;; MSG SIZE  rcvd: 137\n; (4 server found)\n;; Got answer:\n;; ->>HEADER<<- opcode: Query, status: Query Refused, id: 22597\r\n;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 0, ADDITIONAL: 0\n\n;; ERROR: Query Refused\n;; QUESTION SECTION:\ntest1.test.gov.uk.uk.        \tIN \tTXT\n\n;; Query time: 17 msec\n;; SERVER: 1.1.1.1#53\n;; WHEN: Tue Sep 14 11:54:09 Z 2021\n;; MSG SIZE  rcvd: 42\n";

            List<AuditParseResult> expectedResult = new List<AuditParseResult>
            {
                new AuditParseResult { NameServer = "208.67.222.222", MessageSize = 137, QueryTime = "3 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "9.9.9.9", MessageSize = 137, QueryTime = "94 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "8.8.8.8", MessageSize = 137, QueryTime = "62 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "1.1.1.1", MessageSize = 42, QueryTime = "17 msec", Error = "Query Refused" }
            };

            List<AuditParseResult> result = _auditTrailParser.Parse(auditTrail);

            Assert.AreEqual(expectedResult.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].NameServer, result[i].NameServer);
                Assert.AreEqual(expectedResult[i].MessageSize, result[i].MessageSize);
                Assert.AreEqual(expectedResult[i].QueryTime, result[i].QueryTime);
                Assert.AreEqual(expectedResult[i].Error, result[i].Error);
            }
        }

        [Test]
        public void TestAuditTrailParsingWithNoErrors()
        {
            string auditTrail =
@"; (2 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: No Error, id: 57490
;; flags: qr rd ra; QUERY: 1, ANSWER: 3, AUTHORITY: 7, ADDITIONAL: 1

;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 512; code: NoError
;; QUESTION SECTION:
test.gov.uk.                     	IN 	ANY

;; ANSWER SECTION:
test.gov.uk.                     	1800 	IN 	RRSIG 	TXT RSASHA256 2 1800 20211015130753 20210915130753 11400 gov.uk. HuZrtX6ikJhZU2Xu8VT/iM3KlTmzBBanYj1eMAmpeABZ9BaPEIaYOqGxq4X3YfOLQxQHLYKH4xirnFxPsQS5GWaan0Cvpbe2s7kyN/iOuPG18JXiLmD86zymjuEZXTWFkinSdo/8wtZhNZhMoVEENpfNDlFWzqwcFvtBX0fGntc=
test.gov.uk.                     	1800 	IN 	TXT 	""v = spf1 ? all""
test.gov.uk.                        1800    IN TXT     ""v=DMARC1\;p=reject\;rua=mailto:govuk-rua@dmarc.service.gov.uk""

;; AUTHORITIES SECTION:
gov.uk.                             21600   IN NS  ns0.ja.net.
gov.uk.                             21600   IN NS  ns3.ja.net.
gov.uk.                             21600   IN NS  ns1.surfnet.nl.
gov.uk.                             21600   IN NS  auth50.ns.de.uu.net.
gov.uk.                             21600   IN NS  auth00.ns.de.uu.net.
gov.uk.                             21600   IN NS  ns2.ja.net.
gov.uk.                             21600   IN NS  ns4.ja.net.


;; Query time: 30 msec
;; SERVER: 8.8.8.8#53
;; WHEN: Thu Sep 16 08:50:13 Z 2021
;; MSG SIZE  rcvd: 459";

            List<AuditParseResult> expectedResult = new List<AuditParseResult>
            {
                new AuditParseResult { NameServer = "8.8.8.8", MessageSize = 459, QueryTime = "30 msec", Error = "No Error" }
            };

            List<AuditParseResult> result = _auditTrailParser.Parse(auditTrail);

            Assert.AreEqual(expectedResult.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].NameServer, result[i].NameServer);
                Assert.AreEqual(expectedResult[i].MessageSize, result[i].MessageSize);
                Assert.AreEqual(expectedResult[i].QueryTime, result[i].QueryTime);
                Assert.AreEqual(expectedResult[i].Error, result[i].Error);
            }
        }

        [Test]
        public void TestAuditTrailParsingMixOfErrorAndSuccess()
        {
            string auditTrail = @"; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: No Error, id: 52817
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1

;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 512; code: NoError
;; QUESTION SECTION:
test.gov.uk.                     	IN 	MX

;; AUTHORITIES SECTION:
gov.uk.                          	1800 	IN 	SOA 	ns0.ja.net. operations.ja.net. 2021091630 28800 7200 3600000 14400

;; Query time: 19 msec
;; SERVER: 8.8.4.4#53
;; WHEN: Thu Sep 16 10:39:42 Z 2021
;; MSG SIZE  rcvd: 97

; Trying next server.
; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: Non-Existent Domain, id: 36238
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1

;; ERROR: Non-Existent Domain
;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 512; code: NoError
;; QUESTION SECTION:
test1.test.gov.uk.IN TXT

;; AUTHORITIES SECTION:
test.gov.uk. 000 IN SOA ns0.ja.net. operations.ja.net. 2021091630 28800 7200 3600000 14400

;; Query time: 23 msec
;; SERVER: 8.8.8.8#53
;; WHEN: Wed Aug 25 18:33:22 Z 2021
;; MSG SIZE rcvd: 137

; Trying next server.
; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: Query Refused, id: 15505
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 0, ADDITIONAL: 0

;; ERROR: Query Refused
;; QUESTION SECTION:
test1.test.gov.uk.IN TXT

;; Query time: 2 msec
;; SERVER: 1.1.1.1#53
;; WHEN: Wed Aug 25 18:33:22 Z 2021
;; MSG SIZE rcvd: 41

; Trying next server.
; (4 server found)
;; Got answer:
;; ->>HEADER<<- opcode: Query, status: No Error, id: 36863
;; flags: qr rd ra; QUERY: 1, ANSWER: 0, AUTHORITY: 1, ADDITIONAL: 1

;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; UDP: 512; code: NoError
;; QUESTION SECTION:
test.gov.uk.                     	IN 	MX

;; AUTHORITIES SECTION:
gov.uk.                          	1800 	IN 	SOA 	ns0.ja.net. operations.ja.net. 2021091630 28800 7200 3600000 14400

;; Query time: 31 msec
;; SERVER: 9.9.9.9#53
;; WHEN: Thu Sep 16 10:54:54 Z 2021
;; MSG SIZE  rcvd: 97";

            List<AuditParseResult> expectedResult = new List<AuditParseResult>
            {
                new AuditParseResult { NameServer = "8.8.4.4", MessageSize = 97, QueryTime = "19 msec", Error = "No Error" },
                new AuditParseResult { NameServer = "8.8.8.8", MessageSize = 137, QueryTime = "23 msec", Error = "Non-Existent Domain" },
                new AuditParseResult { NameServer = "1.1.1.1", MessageSize = 41, QueryTime = "2 msec", Error = "Query Refused" },
                new AuditParseResult { NameServer = "9.9.9.9", MessageSize = 97, QueryTime = "31 msec", Error = "No Error" }
            };

            List<AuditParseResult> result = _auditTrailParser.Parse(auditTrail);

            Assert.AreEqual(expectedResult.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].NameServer, result[i].NameServer);
                Assert.AreEqual(expectedResult[i].MessageSize, result[i].MessageSize);
                Assert.AreEqual(expectedResult[i].QueryTime, result[i].QueryTime);
                Assert.AreEqual(expectedResult[i].Error, result[i].Error);
            }
        }
    }
}