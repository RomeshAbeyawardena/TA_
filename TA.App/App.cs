using System;
using System.Threading.Tasks;
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

            var asset = await _assetService.SaveAsset(new Asset
            {
                Active = true,
                Attributes = DictionaryBuilder<string, object>.Create()
                    .Add("BannerId", "56755")
                    .Add("AndersiteId", "ahgf4567")
                    .ToJObject(),
                Created = DateTimeOffset.Now,
                Modified = DateTimeOffset.Now,
                Key = "FeaturedProducts2",
                RelativeUrl = "/Featured/Products",
                Site = site
            });

            return 0;
        }
    }
}