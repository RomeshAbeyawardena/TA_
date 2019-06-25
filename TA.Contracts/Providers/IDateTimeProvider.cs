using System;

namespace TA.Contracts.Providers
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
    }
}