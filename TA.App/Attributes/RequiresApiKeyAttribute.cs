using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Constants;

namespace TA.App.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private readonly Permission[] _permissions;

        public RequiresApiKeyAttribute(params Permission[] permissions)
        {
            _permissions = permissions;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            var tokenService = httpContext.RequestServices.GetRequiredService<ITokenService>();

            httpContext.Request.Headers.TryGetValue(General.ApiKey, out var apiKey);

            var token = await tokenService.GetToken(apiKey);

            if (token != null && await tokenService.IsValid(token)
                && await tokenService.HasPermissions(token, _permissions))
            {
                await next();
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}