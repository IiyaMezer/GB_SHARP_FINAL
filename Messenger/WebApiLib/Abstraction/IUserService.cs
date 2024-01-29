

namespace WebApiLib.Abstraction
{
    public interface IUserService
    {
        public Guid UserAdd(string name, string password);
        public string UserCheckRole(string name, string password);
    }
}

