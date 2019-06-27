using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TA.Contracts.Providers;

namespace TA.Services.Providers
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;

        public CacheProvider(IDistributedCache distributedCache)
        {
            _keyEntries = new List<string>();
            _distributedCache = distributedCache;
        }

        public async Task<T> Get<T>(CacheType cacheType, string key)
        {
            var result = await _distributedCache.GetStringAsync(key, CancellationToken.None);
            return result == null ? default : JToken.Parse(result).ToObject<T>();
        }

        public async Task Set<T>(CacheType cacheType, string key, T value)
        {
            var val = JToken.FromObject(value).ToString();
            await _distributedCache.SetStringAsync(key, val);
        }


        private readonly IList<string> _keyEntries;
    }
}