using System;
using Microsoft.Extensions.Hosting;

namespace TA.Contracts
{
    public interface INotificationHandler
    {
        void Enqueue(INotification notification);
        INotification Dequeue();
        INotification<TResult> Dequeue<TResult>();
        void Subscribe(Action<INotification> notificationTrigger);
        int ProcessAll();
    }
}