using Microsoft.Extensions.Caching.Memory;

namespace API_TEMPLATE.CustomClass
{
    public interface ICustomMemoryCache
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null);
        void Remove(string key);
    }
    public class CustomMemoryCache : ICustomMemoryCache
    {
        private readonly IMemoryCache _memoryCache;

        public CustomMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }

            if (absoluteExpiration.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpiration.Value);
            }

            _memoryCache.Set(key, value, options);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
