using ATM.Core.Contracts;
using ATM.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ATM.Infrastructure.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly CacheSettings _cacheSettings;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public CacheService(IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _cacheSettings = cacheSettings.Value;
            _cache = cache;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes));
        }
        
        public T Get<T>(string key) where T : class
        {   
            return _cache.Get<T>(key);
        }

        public void Set<T>(string key, T value) where T : class
        {
            _cache.Set(key, value);
        }      
    }
}
