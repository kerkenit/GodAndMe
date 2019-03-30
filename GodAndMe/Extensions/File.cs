using System;
using System.IO;
using System.Text;
//using PCLCrypto;
//using static PCLCrypto.WinRTCrypto;

namespace GodAndMe.Extensions
{
    public static class CryptFile
    {
        //private ICryptographicKey key = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GodAndMe.Extensions.CryptFile"/> class.
        /// </summary>
        /// <param name="encryptionKey">Encryption key.</param>
        //public CryptFile(string[] encryptionKey = null)
        //{
        //            string privateKeyString = null, publicKeyString = null;
        //            key = Key(encryptionKey, ref privateKeyString, ref publicKeyString);

        //#if DEBUG
        //            System.Diagnostics.Debug.WriteLine(privateKeyString);
        //            System.Diagnostics.Debug.WriteLine(publicKeyString);
        //#endif
        // }


        //private ICryptographicKey Key(string[] encryptionKey, ref string privateKeyString, ref string publicKeyString)
        // {
        //            try
        //            {
        //                // create the key we are going to use
        //                IAsymmetricKeyAlgorithmProvider asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
        //                //IHashAlgorithmProvider hash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);

        //                if (encryptionKey == null || encryptionKey.Length < 2 || (string.IsNullOrEmpty(encryptionKey[0]) && string.IsNullOrEmpty(encryptionKey[1])))
        //                {
        //                    key = asym.CreateKeyPair(512);
        //                    byte[] publicKey = key.ExportPublicKey(CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);
        //                    privateKeyString = Convert.ToBase64String(key.Export());
        //                    publicKeyString = Convert.ToBase64String(publicKey);
        //                    return key;
        //                }
        //                else
        //                {
        //                    byte[] privateKey = Convert.FromBase64String(encryptionKey[0]);
        //                    byte[] publicKey = Convert.FromBase64String(encryptionKey[1]);
        //                    return asym.ImportKeyPair(privateKey);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                throw ex;
        //#else 
        //                return null;
        //#endif
        //}
        // }

        /// <summary>
        /// Encrypt the specified plain string.
        /// </summary>
        /// <returns>The encrypt string.</returns>
        /// <param name="plainString">Plain string.</param>
        public static string Encrypt(string plainString)
        {
            byte[] plain = Encoding.UTF8.GetBytes(CommonFunctions.CRYPTOKEY[0] + plainString + CommonFunctions.CRYPTOKEY[1]);
            return Convert.ToBase64String(plain);
            //            try
            //            {
            //                byte[] plain = Encoding.UTF8.GetBytes(plainString);
            //                byte[] encrypted = CryptographicEngine.Encrypt(key, plain);
            //                string encryptedString = Convert.ToBase64String(encrypted);

            //                return encryptedString;
            //            }
            //            catch (Exception ex)
            //            {
            //#if DEBUG
            //                throw ex;
            //#else 
            //                return null;
            //#endif
            //}
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

            //            try
            //            {
            //                byte[] encrypted = Convert.FromBase64String(encryptedString);
            //                byte[] decrypted = CryptographicEngine.Decrypt(key, encrypted);
            //                string decryptedString = Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);

            //                return decryptedString;
            //            }
            //            catch (Exception ex)
            //            {
            //#if DEBUG
            //                throw ex;
            //#else 
            //                return null;
            //#endif
            //}
        }
    }
}
