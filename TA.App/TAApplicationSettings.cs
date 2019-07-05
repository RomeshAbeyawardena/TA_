using Microsoft.Extensions.Configuration;
using TA.Contracts;

namespace TA.App
{
    public class TAApplicationSettings : IApplicationSettings
    {
        public TAApplicationSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }
        public string ConnectionString { get; set; }
        public string ApiKey { get; set; }
        public long? DefaultTokenExpiryPeriodInDays { get; set; }
    }
}