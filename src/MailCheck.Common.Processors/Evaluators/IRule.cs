using System.Collections.Generic;
using System.Threading.Tasks;
using MailCheck.Common.Contracts.Advisories;

namespace MailCheck.Common.Processors.Evaluators
{
    public interface IRule<in T>
    {
        Task<List<AdvisoryMessage>> Evaluate(T t);
        int SequenceNo { get; }
        bool IsStopRule { get; }
    }
}