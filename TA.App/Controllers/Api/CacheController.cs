using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.Contracts.Providers;

namespace TA.App.Controllers.Api
{
    public class CacheController : ApiControllerBase
    {
        private readonly ICacheProvider _cacheProvider;

        [HttpGet]
        public async Task<ActionResult> Clear()
        {
            await _cacheProvider.Clear();
            return Ok();
        }

        public CacheController(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }
    }
}