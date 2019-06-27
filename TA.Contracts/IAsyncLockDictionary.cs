using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TA.Contracts
{
    public interface IAsyncLockDictionary
    {
        IAsyncLock GetOrCreate(string key, IAsyncLock asyncLock);
        IAsyncLock GetOrCreate(string key, Func<Task> action);
        IAsyncLock<T> GetOrCreate<T>(string key, Func<Task<T>> action);
    }
}