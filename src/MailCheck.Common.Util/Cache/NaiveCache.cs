using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MailCheck.Common.Util.Cache
{
    public interface ICache<T>
    {
        Task<T> GetOrAddAsync(string key, Func<Task<T>> factory, TimeSpan ttl);
    }

    public class NaiveCache<T> : ICache<T>
    {
        private readonly IClock _clock;
        private readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public NaiveCache(IClock clock)
        {
            _clock = clock;
        }

        public async Task<T> GetOrAddAsync(string key, Func<Task<T>> factory, TimeSpan ttl)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (!_cache.TryGetValue(key, out CacheItem cacheItem) || cacheItem.Expiry <= _clock.GetDateTimeUtc())
                {
                    T item = await factory();

                    cacheItem = new CacheItem(item, _clock.GetDateTimeUtc().Add(ttl));

                    _cache[key] = cacheItem;
                }

                return cacheItem.Item;
            }
            finally
            {
                _semaphore.Release(1);
            }
        }

        private class CacheItem
        {
            public CacheItem(T item, DateTime expiry)
            {
                Item = item;
                Expiry = expiry;
            }

            public T Item { get; }
            public DateTime Expiry { get; }
        }
    }
}
