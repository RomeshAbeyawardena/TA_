using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TA.Contracts;
using TA.Domains.Extensions;
using TA.Domains.Models;
using WebToolkit.Common.Builders;

namespace TA
{
    public class App
    {
        private readonly ISiteService _siteService;
        private readonly IAssetService _assetService;

        public App(ISiteService siteService, IAssetService assetService)
        {
            _siteService = siteService;
            _assetService = assetService;
        }

        public async Task<int> Begin()
        {
            var site = await _siteService.GetSite("mysite");
            site.Attributes = site.Attributes.Append("Environment", "Development");
            site = await _siteService.SaveSite(site, false);
            var asset = await _assetService.GetAsset(site, "FeaturedProducts");
            
            asset.Attributes = asset.Attributes.Append("ReviewSubscriptionId", 2234611242);;
            asset = await _assetService.SaveAsset(asset);

            return 0;
        }
    }
}