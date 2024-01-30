

using WebApiLib.DataStore.Entity;
using WebApiLib.Responce;

namespace WebApiLib.Abstraction
{
    public interface IUserService
    {
        public UserResponce UserAdd(LoginModel model);
        public bool Delete(string adminName, string adminPassword, string userToDeleteName);
        public Guid AddAdmin(LoginModel model);
        public UserResponce Authentification(LoginModel model);
        public List<UserModel> GetUsers();
        public UserEntity GetUser(Guid? userId, string? email);


    }
}

