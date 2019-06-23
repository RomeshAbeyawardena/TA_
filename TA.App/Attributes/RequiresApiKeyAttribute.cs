using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TA.Contracts;
using TA.Domains.Constants;

namespace TA.App.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresApiKeyAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var applicationSettings =  context.HttpContext.RequestServices.GetRequiredService<IApplicationSettings>();
            context.HttpContext.Request.Headers.TryGetValue(General.ApiKey, out var apiKey);

            if(apiKey.All(value => value != applicationSettings.ApiKey))
                context.Result = new UnauthorizedResult();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}