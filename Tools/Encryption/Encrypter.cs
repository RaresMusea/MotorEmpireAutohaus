using System.Security.Cryptography;
using System.Text;

namespace MotorEmpireAutohaus.Tools.Encryption
{
    public class Encrypter
    {
        public static string EncryptPassword(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            using (MD5 md5 = MD5.Create())
            {
                data = md5.ComputeHash(data);
            }

            return Convert.ToBase64String(data);
        }

        public static bool ValidateMatch(string input, string encrypted)
        {
            return EncryptPassword(input) == encrypted;
        }
    }
}