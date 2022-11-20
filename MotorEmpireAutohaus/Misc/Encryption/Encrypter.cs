using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Encryption
{
    public class Encrypter
    {
        public static string EncryptPassword(string password)
        {
            byte[] data= Encoding.UTF8.GetBytes(password);
            using (MD5 md5= MD5.Create())
            {
                data= md5.ComputeHash(data);
            } 
            return Convert.ToBase64String(data);
        }

        public static bool ValidateMatch(string input, string encrypted)
        {
            return EncryptPassword(input) == encrypted;
        }
    }
}
