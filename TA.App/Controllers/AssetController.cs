using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Contracts;
using TA.Domains.Dtos;

namespace TA.App.Controllers
{
    public class AssetController : ControllerBase
    {
        private readonly ISiteService _siteService;
        private readonly IAssetService _assetService;

        public AssetController(ISiteService siteService, IAssetService assetService)
        {
            _siteService = siteService;
            _assetService = assetService;
        }

        [HttpPost, RequiresApiKey]
        public async Task<ActionResult> SaveAsset([FromBody] AssetViewModel asset)
        {
            var mappedAsset = Map<AssetViewModel, Asset>(asset);

            mappedAsset.Site = await _siteService.GetSite(asset.SiteName);

            return Ok(
                await _assetService.SaveAsset(mappedAsset));
        }
    }
}