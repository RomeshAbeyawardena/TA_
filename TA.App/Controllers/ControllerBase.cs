using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TA.Domains.Exceptions;
using WebToolkit.Contracts.Providers;

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

        public TDestination Map<TSource, TDestination>(TSource source) => MapperProvider.Map<TSource, TDestination>(source);
        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) => MapperProvider.Map<TSource, TDestination>(source);
    }
}