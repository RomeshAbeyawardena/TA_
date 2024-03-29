﻿using System.Threading.Tasks;
using TA.Contracts.Services;

namespace TA.App
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
            return await Task.FromResult(1);
        }
    }
}