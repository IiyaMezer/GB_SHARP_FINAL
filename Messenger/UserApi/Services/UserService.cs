using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiLib;
using WebApiLib.Abstraction;
using WebApiLib.DataStore.Entity;
using WebApiLib.Responce;

namespace UserApi.Services;

public class UserService : IUserService
{
    public readonly Func<AppDbContext> _context;
    private readonly Account _account;
    private readonly IMapper _mapper;

    public UserService(Func<AppDbContext> context, Account account, IMapper mapper)
    {
        _context = context;
        _account = account;
        _mapper = mapper;
    }

    public UserResponce UserAdd(LoginModel model)
    {
        var users = new List<UserEntity>();
        var responce = UserResponce.OK();
        using (var context = _context())
        {
            
            var userExist = context.Users.Any(x => !x.UserName.ToLower().Equals(model.Name.ToLower()));
            users = context.Users.ToList();            
            if (userExist)
            {
                return UserResponce.UserExist(); ;

            }
            else
            {


                var entity = _mapper.Map<UserEntity>(model);
                entity.RoleType = new RoleEntity { Role = UserRole.User };

                context.Add(entity);
                context.SaveChanges();
                responce.UserId = entity.Id;
                return responce;
            }
        }
    }
    public Guid AddAdmin(LoginModel model)
    {
        var users = new List<UserEntity>();

        using (var context = _context())
        {
            var userExist = context.Users.Any(x => x.UserName.ToLower().Equals(model.Name.ToLower()));
            users = context.Users.ToList();
            UserEntity entity = null;
            if (userExist)
            {
                return default;

            }
            else
            {
                 entity = _mapper.Map<UserEntity>(model);
                entity.RoleType = new RoleEntity
                {
                    Role = UserRole.Admin
                };
                

                context.Users.Add(entity);
                context.SaveChanges();
                return entity.Id;
            }

        }
    }

    public bool Delete(string adminName, string adminPassword, string userToDeleteName)
    {
        if (!_account.Role.Equals(UserRole.Admin))
        {
            return false;
        }
        using (var context = _context())
        {
            var query = context.Users.Include(x => x.RoleType).AsQueryable();
            if (!string.IsNullOrEmpty(userToDeleteName))
            {
                query = query.Where(x => x.UserName == userToDeleteName);
            }

            var exist = query.FirstOrDefault();

            if (exist == null) 
            {
                return false;
            }
            if (exist.RoleType.Role == UserRole.Admin)
            {
                return false;
            }
            context.Users.Remove(exist);
            context.SaveChanges();
            return true;
        }
    }
    public UserResponce Authentification(LoginModel model)
    {
        UserEntity user = null;
        using (var context = _context())
        {
            user = context.Users.Include(x => x.RoleType)
                .FirstOrDefault(x => x.UserName == model.Name);
            if (user is null)
            {
                return UserResponce.UserNotFound();
            }
            if (PasswordValidation(model.Password, user.Password)) 
            {
                var responce = UserResponce.Ok();
                responce.Users.Add(_mapper.Map<UserModel>(user));
                return responce;
            }

            return UserResponce.WrongPassword();
        }
    }

    public List<UserModel> GetUsers()
    {
        var users = new List<UserModel>();
        if (!_account.Role.Equals(UserRole.Admin))
        {
            return null;
        }
        using (var context = _context())
        {
            users.AddRange(context.Users.Include(x=> x.RoleType)
                .Select(x=> _mapper.Map<UserModel>(x)).ToList());
            return users;
        }
    }

    public UserEntity GetUser(Guid? userId, string? email)
    {
        var user = new UserEntity();
        using (var context = _context())
        {
            var query = context.Users.Include(x => x.RoleType).AsQueryable();
            if (!string.IsNullOrEmpty(email))
                query = query.Where(x => x.UserName == email);
            if (userId.HasValue)
                query = query.Where(x => x.Id == userId);

            user = query.FirstOrDefault();
        }
        if (user == null)
            return null;
        if (_account.Role == UserRole.Admin || _account.Id == userId)
            return user;
        return null;
    }

    private static bool PasswordValidation(string password1, string password2)
    {
        return password1 ==password2;
    }


}

