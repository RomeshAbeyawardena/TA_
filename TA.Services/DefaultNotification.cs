using System;
using TA.Contracts;

namespace TA.Services
{
    public abstract class DefaultNotification : INotification
    {
        public object EventResult { get; private set; }
        public void Notify(object eventResult)
        {
            EventResult = eventResult;
        }
    }

    public class DefaultNotification<TResult> : DefaultNotification, INotification<TResult>
    {
        public new TResult EventResult => (TResult)base.EventResult;

        public void Notify(TResult eventResult)
        {
            base.Notify(eventResult);
        }
    }
}