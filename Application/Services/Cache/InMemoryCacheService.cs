using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Application.Services.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache=new MemoryCache(new MemoryCacheOptions());
        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            var json = _cache.Get<string>(key);
            if (json == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public T GetCacheValue<T>(string key)
        {
            var json = _cache.Get<string>(key);
            if (json == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public Task SetCacheValueAsync<T>(string key, T value)
        {
            _cache.Set(key, JsonConvert.SerializeObject(value));
            return Task.CompletedTask;
        }

        public Task DeleteCacheValue(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
