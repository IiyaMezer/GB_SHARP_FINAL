using AutoMapper;
using WebApiLib.DataStore.Entity;

namespace MessagesApi.Mapper
{
    public partial class MessageProfile
    {
        private class EntityToModelConverter : ITypeConverter<MessageEntity, MessageModel>
        {
            public MessageModel Convert(MessageEntity source, MessageModel destination, ResolutionContext context)
            {
                return new MessageModel
                {
                    Id = source.Id,
                    CreatedAt = source.CreatedAt,
                    IsRead = source.IsRead,
                    RecipientEmail = source.Recipient.UserName,
                    SenderEmail = source.Sender.UserName,
                    Text = source.Text

                };
            }
        }

    }
}
