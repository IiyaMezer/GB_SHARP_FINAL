using AutoMapper;
using WebApiLib.DataStore.Entity;

namespace MessagesApi.Mapper
{
    public partial class MessageProfile : Profile
    {
        public MessageProfile() 
        {
            CreateMap<MessageEntity, MessageModel>().ConvertUsing(new EntityToModelConverter());
        }

    }
}
