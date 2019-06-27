using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Contracts.ActionFilters;
using TA.Contracts.Providers;
using TA.Domains.Constants;
using TA.Domains.Models;
using Permission = TA.Contracts.Permission;

namespace TA.App.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresApiKeyAttribute : Attribute, ITokenPermissionAsyncActionFilter<Token, Permission>
    {
        private readonly Permission[] _permissions;

        private readonly Func<IServiceProvider, ITokenService> _getTokenService;
        private readonly Func<IServiceProvider, ICacheProvider> _getCacheProvider;
        public RequiresApiKeyAttribute(params Permission[] permissions)
        {
            _getTokenService = services => services.GetRequiredService<ITokenService>();
            _permissions = permissions;
        }

        public override bool Match(object obj)
        {
            if (obj is RequiresApiKeyAttribute requiresApiKeyAttribute)
                return _permissions.All(a => requiresApiKeyAttribute._permissions.Contains(a));

            return false;
        }

        public override bool IsDefaultAttribute()
        {
            return true;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var token = await GetToken(context.HttpContext);
                if (token != null && await IsValid(context.HttpContext, token)
                                  && HasPermissions(context.HttpContext, token, _permissions))
                {
                    await next();
                }

                throw new UnauthorizedAccessException($"Access token requires the following permissions: {string.Join(",", _permissions)}");
            }
            catch (UnauthorizedAccessException unauthorisedAccessException)
            {
                context.Result = new UnauthorizedObjectResult(unauthorisedAccessException);
            }
        }

        public async Task<Token> GetToken(HttpContext httpContext)
        {
            var tokenService = _getTokenService(httpContext.RequestServices);
            httpContext.Request.Headers.TryGetValue(General.ApiKey, out var apiKey);

            return await tokenService.GetToken(apiKey);
        }

        public async Task<bool> IsValid(HttpContext httpContext, Token token)
        {
            var tokenService = _getTokenService(httpContext.RequestServices);
            return await tokenService.IsValid(token);
        }

        public bool HasPermissions(HttpContext httpContext, Token token, IEnumerable<Permission> permissions)
        {
            var tokenService = _getTokenService(httpContext.RequestServices);
            return tokenService.HasPermissions(token, permissions);
        }
    }
}