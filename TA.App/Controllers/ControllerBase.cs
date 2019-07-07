using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using TA.Contracts.Services;
using TA.Domains.Constants;
using TA.Domains.Exceptions;
using TA.Domains.Models;
using WebToolkit.Contracts.Providers;
using Permission = TA.Domains.Models.Permission;

namespace TA.App.Controllers
{
    public abstract class ControllerBase : WebToolkit.Common.ControllerBase
    {
        private async Task<IEnumerable<TModel>> GetCacheAsync<TModel, TService>(string cacheKey, Func<TService, Task<IEnumerable<TModel>>> get)
        {
            var service = GetRequiredService<TService>();
            var result = await LoadAsync(CacheType.DistributedCache, cacheKey, async() =>
                await get(service));

            return result;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (!context.ModelState.IsValid)
                    throw new InvalidModelStateException(context.ModelState);

                base.OnActionExecuting(context);
            }
            catch (ArgumentException ex)
            {
                context.Result = BadRequest(ex);
            }
            catch (InvalidModelStateException invalidModelStateException)
            {
                context.Result = BadRequest(invalidModelStateException.ModelStateDictionary);
            }
        }

        public async Task ClearCache(string key)
        {
            var cacheProvider = GetRequiredService<ICacheProvider>();

            await cacheProvider.ClearByKey(key);
        }

        public Task<IEnumerable<Token>> Tokens => 
            GetCacheAsync<Token, ITokenService>(Caching.TokenCacheKey, tokenService => tokenService.GetTokens());

        public Task<IEnumerable<Permission>> Permissions =>
            GetCacheAsync<Permission, IPermissionService>(Caching.PermissionsCacheKey,
                permissionService => permissionService.GetPermissions());

        public Task<IEnumerable<Site>> Sites =>
            GetCacheAsync<Site, ISiteService>(Caching.SiteCacheKey,
                siteService => siteService.GetSites());

        public Task<IEnumerable<Site>> AllSites =>
            GetCacheAsync<Site, ISiteService>(Caching.AllSitesCacheKey,
                siteService => siteService.GetSites(true));

        public Task<IEnumerable<User>> Users =>
            GetCacheAsync<User, IUserService>(Caching.UsersCacheKey, 
                userService => userService.GetUsers());
    }
}