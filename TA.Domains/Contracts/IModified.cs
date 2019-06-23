using System;

namespace TA.Domains.Contracts
{
    public interface IModified : IModified<DateTimeOffset>
    {

    }

    public interface IModified<TDate> where TDate : struct
    {
        TDate Modified { get; set; }
    }
}