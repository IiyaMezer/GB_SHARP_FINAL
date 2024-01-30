using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiLib;
using WebApiLib.Abstraction;
using WebApiLib.DataStore.Entity;

namespace UserApi.Services;

public class UserService : IUserService
{
    public readonly AppDbContext _context;
    private readonly Account _account;
    private readonly IMapper _mapper;

    public UserService(AppDbContext context, Account account, IMapper mapper)
    {
        _context = context;
        _account = account;
        _mapper = mapper;
    }

    public Guid UserAdd(LoginModel model)
    {
        var users = new List<UserEntity>();

        using (_context)
        {
            var isFirstUser = !_context.Users.Any();
            var userExist = _context.Users.Any(x => !x.UserName.ToLower().Equals(model.Name.ToLower()));
            users = _context.Users.ToList();
            UserEntity entity = null;
            if (userExist)
            {
                return default;

            }
            else
            {
                if (isFirstUser)
                {
                    AddAdmin(model);
                }
               
                entity = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    UserName = model.Name,
                    Password = model.Password,
                    RoleType = new RoleEntity { Role = UserRole.User}
                };

                _context.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }
    }
    public Guid AddAdmin(LoginModel model)
    {
        var users = new List<UserEntity>();

        using (_context)
        {
            var userExist = _context.Users.Any(x => !x.UserName.ToLower().Equals(model.Name.ToLower()));
            users = _context.Users.ToList();
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
                

                _context.Users.Add(entity);
                _context.SaveChanges();
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
        using (_context)
        {
            var query = _context.Users.Include(x => x.RoleType).AsQueryable();
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
            _context.Users.Remove(exist);
            _context.SaveChanges();
            return true;
        }
    }
    public bool Authentification(LoginModel model)
    {
        UserEntity user = null;
        using (_context)
        {
            user = _context.Users.Include(x => x.RoleType)
                .FirstOrDefault(x => x.UserName == model.Name);
            if (user is null)
            {
                return false;
            }
            if (PasswordValidation(model.Password, user.Password)) 
            {
                return true;
            }

            return false;
        }
    }

    public List<UserModel> GetUsers()
    {
        var users = new List<UserModel>();
        if (!_account.Role.Equals(UserRole.Admin))
        {
            return null;
        }
        using (_context)
        {
            users.AddRange(_context.Users.Include(x=> x.RoleType)
                .Select(x=> _mapper.Map<UserModel>(x)).ToList());
            return users;
        }
    }

    public UserEntity GetUser(Guid? userId, string? email)
    {
        var user = new UserEntity();
        using (_context)
        {
            var query = _context.Users.Include(x => x.RoleType).AsQueryable();
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

    private bool PasswordValidation(string password1, string password2)
    {
        return password1 ==password2;
    }


}

