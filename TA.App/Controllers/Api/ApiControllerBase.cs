using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.Contracts.Services;

namespace TA.App.Controllers.Api
{
    [Route("api/[controller]/[action]"), RequiresApiKey(Permission.ApiAccess)]
    public abstract class ApiControllerBase : ControllerBase
    {
        
    }
}