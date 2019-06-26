using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TA.Contracts.ActionFilters
{
    public interface ITokenPermissionAsyncActionFilter<TToken, in TPermission> : IAsyncActionFilter
    {
        Task<bool> IsValid(HttpContext httpContext, TToken token);
        bool HasPermissions(HttpContext httpContext, TToken token , IEnumerable<TPermission> permissions);
    }
}