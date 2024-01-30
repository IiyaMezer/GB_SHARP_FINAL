using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MessagesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
    }
}
