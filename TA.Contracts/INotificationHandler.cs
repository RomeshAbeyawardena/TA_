using System;

namespace TA.Contracts
{
    public interface INotificationHandler : IDisposable
    {
        void Enqueue(INotification notification);
        INotification Dequeue();
        INotification<TResult> Dequeue<TResult>();
        void Subscribe(Action<INotification> notificationTrigger);
    }
}