using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Contracts;
using TA.Domains.Dtos;

namespace TA.App.Controllers.Api
{
    public class SiteController : ApiControllerBase
    {
        private readonly ISiteService _siteService;

        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpPost, RequiresApiKey(Permission.Create, Permission.Update)]
        public async Task<ActionResult> SaveSite([FromBody] SiteViewModel site)
        {
            var mappedSite = Map<SiteViewModel, Site>(site);
            var savedSite = await _siteService.SaveSite(mappedSite);
            return Ok(savedSite);
        }

        [HttpGet, RequiresApiKey(Permission.Read)]
        public async Task<ActionResult> GetSites(GetSitesViewModel getSitesViewModel)
        {
            return Ok(await Sites);
        }
    }
}