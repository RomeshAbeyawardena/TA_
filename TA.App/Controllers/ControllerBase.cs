using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts.Services;
using TA.Domains.Constants;
using TA.Domains.Exceptions;
using TA.Domains.Models;
using WebToolkit.Contracts.Providers;
using Permission = TA.Domains.Models.Permission;

namespace TA.App.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public TServiceImplementation GetRequiredService<TServiceImplementation>()
        {
            if (HttpContext == null)
                throw new InvalidOperationException("HttpContext unavailable.");
            return HttpContext
                .RequestServices.GetRequiredService<TServiceImplementation>();
        }

        private IMapperProvider MapperProvider => GetRequiredService<IMapperProvider>();

        private async Task<T> LoadAsync<T>(CacheType cacheType, string key, Func<Task<T>> loader)
        {
            var cacheProvider = GetRequiredService<ICacheProvider>();
            var result = await cacheProvider.Get<T>(cacheType, key);

            if (result != null)
                return result;

            result = await loader();
            await cacheProvider.Set(cacheType, key, result);

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

        public Task<IEnumerable<Token>> Tokens
        {
            get
            {
                var tokenService = GetRequiredService<ITokenService>();

                var tokens = LoadAsync(CacheType.DistributedCache, Caching.TokenPermissionCacheKey, 
                    async () => await tokenService.GetTokens());

                return tokens;
            }
        }

        public Task<IEnumerable<Permission>> Permissions
        {
            get
            {
                var permissionService = GetRequiredService<IPermissionService>();
                var permissions = LoadAsync(CacheType.DistributedCache, Caching.PermissionsCacheKey,
                    async () => await permissionService.GetPermissions());

                return permissions;
            }
        }

        public Task<IEnumerable<Domains.Dtos.Site>> Sites
        {
            get
            {
                var siteService = GetRequiredService<ISiteService>();
                    var sites = LoadAsync(CacheType.DistributedCache, Caching.SiteCacheKey,
                        async () => await siteService.GetSites());
                    return sites;
            }
        }

        public TDestination Map<TSource, TDestination>(TSource source) => MapperProvider.Map<TSource, TDestination>(source);
        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) => MapperProvider.Map<TSource, TDestination>(source);
    }
}