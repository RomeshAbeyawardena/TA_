using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Domains.Dtos;

namespace TA.App.Controllers
{
    public class SiteController : ControllerBase
    {
        private readonly ISiteService _siteService;

        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpPost, RequiresApiKey]
        public async Task<ActionResult> SaveSite([FromBody] SiteViewModel site)
        {
            var mappedSite = Map<SiteViewModel, Site>(site);
            var savedSite = await _siteService.SaveSite(mappedSite);
            return Ok(savedSite);
        }

        [HttpGet]
        public async Task<ActionResult> GetSites(GetSitesViewModel getSitesViewModel)
        {
            return Ok(await _siteService.GetSites(getSitesViewModel.ShowInActive));
        }
    }
}