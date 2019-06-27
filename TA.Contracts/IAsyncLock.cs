using System;
using System.Threading.Tasks;

namespace TA.Contracts
{
    public interface IAsyncLock
    {
        Func<Task> Task { get; }
        Task Invoke();
    }

    public interface IAsyncLock<TResult> : IAsyncLock
    {
        new Func<Task<TResult>> Task { get; }
        new Task<TResult> Invoke();
    }
}