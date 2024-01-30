using AutoMapper;
using System.Text;
using WebApiLib;
using WebApiLib.DataStore.Entity;

namespace UserApi.Mapper;

public partial class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<UserEntity, UserModel>().ConvertUsing(new EntityToModelConverter());
        CreateMap<UserEntity, Account>(MemberList.Destination);
        CreateMap<LoginModel, UserEntity>().ConvertUsing(new RegisterEntityConverter());
        CreateMap<LoginModel, Account>();
    }

}
