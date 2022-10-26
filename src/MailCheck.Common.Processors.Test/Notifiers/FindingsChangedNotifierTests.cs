using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using MailCheck.Common.Messaging.Abstractions;
using MailCheck.Common.Processors.Notifiers;
using NUnit.Framework;
using MailCheck.Common.Contracts.Findings;
using System.Linq;
using MailCheck.Common.Contracts.Advisories;

namespace MailCheck.Common.Processors.Test.Notifiers
{
    [TestFixture]
    public class FindingsChangedNotifierTests
    {
        private FindingsChangedNotifier _notifier;

        private const string Id = "test.gov.uk";

        [SetUp]
        public void SetUp()
        {
            _notifier = new FindingsChangedNotifier();
        }

        [TestCaseSource(nameof(ExerciseFindingsChangedNotifierTestPermutations))]
        public void ExerciseFindingsChangedNotifier(FindingsChangedNotifierTestCase testCase)
        {
            FindingsChanged result = _notifier.Process(Id, "DMARC", testCase.CurrentFindings, testCase.IncomingFindings);

            Assert.AreEqual(testCase.ExpectedAdded.Count, result.Added.Count);
            Assert.AreEqual(testCase.ExpectedRemoved.Count, result.Removed.Count);
            Assert.AreEqual(testCase.ExpectedSustained.Count, result.Sustained.Count);

            for (int i = 0; i < testCase.ExpectedAdded.Count; i++)
            {
                Assert.AreEqual(testCase.ExpectedAdded[i].Name, result.Added[i].Name);
                Assert.AreEqual(testCase.ExpectedAdded[i].EntityUri, result.Added[i].EntityUri);
                Assert.AreEqual(testCase.ExpectedAdded[i].Severity, result.Added[i].Severity);
                Assert.AreEqual(testCase.ExpectedAdded[i].SourceUrl, result.Added[i].SourceUrl);
                Assert.AreEqual(testCase.ExpectedAdded[i].Title, result.Added[i].Title);
            }

            for (int i = 0; i < testCase.ExpectedRemoved.Count; i++)
            {
                Assert.AreEqual(testCase.ExpectedRemoved[i].Name, result.Removed[i].Name);
                Assert.AreEqual(testCase.ExpectedRemoved[i].EntityUri, result.Removed[i].EntityUri);
                Assert.AreEqual(testCase.ExpectedRemoved[i].Severity, result.Removed[i].Severity);
                Assert.AreEqual(testCase.ExpectedRemoved[i].SourceUrl, result.Removed[i].SourceUrl);
                Assert.AreEqual(testCase.ExpectedRemoved[i].Title, result.Removed[i].Title);
            }

            for (int i = 0; i < testCase.ExpectedSustained.Count; i++)
            {
                Assert.AreEqual(testCase.ExpectedSustained[i].Name, result.Sustained[i].Name);
                Assert.AreEqual(testCase.ExpectedSustained[i].EntityUri, result.Sustained[i].EntityUri);
                Assert.AreEqual(testCase.ExpectedSustained[i].Severity, result.Sustained[i].Severity);
                Assert.AreEqual(testCase.ExpectedSustained[i].SourceUrl, result.Sustained[i].SourceUrl);
                Assert.AreEqual(testCase.ExpectedSustained[i].Title, result.Sustained[i].Title);
            }

        }

        private static IEnumerable<FindingsChangedNotifierTestCase> ExerciseFindingsChangedNotifierTestPermutations()
        {
            Finding findingEvalError1 = new Finding
            {
                EntityUri = "domain:test.gov.uk",
                Name = "mailcheck.dmarc.testName1",
                SourceUrl = $"https://testurl.com/app/domain-security/{Id}/dmarc",
                Severity = "Urgent",
                Title = "EvaluationError"
            };

            Finding findingEvalError2 = new Finding
            {
                EntityUri = "domain:test.gov.uk",
                Name = "mailcheck.dmarc.testName2",
                SourceUrl = $"https://testurl.com/app/domain-security/{Id}/dmarc",
                Severity = "Urgent",
                Title = "EvaluationError"
            };

            Finding findingPollerWarn1 = new Finding
            {
                EntityUri = "domain:test.gov.uk",
                Name = "mailcheck.dmarc.testName3",
                SourceUrl = $"https://testurl.com/app/domain-security/{Id}/dmarc",
                Severity = "Advisory",
                Title = "PollerError"
            };

            FindingsChangedNotifierTestCase test1 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                IncomingFindings = new List<Finding>(),
                ExpectedAdded = new List<Finding>(),
                ExpectedRemoved = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedSustained = new List<Finding>(),
                Description = "3 removed findings should produce 3 findings removed"
            };

            FindingsChangedNotifierTestCase test2 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                IncomingFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedAdded = new List<Finding>(),
                ExpectedRemoved = new List<Finding>(),
                ExpectedSustained = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                Description = "3 sustained findings should produce 3 findings sustained"
            };

            FindingsChangedNotifierTestCase test3 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = new List<Finding>(),
                IncomingFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedAdded = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedRemoved = new List<Finding>(),
                ExpectedSustained = new List<Finding>(),
                Description = "3 added findings should produce 3 findings added"
            };

            FindingsChangedNotifierTestCase test4 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = new List<Finding> { findingEvalError1 },
                IncomingFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedAdded = new List<Finding> { findingEvalError2, findingPollerWarn1 },
                ExpectedRemoved = new List<Finding>(),
                ExpectedSustained = new List<Finding> { findingEvalError1 },
                Description = "2 added findings and 1 sustained should produce 2 findings added and 1 finding sustained"
            };

            FindingsChangedNotifierTestCase test5 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                IncomingFindings = null,
                ExpectedAdded = new List<Finding>(),
                ExpectedRemoved = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedSustained = new List<Finding>(),
                Description = "3 removed findings due to nulls should produce 3 findings removed"
            };

            FindingsChangedNotifierTestCase test6 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = null,
                IncomingFindings = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedAdded = new List<Finding> { findingEvalError1, findingEvalError2, findingPollerWarn1 },
                ExpectedRemoved = new List<Finding>(),
                ExpectedSustained = new List<Finding>(),
                Description = "3 added findings from nulls should produce 3 findings added"
            };

            FindingsChangedNotifierTestCase test7 = new FindingsChangedNotifierTestCase
            {
                CurrentFindings = null,
                IncomingFindings = null,
                ExpectedAdded = new List<Finding>(),
                ExpectedRemoved = new List<Finding>(),
                ExpectedSustained = new List<Finding>(),
                Description = "Null findings both incoming and current produce no findings added/removed/sustained"
            };

            yield return test1;
            yield return test2;
            yield return test3;
            yield return test4;
            yield return test5;
            yield return test6;
            yield return test7;
        }

        public class FindingsChangedNotifierTestCase
        {
            public List<Finding> CurrentFindings { get; set; }
            public List<Finding> IncomingFindings { get; set; }
            public List<Finding> ExpectedAdded { get; set; }
            public List<Finding> ExpectedRemoved { get; set; }
            public List<Finding> ExpectedSustained { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return Description;
            }
        }
    }
}