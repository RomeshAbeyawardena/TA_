using System;

namespace TA.Contracts
{
    public interface INotification
    {
        object EventResult { get; }
        void Notify(object eventResult);
    }
    public interface INotification<TEventResult> : INotification
    {
        new TEventResult EventResult { get; }
        void Notify(TEventResult eventResult);
    }
}