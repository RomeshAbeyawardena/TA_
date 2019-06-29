using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using TA.Contracts;
using Timer = System.Timers.Timer;

namespace TA.Services
{
    public class NotificationDispatcherHostedService : IHostedService, IDisposable
    {
        private readonly INotificationHandler _notificationHandler;
        private readonly Timer _timer; 
        
        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            var proccessedCount = _notificationHandler.ProcessAll();
//#if DEBUG == false
            if(proccessedCount > 1)
//#endif
            Console.WriteLine("{0}: Processed {1} notifications", e.SignalTime, proccessedCount);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            return Task.CompletedTask;
        }

        public NotificationDispatcherHostedService(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
            _timer = new Timer(1000);
            _timer.Elapsed += Elapsed;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}