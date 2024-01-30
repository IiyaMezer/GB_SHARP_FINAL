using AutoMapper;
using WebApiLib.DataStore.Entity;

namespace UserApi
{
    public partial class UserProfile
    {
        private class EntityToModelConverter : ITypeConverter<UserEntity, UserModel>
        {
            public UserModel Convert(UserEntity source, UserModel destination, ResolutionContext context)
            {
                return new UserModel
                {
                    Id = source.Id,
                    UserName = source.UserName,
                    Role = source.RoleType.Role
                };
            }
        }

    }
}
