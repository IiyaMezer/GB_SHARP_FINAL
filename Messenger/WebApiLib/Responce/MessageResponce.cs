using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib.DataStore.Entity;

namespace WebApiLib.Responce
{
    public class MessageResponce
    {
        public bool IsSuccess { get; set; }
        public List<MessageModel> Messages { get; set; }
        public List<ErrorModel> Errors { get; set; }
    }
}
