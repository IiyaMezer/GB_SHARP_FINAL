

using WebApiLib.DataStore.Entity;

namespace WebApiLib.Abstraction
{
    public interface IUserService
    {
        public Guid UserAdd(LoginModel model);
        public bool Delete(string adminName, string adminPassword, string userToDeleteName);
        public Guid AddAdmin(LoginModel model);

    }
}

