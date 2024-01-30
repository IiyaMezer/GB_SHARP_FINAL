using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib.DataCheck;
using WebApiLib.DataStore.Entity;

namespace ApiTests
{
    [TestFixture]
    public class UserApiTests
    {
        [Test]
        public void AuthentificateUserNullTest()
        {
            UserEntity user = null;

            Assert.IsNull(user);
        }

        [Test]
        public void CheckWrongPassword()
        {
            Assert.IsTrue(!Password.Check("13568"));
        }
        [Test]
        public void CheckCorrectPassword()
        {
            Assert.IsTrue(Password.Check("1qazxsW2"));
        }

        [Test]
        public void CheckInCorrectEmail()
        {
            Assert.IsTrue(!UserName.Check("incorrect"));
        }
        [Test]
        public void CheckCorrectEmail()
        {
            Assert.IsTrue(UserName.Check("correct@mail.ru"));
        }


    }
}
