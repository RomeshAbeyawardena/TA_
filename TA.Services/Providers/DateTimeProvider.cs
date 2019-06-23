using System;
using Microsoft.Extensions.Internal;
using TA.Contracts;

namespace TA.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly ISystemClock _systemClock;
        public DateTimeOffset DateTimeOffSet => _systemClock.UtcNow;

        public DateTimeProvider(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }
    }
}