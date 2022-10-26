using System;
using System.Collections.Generic;
using System.Linq;
using MailCheck.Common.Contracts.Findings;

namespace MailCheck.Common.Processors.Notifiers
{
    public interface IFindingsChangedNotifier
    {
        FindingsChanged Process(string domain, string recordType, IList<Finding> currentFindings, IList<Finding> newFindings);
    }

    public class FindingsChangedNotifier : IFindingsChangedNotifier
    {
        private static readonly MessageEqualityComparer DefaultComparer = new MessageEqualityComparer();

        public FindingsChanged Process(string domain, string recordType, IList<Finding> currentFindings, IList<Finding> newFindings)
        {
            currentFindings = currentFindings ?? new List<Finding>();
            newFindings = newFindings ?? new List<Finding>();
            
            IList<Finding> addedFindings = newFindings.Except(currentFindings, DefaultComparer).ToList();

            IList<Finding> sustainedFindings = currentFindings.Intersect(newFindings, DefaultComparer).ToList();

            IList<Finding> removedFindings = currentFindings.Except(newFindings, DefaultComparer).ToList();

            return new FindingsChanged(Guid.NewGuid().ToString())
            {
                Domain = domain,
                RecordType = recordType,
                Added = addedFindings,
                Sustained = sustainedFindings,
                Removed = removedFindings
            };
        }
    }
}
