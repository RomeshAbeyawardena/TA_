using System;
using System.Threading.Tasks;

namespace TA.Contracts
{
    public interface IAsyncLock
    {
        Task Invoke();
    }

    public interface IAsyncLock<TResult> : IAsyncLock
    {
        new Task<TResult> Invoke();
    }
}