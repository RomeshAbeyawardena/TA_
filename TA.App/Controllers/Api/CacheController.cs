﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.Contracts.Providers;

namespace TA.App.Controllers.Api
{
    public class CacheController : ApiControllerBase
    {
        private readonly ICacheProvider _cacheProvider;

        [HttpGet]
        public ActionResult Clear()
        {
            return Ok(_cacheProvider.Clear());
        }

        public CacheController(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }
    }
}