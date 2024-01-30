using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserApi.Services;
using WebApiLib.Abstraction;
using WebApiLib.DataStore.Entity;




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
        public  IActionResult Login(string login, string password)
        {
            var token = _userService.UserCheckRole(login, password);
            if (!token.IsNullOrEmpty())
                return Ok(token);

            return NotFound("User not found");
        }

        [HttpPost("register")]
        public ActionResult<Guid> Login([FromBody] LoginModel model)
        {
            var userId = _userService.UserAdd(model);

            if(userId.Equals(default))
            {
                return BadRequest("User already exists");
            }
            return userId;
        }

        [HttpPost("list")]
        public IActionResult<> GetUsersList([FromBody] LoginModel model)
        {
            var usersList = _userService.GetList();

            if(usersList.Count == 0)
            {
                return BadRequest("No users found");
            }
            return Ok(usersList);
        }

        [HttpDelete("delete/(userToDeleteName)")]
        public IActionResult DeleteUser(string userToDeleteName, [FromBody] LoginModel model){
            bool isDeleted = _userService.Delete(model.UserName, model.Password, userToDeleteName);

            if (!isDeleted)
            {
                return BadRequest("User not found, cannot be deleted, or insufficient permissions");
            }
            return Ok("User deleted");
        }

            }
}
