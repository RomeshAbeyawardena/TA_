using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TA.Domains.Extensions;
using TA.Domains.Models;
using WebToolkit.Common.Builders;
using WebToolkit.Contracts.Builders;

namespace TA
{
    public class App
    {
        private readonly ISiteService _siteService;

        public App(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<int> Begin()
        {
            var site = await _siteService.GetSite();
            return 0;
        }
    }
}