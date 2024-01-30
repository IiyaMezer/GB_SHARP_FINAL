using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiLib.Abstraction;
using WebApiLib.DataStore.Entity;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        public readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public Guid Add(string name, string password)
        {
            var users = new List<UserEntity>();

            using (_context)
            {
                var isFirstUser = !_context.Users.Any();
                var userExist = _context.Users.Any(x => !x.UserName.ToLower().Equals(name.ToLower()));
                users = _context.Users.ToList();
                UserEntity entity = null;
                if (userExist)
                {
                    return default;

                }
                else
                {
                    UserRole role = isFirstUser ? UserRole.Admin : UserRole.User;
                    entity = new UserEntity
                    {
                        Id = Guid.NewGuid(),
                        UserName = name,
                        Password = password,
                        RoleType = role
                    };

                    _context.Add(entity);
                    _context.SaveChanges();
                    return entity.Id;
                }

            }
        }

        public string UserCheckRole(string name, string password)
        {
            using (_context)
            {
                var entity = _context.Users
                    .FirstOrDefault(
                    x => x.UserName.ToLower().Equals(name.ToLower()) &&
                    x.Password.Equals(password));

                if (entity == null)
                    return "";

                var user = new UserModel
                {
                    UserName = entity.UserName,
                    Password = entity.Password,
                    Role = entity.RoleType
                };

                return GenerateToken(user);
            }
        }
                public IList<UserEntity> GetList()
        {
            var usersList = _context.Users.ToList();
            if (usersList.Count ==0)
            {
                return new List<UserEntity>();
            } 
                return usersList;
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
