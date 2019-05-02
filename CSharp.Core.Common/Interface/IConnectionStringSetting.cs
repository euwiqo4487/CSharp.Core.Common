using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 給CSharp.Customer使用來取得連線字串和DB提供者的介面定義
    /// </summary>
    public interface IConnectionStringSetting
    {
        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="ConnectionStringName">連線名稱</param>
        /// <returns>連線字串,提供者</returns>
        Tuple<string, string> GetConnectionString(string ConnectionStringName);
    }

    /// <summary>
    /// 給CSharp.Customer使用來取得系統使用的檔案傳輸帳密的介面定義
    /// </summary>
    public interface IFileTransferSettings
    {
        /// <summary>
        /// 取得系統使用的檔案傳輸帳密
        /// </summary>
        /// <returns>
        /// Item1為帳號 (格式: 網域名稱\使用者ID 或 機器名稱\使用者ID)
        /// Item2為密碼
        /// </returns>
        Tuple<string, string> GetFileTransferAuthority(params object[] args);
    }
}
