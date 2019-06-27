using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using TA.Contracts;
using TA.Contracts.Providers;
using TA.Domains.Extensions;

namespace TA.Services.Providers
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IAsyncLockDictionary _asyncLocks;
        public CacheProvider(IDistributedCache distributedCache)
        {
            _asyncLocks = new DefaultAsyncLockDictionary();
            _keyEntries = new List<string>();
            _distributedCache = distributedCache;
        }

        public async Task<T> Get<T>(CacheType cacheType, string key)
        {

            return await _asyncLocks.GetOrCreate("Get", async () =>
            {
                if (!_keyEntries.Contains(key))
                    return default;
                var result = await _distributedCache.GetStringAsync(key, CancellationToken.None);
                return result == null ? default : JToken.Parse(result).ToObject<T>();
            }).Invoke();
        }

        public async Task Set<T>(CacheType cacheType, string key, T value)
        {
            await _asyncLocks.GetOrCreate("Set", async () =>
            {
                _keyEntries.Add(key);
                var val = JToken.FromObject(value).ToString();
                await _distributedCache.SetStringAsync(key, val);
            }).Invoke();
        }

        public async Task<int> Clear()
        {
            return await _asyncLocks.GetOrCreate("Clear", async () =>
            {
                var keyCount = _keyEntries.Count;
                await _keyEntries.ForEach(async (key) => await _distributedCache.RemoveAsync(key));
                _keyEntries.Clear();

                return keyCount;
            }).Invoke();
        }


        private readonly IList<string> _keyEntries;
    }
}