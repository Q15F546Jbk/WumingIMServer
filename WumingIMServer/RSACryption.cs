using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.ComponentModel;

namespace WumingIMServer
{
    class RSACryption
    {
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        private string str;
        //UnicodeEncoding ByteConverter = new UnicodeEncoding();



        public RSACryption(string RSAXmlBase64)
        {
            provider = new RSACryptoServiceProvider();

            try
            {
                byte[] bytes = Convert.FromBase64String(RSAXmlBase64); 
                str = Encoding.Default.GetString(bytes);
            }
            catch (Exception e)
            {
                throw e;
            }
            
            provider.FromXmlString(str);

            
        }


        public string Encrypt(string DataToEncrypt)
        {
            try
            {
                byte[] Data = Encoding.Unicode.GetBytes(DataToEncrypt);
                byte[] encryptedData;
                encryptedData = provider.Encrypt(Data, false);
                return Convert.ToBase64String(encryptedData);
            }
            //Catch a Exception  

            catch (Exception e)
            {
                throw e;
            }
        }

        public string Decrypt(string DataToDecrypt)
        {
            try
            {
                byte[] Data = Convert.FromBase64String(DataToDecrypt);
                byte[] decryptedData;
                //Decrypt the passed string
                decryptedData = provider.Decrypt(Data, false);
                return Encoding.Unicode.GetString(decryptedData);
            }
            //Catch a Exception  

            catch (Exception e)
            {
                throw e;
            }
        }
    }

}

