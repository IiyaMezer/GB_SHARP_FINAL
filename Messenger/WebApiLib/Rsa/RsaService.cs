using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLib.Rsa
{
    public static class RsaService
    {
        public static RSA GetPublicKey()
        {
            var key = File.ReadAllText(@"..\WebApiLib\Rsa\public_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(key);
            return rsa;
        }

        public static RSA GetPrivateKey()
        {
            var key = File.ReadAllText(@"..\WebApiLib\Rsa\private_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(key);
            return rsa;
        }
    }
}
