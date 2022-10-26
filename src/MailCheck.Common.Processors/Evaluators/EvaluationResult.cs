using System.Collections.Generic;
using System.Linq;
using MailCheck.Common.Contracts.Advisories;

namespace MailCheck.Common.Processors.Evaluators
{
    public class EvaluationResult<T>
    {
        public EvaluationResult(T item, params AdvisoryMessage[] advisoryMessages)
        {
            Item = item;
            AdvisoryMessages = advisoryMessages.ToList();
        }

        public EvaluationResult(T item, List<AdvisoryMessage> advisoryMessages)
        {
            Item = item;
            AdvisoryMessages = advisoryMessages;
        }

        public T Item { get; }

        public List<AdvisoryMessage> AdvisoryMessages { get; }
    }
}
