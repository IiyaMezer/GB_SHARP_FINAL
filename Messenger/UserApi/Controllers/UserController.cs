using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserApi.Services;
using WebApiLib.Abstraction;




namespace UserApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(IUserService userService)
        {
            _userService = (UserService?)userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string login, string password)
        {
            var token = _userService.UserCheckRole(login, password);
            if (!token.IsNullOrEmpty())
                return Ok(token);

            return NotFound("User not found");
        }
    }
}
