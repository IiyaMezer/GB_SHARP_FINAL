using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib.DataStore.Entity;

namespace WebApiLib
{
    public class Account : UserModel
    {
        private string? _token;

        public string RefreshToken(string token)
        {
            _token = token;
            return _token;
        }

        public string? GetToken() => _token;

        public void Login (UserModel model)
        {
            Id = model.Id;
            UserName = model.UserName;
            Password = model.Password;
            Role = model.Role;
        }

        public void Logout () 
        {
            Id = null;
            UserName = null;
            Password = null;
            Role = null;
            _token = null;
        }
    }
}
