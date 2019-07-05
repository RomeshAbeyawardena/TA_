using System.Collections.Generic;
using System.Reflection;
using TA.Contracts;
using WebToolkit.Contracts;

namespace TA.ServiceBroker
{
    public class TAServiceBroker : IServiceBroker
    {
        public IEnumerable<Assembly> GetServiceAssemblies()
        {
            return new [] { 
                Assembly.GetAssembly(typeof(Data.ServiceRegistration)), 
                Assembly.GetAssembly(typeof(Services.ServiceRegistration))
            };
        }
    }
}