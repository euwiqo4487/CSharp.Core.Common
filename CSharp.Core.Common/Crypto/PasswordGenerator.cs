using System;
using System.Text;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Password Generator 協助靜態類別 
    /// </summary>
    public static class PasswordGenerator
    {
        /// <summary>
        /// 密碼產生器
        /// </summary>
        /// <param name="len">密碼長度</param>
        /// <param name="pwdStrength">1:數字 2:數字+小寫 3:數字+小寫+大寫 4:數字+小寫+大寫+符號</param>
        /// <returns>密碼值</returns>
        public static string Generator(int len = 4, PasswordStrength pwdStrength = PasswordStrength.NumbersAndLowercaseAndUppercaseAndSymbols)
        {
            char[] numbers = "0123456789".ToCharArray();
            char[] lowercaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] symbols = "!@#$%^&*".ToCharArray();

            StringBuilder sb = new System.Text.StringBuilder();             
            for (int i = 0; i < len; i++)
            {
                switch (RNG.Next( Convert.ToInt16(pwdStrength) - 1))  //隨機 0~3  依據  pwdStrength 的值  預設  4
                {
                    case 0:
                        sb.Append(numbers[RNG.Next(numbers.Length - 1)]);
                        break;
                    case 1:
                        sb.Append(lowercaseLetters[RNG.Next(lowercaseLetters.Length - 1)]);
                        break;
                    case 2:
                       sb.Append(uppercaseLetters[RNG.Next(uppercaseLetters.Length - 1)]);
                        break;
                    case 3:
                        sb.Append(symbols[RNG.Next(symbols.Length - 1)]);
                        break;
                }                 
            }             
            return sb.ToString();
        }         
    }
}
