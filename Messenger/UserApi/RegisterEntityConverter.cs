using AutoMapper;
using WebApiLib.DataStore.Entity;

namespace UserApi
{
    public partial class UserProfile
    {
        private class RegisterEntityConverter : ITypeConverter<LoginModel, UserEntity>
        {
           public UserEntity Convert (LoginModel loginModel, UserEntity userEntity, ResolutionContext resolutionContext)
            {
                var entity = new UserEntity 
                {
                    Id = Guid.NewGuid(),
                    UserName = loginModel.Name,
                    Password = loginModel.Password
                };
                
                return entity;
            }
        }

    }
}
