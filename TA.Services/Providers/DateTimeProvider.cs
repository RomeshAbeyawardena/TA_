using System;
using Microsoft.Extensions.Internal;
using TA.Contracts;
using TA.Contracts.Providers;

namespace TA.Services.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly ISystemClock _systemClock;
        public DateTimeOffset Now => _systemClock.UtcNow;

        public DateTimeProvider(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }
    }
}