using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEM4_Swagger.DataStore.Entity
{
    public class UserModel
    {
        public string UserName {get; set;}
        public string Password {get; set;}
        public UserRole Role {get; set;}
    }
}