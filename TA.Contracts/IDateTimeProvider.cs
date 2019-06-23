using System;

namespace TA.Contracts
{
    public interface IDateTimeProvider
    {
        DateTimeOffset DateTimeOffSet { get; }
    }
}