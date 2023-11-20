using System.Security.Cryptography;

namespace BlazorApp2.Client.Helper
{
    public class SecurityHelper
    {
        public static string GetKeyStrFromPassword(string password)
        {
            var key= GetKeyFromPassword(password);
            string keyStr = BitConverter.ToString(key).Replace("-", "");
            return keyStr;
        }
        public static byte[] GetKeyFromPassword(string password)
        {
            int iterations = 1;
            int keySize = 256;
            byte[] key = null;
            var iv = new byte[] { 49, 128, 63, 92, 141, 94, 111, 212, 150, 4, 110, 154, 190, 143, 138, 207 };
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, iv, iterations, HashAlgorithmName.SHA256))
            {
                key = pbkdf2.GetBytes(keySize / 8);
            }
            return key;
        }

        //------------------------------------------------------------------------


    }
}
