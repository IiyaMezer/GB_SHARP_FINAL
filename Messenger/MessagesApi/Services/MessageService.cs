using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using WebApiLib;
using WebApiLib.Abstraction;
using WebApiLib.DataStore.Entity;
using WebApiLib.Responce;

namespace MessagesApi.Services
{
    public class MessageService : IMessageService
    {
        public readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MessageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public MessageResponce GetNewMessages(string recipientEmail)
        {
            var responce = new MessageResponce();
            using(var context = _context)
            {
                var messages = context.Messages
                    .Include(m=> m.Recipient)
                    .Include(m => m.Sender)
                    .Where(m=> m.Recipient.UserName == recipientEmail && !m.IsRead).ToList();
                foreach (var message in messages)
                {
                    message.IsRead = true;
                }
                context.UpdateRange(messages);
                context.SaveChanges();

                responce.Messages.AddRange(messages.Select(_mapper.Map<MessageModel>));
                responce.IsSuccess = true;
            }
            return responce;
        }

        public MessageResponce SendMessage(MessageModel model)
        {
            var responce = new MessageResponce();
            using (var context = _context)
            {
                var message = _mapper.Map<MessageEntity>(model);
                context.Messages.Add(message);
                context.SaveChangesAsync();

                responce.Messages.Add(model);
                responce.IsSuccess = true;
            }
            return responce;
        }
    }
}
