using System;

namespace TA.Contracts
{
    public interface INotification
    {
        object EventResult { get; }
        INotification Notify(object eventResult);
    }
    public interface INotification<TEventResult> : INotification
    {
        new TEventResult EventResult { get; }
        INotification<TEventResult> Notify(TEventResult eventResult);
    }
}