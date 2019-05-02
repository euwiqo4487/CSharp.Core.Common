using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Password Strength 定義
    /// </summary>
    public enum PasswordStrength
    {
        /// <summary>
        /// 數字
        /// </summary>
        [Description("數字")]
        Numbers = 1,
        /// <summary>
        /// 數字和小寫
        /// </summary>
        [Description("數字和小寫")]
        NumbersAndLowercase = 2,
        /// <summary>
        /// 數字和小寫和大寫
        /// </summary>
        [Description("數字和小寫和大寫")]
        NumbersAndLowercaseAndUppercase = 3,
        /// <summary>
        /// 數字和小寫和大寫和符號
        /// </summary>
        [Description("數字和小寫和大寫和符號")]
        NumbersAndLowercaseAndUppercaseAndSymbols = 4
    }
}
