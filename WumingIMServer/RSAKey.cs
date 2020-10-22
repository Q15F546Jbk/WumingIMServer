using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WumingIMServer
{
    class RSAInfo
    {

        public string info;
        public RSAInfo(Int32 KeyLen)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(KeyLen);
            info = provider.ToXmlString(true);
            
        }

        public string Export()
        {
            byte[] bytes = Encoding.Default.GetBytes(info);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
    }
}
