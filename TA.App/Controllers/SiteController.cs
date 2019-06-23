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
            var savedSite = await _siteService.SaveSite(Map<SiteViewModel, Site>(site));
            return Ok(savedSite);
        }
    }
}