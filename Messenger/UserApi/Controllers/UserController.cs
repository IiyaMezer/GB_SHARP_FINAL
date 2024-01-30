using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using UserApi.Services;
using WebApiLib;
using WebApiLib.Abstraction;
using WebApiLib.DataStore.Entity;
using WebApiLib.Rsa;




namespace UserApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Account _account;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, Account account, IConfiguration configuration)
        {
            _userService = userService;
            _account = account;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public  IActionResult Login([Description("User Auth")][FromBody] LoginModel model)
        {
            if (ValidMail(model.Name))
                return BadRequest($"Email:{model.Name} - should be Email");

            if(_account.GetToken() is not null)
                return BadRequest("already logged in");

            var responce = _userService.Authentification(model);

            if (!responce.IsSuccess)
                return NotFound();

            _account.Login(responce.Users[0]);
            _account.RefreshToken(GenerateToken(_account));
            return Ok(_account.GetToken());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addUser")]
        public ActionResult AddUser([FromBody] LoginModel model)
        {
            
            if (ValidMail(model.Name) == false)
                return BadRequest($"Email:{model.Name} - should be Email");
            var userId = _userService.UserAdd(model);
            if (userId.Equals(default))
            {
                return BadRequest("User already exists");
            }
            return Ok(userId);
        }

        [AllowAnonymous]
        [HttpPost("addAdmin")]
        public ActionResult AddAdmin([FromBody] LoginModel model)
        {
            if (ValidMail(model.Name) == false)
                return BadRequest($"Email:{model.Name} - should be Email");
            var userId = _userService.AddAdmin(model);

            if (userId.Equals(default))
            {
                return BadRequest("User already exists");
            }
            return Ok(userId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("usersList")]
        public IActionResult GetUsers()
        {
            var usersList = _userService.GetUsers();

            if(usersList.Count == 0)
            {
                return BadRequest("No users found");
            }
            return Ok(usersList);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/(userToDeleteName)")]
        public IActionResult DeleteUser(string userToDeleteName, [FromBody] LoginModel model){
            bool isDeleted = _userService.Delete(model.Name, model.Password, userToDeleteName);

            if (!isDeleted)
            {
                return BadRequest("User not found, cannot be deleted, or insufficient permissions");
            }
            return Ok("User deleted");
        }

        [HttpPost("logout")]
        public ActionResult LogOut()
        {
            _account.Logout();
            return Ok();
        }

        private string GenerateToken(Account account)
        {
            var key = new RsaSecurityKey(RsaService.GetPrivateKey());
            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim("Id", account.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool ValidMail(string name)
        {
            try
            {
                MailAddress mail = new MailAddress(name);
                return true;
            }
            catch(FormatException) 
            {
                return false;
            }
        }

    }
}
