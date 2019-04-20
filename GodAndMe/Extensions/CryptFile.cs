using System;
using System.IO;
using System.Net;
using System.Text;
using PCLCrypto;

namespace GodAndMe.Extensions
{
    public static class CryptFile
    {
        /// <summary>    
        /// Creates Salt with given length in bytes.    
        /// </summary>    
        /// <param name="lengthInBytes">No. of bytes</param>    
        /// <returns></returns>    
        public static byte[] CreateSalt(int lengthInBytes)
        {
            return WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
        }
        /// <summary>    
        /// Creates static Salt with given length in bytes.    
        /// </summary>    
        /// <returns></returns>    
        public static byte[] CreateStaticSalt()
        {
            return WinRTCrypto.CryptographicBuffer.ConvertStringToBinary("Kerk en IT", Encoding.UTF8);
        }

        /// <summary>    
        /// Creates a derived key from a comnination     
        /// </summary>    
        /// <param name="password"></param>    
        /// <param name="salt"></param>    
        /// <param name="keyLengthInBytes"></param>    
        /// <param name="iterations"></param>    
        /// <returns></returns>    
        public static byte[] CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 32)
        {
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            return key;
        }

        /// <summary>    
        /// Encrypts given data using symmetric algorithm AES    
        /// </summary>    
        /// <param name="data">Data to encrypt</param>    
        /// <param name="password">Password</param>    
        /// <param name="salt">Salt</param>    
        /// <returns>Encrypted Base64 string</returns>    
        public static string Encrypt(string data, string password = "G", byte[] salt = null)
        {
            if (salt == null)
            {
                salt = CreateStaticSalt();
            }
            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));
            return WebUtility.UrlEncode(Convert.ToBase64String(bytes));
        }
        /// <summary>    
        /// Decrypts given bytes using symmetric alogrithm AES    
        /// </summary>    
        /// <param name="data">data to decrypt</param>    
        /// <param name="password">Password used for encryption</param>    
        /// <param name="salt">Salt used for encryption</param>    
        /// <returns></returns>    
        public static string Decrypt(string data, string password = "G", byte[] salt = null)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(WebUtility.UrlDecode(data).Replace(" ", "+"));

            if (salt == null)
            {
                salt = CreateStaticSalt();
            }
            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, base64EncodedBytes);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Encrypt the specified plain string.
        /// </summary>
        /// <returns>The encrypt string.</returns>
        /// <param name="plainString">Plain string.</param>
        [Obsolete("Use Encrypt(string data, string password = \"G\", byte[] salt = null)", false)]
        public static string Encrypt_Legacy(string plainString)
        {
            byte[] plain = Encoding.UTF8.GetBytes(CommonFunctions.CRYPTOKEY[0] + plainString + CommonFunctions.CRYPTOKEY[1]);
            return WebUtility.UrlEncode(Convert.ToBase64String(plain));
        }

        /// <summary>
        /// Decrypt the specified encrypted string.
        /// </summary>
        /// <returns>The decrypt string.</returns>
        /// <param name="encryptedString">Encrypted string.</param>
        [Obsolete("Use Decrypt(string data, string password = \"G\", byte[] salt = null)", false)]
        public static string Decrypt_Legacy(string encryptedString)
        {
            byte[] encrypted = Convert.FromBase64String(WebUtility.UrlDecode(encryptedString));
            var json = Encoding.UTF8.GetString(encrypted, 0, encrypted.Length).Remove(0, CommonFunctions.CRYPTOKEY[0].Length);
            return json.Remove(json.Length - CommonFunctions.CRYPTOKEY[1].Length, CommonFunctions.CRYPTOKEY[1].Length);
        }
    }
}
