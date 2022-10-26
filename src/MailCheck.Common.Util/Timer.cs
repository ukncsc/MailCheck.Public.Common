using System;
using System.Timers;

namespace MailCheck.Common.Util
{
    public interface ITimer
    {
        void Start(TimeSpan interval);

        void Stop();

        event Action Elapsed;
    }

    public class TimerWrapper : ITimer, IDisposable
    {
        private readonly Timer _timer;

        public TimerWrapper()
        {
            _timer = new Timer();
            _timer.Elapsed += TimerOnElapsed;
        }
        
        public void Start(TimeSpan interval)
        {
            _timer.Interval = interval.TotalMilliseconds;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public event Action Elapsed;

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
        }

        public void Dispose()
        {
            _timer.Elapsed -= TimerOnElapsed;
            _timer.Dispose();
        }
    }
}
