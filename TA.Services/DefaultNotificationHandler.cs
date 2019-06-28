using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TA.Contracts;
using System.Timers;

namespace TA.Services
{
    public class DefaultNotificationHandler : INotificationHandler
    {
        private readonly object _lockObject = new object();
        private readonly IList<Action<INotification>> _notificationTriggerList;
        private readonly ConcurrentQueue<INotification> _notifyHandlerQueue;
        private readonly Timer _timer; 

        public DefaultNotificationHandler()
        {
            _notificationTriggerList = new List<Action<INotification>>();
            _notifyHandlerQueue = new ConcurrentQueue<INotification>();
            _timer = new Timer(1000);
            _timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
            lock (_lockObject)
            {
                var nextNotification = Dequeue();
                if (nextNotification == null) 
                    return;

                foreach (var notificationTrigger in _notificationTriggerList) 
                { 
                    notificationTrigger(nextNotification);
                }
            }
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

        public void Subscribe(Action<INotification> notificationTrigger)
        {
            lock (_lockObject)
            {
                _notificationTriggerList.Add(notificationTrigger);
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}