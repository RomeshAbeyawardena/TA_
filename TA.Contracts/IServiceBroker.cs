using System.Collections.Generic;
using System.Reflection;

namespace TA
{
    public interface IServiceBroker
    {
        IEnumerable<Assembly> GetServiceAssemblies();
    }
}