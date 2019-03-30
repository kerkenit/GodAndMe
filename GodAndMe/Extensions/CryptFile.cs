using System;
using System.Text;

namespace GodAndMe.Extensions
{
    public static class CryptFile
    {
        /// <summary>
        /// Encrypt the specified plain string.
        /// </summary>
        /// <returns>The encrypt string.</returns>
        /// <param name="plainString">Plain string.</param>
        public static string Encrypt(string plainString)
        {
            byte[] plain = Encoding.UTF8.GetBytes(CommonFunctions.CRYPTOKEY[0] + plainString + CommonFunctions.CRYPTOKEY[1]);
            return Convert.ToBase64String(plain);
        }

        /// <summary>
        /// Decrypt the specified encrypted string.
        /// </summary>
        /// <returns>The decrypt string.</returns>
        /// <param name="encryptedString">Encrypted string.</param>
        public static string Decrypt(string encryptedString)
        {
            byte[] encrypted = Convert.FromBase64String(encryptedString);
            var json = Encoding.UTF8.GetString(encrypted, 0, encrypted.Length).Remove(0, CommonFunctions.CRYPTOKEY[0].Length);
            return json.Remove(json.Length - CommonFunctions.CRYPTOKEY[1].Length, CommonFunctions.CRYPTOKEY[1].Length);
        }
    }
}
