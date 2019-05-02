using System;
using System.Collections;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 給CSharp.Customer使用來進行LDAP登入的介面定義
    /// </summary>
    public interface ILdapLoginSetting
    {
        /// <summary>
        /// LDAP 登入
        /// </summary>
        /// <param name="userid">使用者名稱</param>
        /// <param name="password">密碼</param>
        /// <returns>登入結果,額外資訊</returns>
        Tuple<bool, Hashtable> Login(string userid, string password);
    }
}
