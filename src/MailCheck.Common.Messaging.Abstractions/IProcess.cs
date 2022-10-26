using System.Threading.Tasks;

namespace MailCheck.Common.Messaging.Abstractions
{
    public interface IProcess
    {
        Task<ProcessResult> Process();
    }

    public class ProcessResult
    {
        public static ProcessResult Continue = new ProcessResult(true);
        public static ProcessResult Stop = new ProcessResult(false);

        private ProcessResult(bool continueProcessing)
        {
            ContinueProcessing = continueProcessing;
        }

        public bool ContinueProcessing { get; }
    }
}
