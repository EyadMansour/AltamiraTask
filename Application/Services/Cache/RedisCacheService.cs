using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Application.Services.Cache
{
    public class RedisCacheService:ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = await db.StringGetAsync(key);
            if (json.ToString() == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public T GetCacheValue<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = db.StringGet(key);
            if (json.ToString() == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }



        public async Task SetCacheValueAsync<T>(string key, T value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }
        public Task DeleteCacheValue(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.KeyDelete(key);
            return Task.CompletedTask;
        }
    }
}
