using System;

namespace TA.Contracts
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
    }
}