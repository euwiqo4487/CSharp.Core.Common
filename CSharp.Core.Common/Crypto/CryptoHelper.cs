using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 加密 協助靜態類別,使用AES 加解密 或  模擬驗章的功能  使用  SHA512
    /// </summary>
    public static class CryptoHelper
    {
        static readonly byte[] Key = { 0xA1, 0x02, 0xA3, 0x04, 0xA5, 0x06, 0xA7, 0x08, 0xA9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        //static readonly byte[] Key256 = { 0xA1, 0x02, 0xA3, 0x04, 0xA5, 0x06, 0xA7, 0x08, 0xA9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0xA1, 0x02, 0xA3, 0x04, 0xA5, 0x06, 0xA7, 0x08, 0xA9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        static readonly byte[] IV = { 0xF1, 0x02, 0xF3, 0x04, 0xF5, 0x06, 0xF7, 0x08, 0xF9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        /// <summary>
        /// 加密字串,使用AES
        /// </summary>
        /// <example>
        /// <code language="cs" title="加解密">
        /// string EText = "Hello World!!".EncryptData();
        /// Console.WriteLine("加密:" + EText);
        /// Console.WriteLine("解密後:" + EText.DecryptData());
        /// </code>
        /// </example>
        /// <param name="plainText">被加密對象</param>
        /// <returns>加密後字串</returns>
        public static string EncryptData(this string plainText)
        {   // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText is null or Empty!!");            

            byte[] encrypted;
            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {   //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }
        /// <summary>
        /// 解密字串,使用AES
        /// </summary>
        /// <param name="encryptString">解密對象</param>
        /// <returns>解密後字串</returns>
        public static string DecryptData(this string encryptString)
        {   // Check arguments.
            if (encryptString == null || encryptString.Length <= 0)
                throw new ArgumentNullException("encryptString is null or Empty!!");

            // Declare the string used to hold the decrypted text.
            string plaintext = null;
            byte[] cipherText = Convert.FromBase64String(encryptString);
            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {  // Read the decrypted bytes from the decrypting  stream and place them in a string.
                           plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        /// <summary>
        /// Hash an input string and return the hash as
        /// a 32 character hexadecimal string.模擬驗章的功能  使用  SHA512
        /// </summary>
        /// <example>
        /// <code language="cs" title="簽章和驗章">
        /// string input = "Hello World!!".GetHashData();
        /// Console.WriteLine(input);
        /// Console.WriteLine("Hello World!!".VerifyHashData(input)); //成功 true
        /// </code>
        /// </example>
        /// <param name="input">輸入</param>
        /// <returns>a 32 character hexadecimal string.(32字元16進制)</returns>
        public static string GetHashData(this string input)
        {
            SHA512 shaM = new SHA512Managed();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaM.ComputeHash(Encoding.Default.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        /// <summary>
        /// 模擬驗章的功能   Verify a hash against a string.  使用  SHA512
        /// </summary>
        /// <param name="input">比對的內容</param>
        /// <param name="hash">雜湊值 (Hash Value)</param>
        /// <returns>true:驗章成功 false:驗章失敗</returns>
        public static bool VerifyHashData(this string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetHashData(input);
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
