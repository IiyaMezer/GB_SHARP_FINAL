using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib.DataStore.Entity;

namespace WebApiLib.Responce
{
    public class UserResponce
    {
        public bool IsSuccess { get; set; }
        public List<ErrorModel> Errors = new();
        public List<UserModel> Users = new();
        public Guid? UserId { get; set; }

        public static UserResponce Ok()
        {
            return new UserResponce
            {
                IsSuccess = true,
            };
        }
        public static UserResponce UserNotFound()
        {
            return new UserResponce
            {
                IsSuccess = false,
                Errors = new List<ErrorModel> {
                    new() {
                        Message = "User not found",
                        StatusCode = 404
                    }
                }
            };
        }

        public static UserResponce WrongPassword()
        {
            return new UserResponce
            {
                IsSuccess = false,
                Errors = new List<ErrorModel> {
                    new() {
                        Message = "Wrong password",
                    }
                }
            };
        }
        public static UserResponce UserExist()
        {
            return new UserResponce
            {
                IsSuccess = false,
                Errors = new List<ErrorModel> {
                    new ErrorModel
                    {
                        Message = "User already exists",
                        StatusCode = 409
                    }
                }
            };
        }
    }
}
