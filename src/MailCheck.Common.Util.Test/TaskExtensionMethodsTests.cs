using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MailCheck.Common.Util.Test
{
    [TestFixture]
    public class TaskExtensionMethodsTests
    {
        [Test]
        public async Task TaskCompletesBeforeTimeoutTaskReturned()
        {
            bool expected = true;
            Task<bool> task = Task.FromResult(expected);
            bool actual = await task.TimeoutAfter(TimeSpan.FromTicks(1));
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void TimeoutCompleteBeforeTaskThrowsTimeoutException()
        {
            Task<bool> task = new TaskCompletionSource<bool>().Task;
            Assert.ThrowsAsync<TimeoutException>(async () => await task.TimeoutAfter(TimeSpan.FromTicks(1)));
        }
    }
}