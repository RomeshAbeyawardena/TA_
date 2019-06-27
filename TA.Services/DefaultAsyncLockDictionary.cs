using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TA.Contracts;

namespace TA.Services
{
    public class DefaultAsyncLockDictionary :IAsyncLockDictionary
    {
        private readonly ConcurrentDictionary<string, IAsyncLock> _asyncLocks;

        public DefaultAsyncLockDictionary(ConcurrentDictionary<string, IAsyncLock> asyncLocks = null)
        {
            _asyncLocks = asyncLocks ?? new ConcurrentDictionary<string, IAsyncLock>();
        }

        public IAsyncLock GetOrCreate(string key, IAsyncLock value)
        {
            if (_asyncLocks.TryGetValue(key, out var asyncLock))
                return asyncLock;

            if(!_asyncLocks.TryAdd(key, value))
                throw new InvalidOperationException();

            return value;
        }

        public IAsyncLock GetOrCreate(string key, Func<Task> action)
        {
            return GetOrCreate(key, DefaultAsyncLock.Create(action));
        }

        public IAsyncLock<T> GetOrCreate<T>(string key, Func<Task<T>> action)
        {
            if (GetOrCreate(key, DefaultAsyncLock<T>.Create(action)) is IAsyncLock<T> genericLockAsync)
                return genericLockAsync;

            return null;
        }
    }
}