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
        private readonly IAsyncLockDictionary _asyncLockDictionary;
        private readonly INotificationHandler _notificationHandler;

        public CacheProvider(IDistributedCache distributedCache, IAsyncLockDictionary asyncLockDictionary, INotificationHandler notificationHandler)
        {
            _keyEntries = new List<string>();
            _distributedCache = distributedCache;
            _asyncLockDictionary = asyncLockDictionary;
            _notificationHandler = notificationHandler;
        }

        public async Task<T> Get<T>(CacheType cacheType, string key)
        {
            return await _asyncLockDictionary.GetOrCreate("Get", async () =>
            {
                if (!_keyEntries.Contains(key))
                    return default;
                var result = await _distributedCache.GetStringAsync(key, CancellationToken.None);
                _notificationHandler.Enqueue(new DefaultNotification<string>());
                return result == null ? default : JToken.Parse(result).ToObject<T>();
            }).Invoke();
        }

        public async Task Set<T>(CacheType cacheType, string key, T value)
        {
            await _asyncLockDictionary.GetOrCreate("Set", async () =>
            {
                _keyEntries.Add(key);
                var val = JToken.FromObject(value).ToString();
                await _distributedCache.SetStringAsync(key, val);
            }).Invoke();
        }

        public async Task<int> Clear()
        {
            return await _asyncLockDictionary.GetOrCreate("Clear", async () =>
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