using Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddCache();


            return services;
        }
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheStore, MemoryCacheStore>(x =>
                new MemoryCacheStore(x.GetRequiredService<IMemoryCache>(),
                    new Dictionary<string, TimeSpan>()));


            return services;
        }

    }
}
