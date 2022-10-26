using System;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using MailCheck.Common.Util.Cache;
using NUnit.Framework;

namespace MailCheck.Common.Util.Test.Cache
{
    [TestFixture]
    public class NaiveCacheTests
    {
        private const string Key1 = "key1";
        private const string Key2 = "key2";


        private IClock _clock;
        private NaiveCache<string> _cache;

        [SetUp]
        public void SetUp()
        {
            _clock = A.Fake<IClock>();
            _cache = new NaiveCache<string>(_clock);
        }

        [Test]
        public async Task NoValueExistsOriginIsHit()
        {
            Func<Task<string>> factory = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory.Invoke()).ReturnsLazily(() => Guid.NewGuid().ToString());

            string value = await _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(1));

            Assert.That(value, Is.Not.Null);
            A.CallTo(() => factory.Invoke()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task ExpiredValueExistsOriginIsHit()
        {
            DateTime now = DateTime.UtcNow;
            int cacheItemTtlSeconds = 10;
            A.CallTo(() => _clock.GetDateTimeUtc()).ReturnsNextFromSequence(now, now.AddSeconds(cacheItemTtlSeconds));

            Func<Task<string>> factory = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory.Invoke()).ReturnsLazily(() => Guid.NewGuid().ToString());

            string value1 = await _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds));
            string value2 = await _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds));

            Assert.That(value1, Is.Not.SameAs(value2));
            A.CallTo(() => factory.Invoke()).MustHaveHappenedTwiceExactly();
        }

        [Test]
        public async Task ValueExistsValueIsServedFromCache()
        {
            DateTime now = DateTime.UtcNow;
            int cacheItemTtlSeconds = 10;
            A.CallTo(() => _clock.GetDateTimeUtc()).ReturnsNextFromSequence(now, now.AddSeconds(cacheItemTtlSeconds - 1));

            Func<Task<string>> factory = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory.Invoke()).ReturnsLazily(() => Guid.NewGuid().ToString());

            string value1 = await _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds));
            string value2 = await _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds));

            Assert.That(value1, Is.SameAs(value2));
            A.CallTo(() => factory.Invoke()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DifferentKeysReturnDifferenceObjects()
        {
            DateTime now = DateTime.UtcNow;
            int cacheItemTtlSeconds = 10;
            A.CallTo(() => _clock.GetDateTimeUtc()).ReturnsNextFromSequence(now, now.AddSeconds(cacheItemTtlSeconds - 1));

            Func<Task<string>> factory = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory.Invoke()).ReturnsLazily(() => Guid.NewGuid().ToString());

            string value1 = await _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds));
            string value2 = await _cache.GetOrAddAsync(Key2, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds));

            Assert.That(value1, Is.Not.SameAs(value2));
            A.CallTo(() => factory.Invoke()).MustHaveHappenedTwiceExactly();
        }

        [Test]
        public async Task MultipleTasksAccessingKeyOnlyOneHitsOrigin()
        {
            DateTime now = DateTime.UtcNow;
            int cacheItemTtlSeconds = 10;
            A.CallTo(() => _clock.GetDateTimeUtc()).ReturnsNextFromSequence(now, now.AddSeconds(cacheItemTtlSeconds - 1));

            Func<Task<string>> factory = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory.Invoke()).ReturnsLazily(() => Guid.NewGuid().ToString());

            var tasks = Enumerable.Range(0, 2).Select(_ =>
                _cache.GetOrAddAsync(Key1, factory, TimeSpan.FromSeconds(cacheItemTtlSeconds)))
                .ToList();

            await Task.WhenAll(tasks);

            Assert.That(tasks[0].Result, Is.SameAs(tasks[1].Result));
            A.CallTo(() => factory.Invoke()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task FactoryThrowsExceptionsTaskNotCached()
        {
            DateTime now = DateTime.UtcNow;
            int cacheItemTtlSeconds = 10;
            A.CallTo(() => _clock.GetDateTimeUtc()).ReturnsNextFromSequence(now, now.AddSeconds(cacheItemTtlSeconds - 1));

            Func<Task<string>> factory1 = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory1.Invoke()).ReturnsLazily((Func<string>)(() => throw new Exception()));

            Func<Task<string>> factory2 = A.Fake<Func<Task<string>>>();
            A.CallTo(() => factory2.Invoke()).ReturnsLazily(() => Guid.NewGuid().ToString());

            try
            {
                string value1 = await _cache.GetOrAddAsync(Key1, factory1, TimeSpan.FromSeconds(cacheItemTtlSeconds));
            }
            catch
            {
                //swallow
            }

            string value2 = await _cache.GetOrAddAsync(Key2, factory2, TimeSpan.FromSeconds(cacheItemTtlSeconds));

            Assert.That(value2, Is.Not.Null);
            A.CallTo(() => factory1.Invoke()).MustHaveHappenedOnceExactly();
            A.CallTo(() => factory2.Invoke()).MustHaveHappenedOnceExactly();
        }
    }
}
