using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLib.DataStore.Entity
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual RoleEntity RoleType { get; set; }
        public virtual List<MessageEntity> SendMessages { get; set; }
        public virtual List<MessageEntity> ReceiveMessages { get; set; }

    }
}