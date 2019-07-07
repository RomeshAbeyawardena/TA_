using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.Contracts.Services;
using TA.Domains.Constants;

namespace TA.App.Controllers.Api
{
    [Route("api/[controller]/[action]"), RequiresApiKey(Permission.ApiAccess)]
    public abstract class ApiControllerBase : ControllerBase
    {
        public async Task ClearSiteCache()
        {
            await ClearCache(Caching.SiteCacheKey);
            await ClearCache(Caching.AllSitesCacheKey);
        }

        public async Task ClearTokenCache()
        {
            await ClearCache(Caching.TokenCacheKey);
        }

        public async Task ClearUserCache()
        {
            await ClearCache(Caching.UsersCacheKey);
        }
    }
}