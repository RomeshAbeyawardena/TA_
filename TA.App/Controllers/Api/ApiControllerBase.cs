using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TA.App.Attributes;
using TA.Contracts;

namespace TA.App.Controllers.Api
{
    [Route("api/[controller]/[action]"), RequiresApiKey(Permission.ApiAccess)]
    public abstract class ApiControllerBase : ControllerBase
    {
        
    }
}