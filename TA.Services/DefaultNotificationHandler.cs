using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TA.Contracts;

namespace TA.Services
{
    public class DefaultNotificationHandler : INotificationHandler
    {
        private readonly object _lockObject = new object();
        private readonly IDictionary<string, Action<INotification>> _notificationTriggerList;
        private readonly ConcurrentQueue<INotification> _notifyHandlerQueue;

        public DefaultNotificationHandler()
        {
            _notificationTriggerList = new Dictionary<string, Action<INotification>>();
            _notifyHandlerQueue = new ConcurrentQueue<INotification>();
        }

        public void Enqueue(INotification notification)
        {
            _notifyHandlerQueue.Enqueue(notification);
        }

        public INotification Dequeue()
        {
            return _notifyHandlerQueue.TryDequeue(out var notification)
                ? notification 
                : null;
        }

        public INotification<TResult> Dequeue<TResult>()
        {
            var notification = Dequeue();

            return notification != null 
                   && notification is INotification<TResult> genericNotification 
                ? genericNotification 
                : null;
        }

        public void Subscribe(Action<INotification> notificationTrigger, string callerMember = "")
        {
            lock (_lockObject)
            {
                if(!_notificationTriggerList.ContainsKey(callerMember))
                    _notificationTriggerList.Add(callerMember, notificationTrigger);
            }
        }

        public int ProcessAll()
        {
            lock (_lockObject)
            {
                var notificationCount = 0;
                var nextNotification = Dequeue();
                while (nextNotification != null)
                {
                    notificationCount++;
                    foreach (var notificationTrigger in _notificationTriggerList)
                    {
                        notificationTrigger.Value(nextNotification);
                    }
                    nextNotification = Dequeue();
                }

                return notificationCount;
            }
        }
    }
}