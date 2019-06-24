using System;
using Microsoft.Extensions.Internal;
using TA.Contracts;

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