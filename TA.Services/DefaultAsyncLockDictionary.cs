using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TA.Contracts;

namespace TA.Services
{
    public class DefaultAsyncLockDictionary :IAsyncLockDictionary
    {
        private readonly ConcurrentDictionary<string, IAsyncLock> _asyncLockDictionary;

        public DefaultAsyncLockDictionary(ConcurrentDictionary<string, IAsyncLock> asyncLocks = null)
        {
            _asyncLockDictionary = asyncLocks ?? new ConcurrentDictionary<string, IAsyncLock>();
        }

        public IAsyncLock GetOrCreate(string key, IAsyncLock value)
        {
            if (_asyncLockDictionary.TryGetValue(key, out var asyncLock))
            {
                if(!_asyncLockDictionary.TryUpdate(key, value, asyncLock))
                    throw  new InvalidOperationException();

                return asyncLock;   
            }

            if(!_asyncLockDictionary.TryAdd(key, value))
                throw new InvalidOperationException();

            return value;
        }

        public IAsyncLock GetOrCreate(string key, Func<Task> action)
        {
            return GetOrCreate(key, DefaultAsyncLock.Create(options => { 
                options.Task = action;
                options.ReleaseCount = 1;
                options.Initial = 1;
                options.Maximum = 1;
            }));
        }

        public IAsyncLock<T> GetOrCreate<T>(string key, Func<Task<T>> action)
        {
            if (GetOrCreate(key, DefaultAsyncLock<T>.Create(options => { 
                options.Task = action;
                options.ReleaseCount = 1;
                options.Initial = 1;
                options.Maximum = 1;
            })) is IAsyncLock<T> genericLockAsync)
                return genericLockAsync;

            return null;
        }
    }
}