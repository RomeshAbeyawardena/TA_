using System;
using System.Threading;
using System.Threading.Tasks;
using TA.Contracts;
using TA.Domains.Options;

namespace TA.Services
{
    public class DefaultAsyncLock : IAsyncLock
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly AsyncLockOptions _asyncLockOptions;
        private DefaultAsyncLock(Action<AsyncLockOptions> asyncLockOptions)
        {
            _asyncLockOptions = new AsyncLockOptions();
            asyncLockOptions(_asyncLockOptions);
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public static IAsyncLock Create(Action<AsyncLockOptions> asyncLockOptions)
        {
            return new DefaultAsyncLock(asyncLockOptions);
        }
        public async Task Invoke()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await _asyncLockOptions.Task();
            }
            finally
            {
                _semaphoreSlim.Release(1);
            }
        }
    }
    public class DefaultAsyncLock<TResult> : IAsyncLock<TResult>
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly AsyncLockOptions<TResult> _asyncLockOptions;
        private DefaultAsyncLock(Action<AsyncLockOptions<TResult>> asyncLockOptions)
        {
            _asyncLockOptions = new AsyncLockOptions<TResult>();
            asyncLockOptions(_asyncLockOptions);
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public static IAsyncLock<TResult> Create(Action<AsyncLockOptions<TResult>> asyncLockOptions)
        {
            return new DefaultAsyncLock<TResult>(asyncLockOptions);
        }

        public async Task<TResult> Invoke()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                return await _asyncLockOptions.Task();
            }
            finally
            {
                _semaphoreSlim.Release(1);
            }
        }

        Task IAsyncLock.Invoke()
        {
            return Invoke();
        }
    }
}