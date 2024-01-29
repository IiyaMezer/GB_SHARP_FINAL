using SEM4_Swagger.DataStore.Entity;

namespace SEM4_Swagger.Abstraction
{
    public interface IUserService
    {
        public Guid UserAdd(string name, string password);
        public string UserCheckRole(string name, string password);
    }
}

