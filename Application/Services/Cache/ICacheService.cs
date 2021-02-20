using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Cache
{
    public interface ICacheService
    {
        public Task<T> GetCacheValueAsync<T>(string key);
        public T GetCacheValue<T>(string key);
        public Task SetCacheValueAsync<T>(string key, T value);
        public Task DeleteCacheValue(string key);
    }
}
