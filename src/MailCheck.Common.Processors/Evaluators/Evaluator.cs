using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailCheck.Common.Contracts.Advisories;

namespace MailCheck.Common.Processors.Evaluators
{
    public interface IEvaluator<T>
    {
        Task<EvaluationResult<T>> Evaluate(T item);
    }

    public class Evaluator<T> : IEvaluator<T>
    {
        private readonly List<IRule<T>> _rules;

        public Evaluator(IEnumerable<IRule<T>> rules)
        {
            _rules = rules.OrderBy(_ => _.SequenceNo).ToList();
        }

        public virtual async Task<EvaluationResult<T>> Evaluate(T item)
        {
            List<AdvisoryMessage> errors = new List<AdvisoryMessage>();
            foreach (IRule<T> rule in _rules)
            {
                List<AdvisoryMessage> ruleErrors = await rule.Evaluate(item);

                if (ruleErrors.Count > 0)
                {
                    errors.AddRange(ruleErrors);

                    if (rule.IsStopRule)
                    {
                        break;
                    }
                }
            }
            return new EvaluationResult<T>(item, errors);
        }
    }
}