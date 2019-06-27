using System;
using System.Threading;
using System.Threading.Tasks;
using TA.Contracts;

namespace TA.Services
{
    public class DefaultAsyncLock : IAsyncLock
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private DefaultAsyncLock(Func<Task> task)
        {
            Task = task;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public static IAsyncLock Create(Func<Task> task)
        {
            return new DefaultAsyncLock(task);
        }
        public Func<Task> Task { get; }
        public async Task Invoke()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await Task();
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
        private DefaultAsyncLock(Func<Task<TResult>> task)
        {
            Task = task;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public static IAsyncLock<TResult> Create(Func<Task<TResult>> task)
        {
            return new DefaultAsyncLock<TResult>(task);
        }

        Func<Task> IAsyncLock.Task => Task;

        public async Task<TResult> Invoke()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                return await Task();
            }
            finally
            {
                _semaphoreSlim.Release(1);
            }
        }

        public Func<Task<TResult>> Task { get; }

        Task IAsyncLock.Invoke()
        {
            return Invoke();
        }
    }
}