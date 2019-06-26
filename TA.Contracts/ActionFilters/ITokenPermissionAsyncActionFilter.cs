using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TA.Contracts.ActionFilters
{
    public interface ITokenPermissionAsyncActionFilter<TToken, in TPermission>
    {
        Task<bool> IsValid(HttpContext httpContext, TToken token);
        bool HasPermissions(HttpContext httpContext, TToken token , IEnumerable<TPermission> permissions);
    }
}