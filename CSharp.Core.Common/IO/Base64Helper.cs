using System;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Bass64協助靜態類別,如Bytes To Base64 String 反之
    /// </summary>
    public static class Base64Helper
    {
        /// <summary>
        /// Bytes To Base64 String
        /// </summary>
        /// <param name="bytes">位元組</param>
        /// <returns>Base64 String</returns>
        public static string BytesToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// Base64 String To Bytes
        /// </summary>
        /// <param name="base64String">Base64 String</param>
        /// <returns>位元組</returns>
        public static byte[] Base64StringToBytes(this string base64String)
        {
            return Convert.FromBase64String(base64String); 
        }
    }
}
