using System;

namespace TA.Domains.Contracts
{
    public interface ICreated : ICreated<DateTimeOffset>
    {

    }

    public interface ICreated<TDate> where TDate : struct
    {
        TDate Created { get; set; }
    }
}