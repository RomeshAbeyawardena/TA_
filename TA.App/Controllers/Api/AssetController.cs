﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Contracts.Services;
using TA.Domains.Models;
using Permission = TA.Contracts.Services.Permission;

namespace TA.App.Controllers.Api
{
    public class AssetController : ApiControllerBase
    {
        private readonly ISiteService _siteService;
        private readonly IAssetService _assetService;

        private async Task<Site> GetSiteByName(string name)
        {
            return _siteService.GetSite(await Sites, name);
        }

        public AssetController(ISiteService siteService, IAssetService assetService)
        {
            _siteService = siteService;
            _assetService = assetService;
        }

        [HttpPost, RequiresApiKey(Permission.Create, Permission.Update)]
        public async Task<ActionResult> SaveAsset([FromBody] AssetViewModel asset)
        {
            var mappedAsset = Map<AssetViewModel, Asset>(asset);

            mappedAsset.Site = await GetSiteByName(asset.SiteName);

            return Ok(
                await _assetService.SaveAsset(mappedAsset));
        }

        [HttpGet, RequiresApiKey(Permission.Read)]
        public async Task<ActionResult> GetAssets(GetAssetsViewModel getAssetsViewModel)
        {
            var site = await GetSiteByName(getAssetsViewModel.SiteName);
            return Ok(await _assetService.GetAssets(site, getAssetsViewModel.ShowAll));
        }
    }
}