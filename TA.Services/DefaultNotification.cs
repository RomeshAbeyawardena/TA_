using System;
using TA.Contracts;

namespace TA.Services
{
    public abstract class DefaultNotification : INotification
    {
        public object EventResult { get; private set; }
        public INotification Notify(object eventResult)
        {
            EventResult = eventResult;
            return this;
        }
    }

    public class DefaultNotification<TResult> : DefaultNotification, INotification<TResult>
    {
        public new TResult EventResult => (TResult)base.EventResult;

        public INotification<TResult> Notify(TResult eventResult)
        {
            base.Notify(eventResult);
            return this;
        }
    }
}