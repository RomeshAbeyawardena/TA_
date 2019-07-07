using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Contracts.Services;
using TA.Domains.Models;
using Permission = TA.Contracts.Services.Permission;

namespace TA.App.Controllers.Api
{
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, RequiresApiKey(Permission.Read)]
        public async Task<ActionResult> GetUsers()
        {
            var users = await Users;
            return Ok(users);
        }

        [HttpGet, RequiresApiKey(Permission.Read)]
        public async Task<ActionResult> AuthenticateUser([FromBody] AuthenticateUserViewModel authenticateUserViewModel)
        {
            var users = await Users;
            if (_userService.IsValid(users, authenticateUserViewModel.EmailAddress,
                authenticateUserViewModel.Password))
                return Ok(_userService.GetUser(users, authenticateUserViewModel.EmailAddress));

            return BadRequest();
        }

        [HttpPost, RequiresApiKey(Permission.Create, Permission.Update, Permission.SoftDelete)]
        public async Task<ActionResult> SaveUser([FromBody] UserViewModel userViewModel)
        {
            var users = await Users;
            var mappedUser = Map<UserViewModel, User>(userViewModel);
            if (userViewModel.Id == default 
                && _userService.GetUser(users, userViewModel.UserToken) != null
                || _userService.GetUser(users, userViewModel.EmailAddress) != null)
                return BadRequest();

            var user = await _userService.SaveUser(mappedUser);
            await ClearUserCache();

            return Ok(user);
        }
    }
}