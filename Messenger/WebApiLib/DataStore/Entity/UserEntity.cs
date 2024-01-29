using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEM4_Swagger.DataStore.Entity
{
    public class UserEntity
    {
        public Guid Id {get; set;}
        public string UserName {get; set;}
        public string Password {get; set;}
        public UserRole RoleType {get; set;}
        public virtual Role Role { get; set;}

    }
}