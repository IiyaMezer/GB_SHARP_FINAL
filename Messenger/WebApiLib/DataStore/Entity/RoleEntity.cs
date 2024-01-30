using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLib.DataStore.Entity
{
    public class RoleEntity
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
        public virtual List<UserEntity>? Users { get; set; }
    }
}
