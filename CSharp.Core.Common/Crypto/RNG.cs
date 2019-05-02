using System;
using System.Security.Cryptography;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 亂數產生協助靜態類別 
    /// </summary>
    public static class RNG
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];
        /// <summary>
        /// 產生一個非負數的亂數
        /// </summary>
        /// <returns>非負數的亂數</returns>
        public static int Next()
        {
            rngCsp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary>
        /// 產生一個非負數且最大值 max 以下的亂數
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>非負數且最大值 max 以下的亂數</returns>
        public static int Next(int max)
        {
            rngCsp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary>
        /// 產生一個非負數且最小值在 min 以上最大值在 max 以下的亂數
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>非負數且最小值在 min 以上最大值在 max 以下的亂數</returns>
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
    }
}
