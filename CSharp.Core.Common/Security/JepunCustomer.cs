using System;
using System.IO;
using System.Reflection;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 處理CSharp.Customer之類別
    /// </summary>
    public static class CSharpCustomer
    {
        static string locCusDll;
        const string ERRMSG = "<<ErrMsg>>";
        /// <summary>
        /// 要求檔案傳輸之客製化權限
        /// 否則提供系統參數之對應
        /// </summary>
        /// <param name="account">帳號(解析之前)</param>
        /// <param name="password">密碼</param>
        /// <param name="args">其他動態參數</param>
        /// <returns></returns>
        public static Tuple<string, string, string> RequestFileTransferAuth(string account, string password, params object[] args)
        {
            string domain = "", errMsg = "";
            if (string.IsNullOrEmpty(account) || account.Trim() == "-" || account.LastIndexOf('|') == account.Length - 1)
            {
                if (string.IsNullOrEmpty(locCusDll))
                {
                    var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    locCusDll = Path.Combine(path, CSharpConfig.CustomerDllName + ".dll");
                }

                if (File.Exists(locCusDll))
                {
                    IFileTransferSettings tranferAuthority = null;
                    try
                    {
                        //客製化客戶DLL
                        tranferAuthority = AssemblyHelp.LoadAssembly<IFileTransferSettings>(CSharpConfig.CustomerDllName, "FileTransferSettings", CSharpConfig.CustomerDllName);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw ex;
                    }
                    if (tranferAuthority != null)
                    {
                        Tuple<string, string> auth = tranferAuthority.GetFileTransferAuthority(args);
                        if (auth != null)
                        {
                            if (auth.Item1.Trim() == ERRMSG)
                                errMsg = auth.Item2.Trim();
                            else
                            {
                                //客戶提供之LDAP檔案傳輸帳密
                                account = auth.Item1; //帳號
                                password = auth.Item2; //密碼
                            }
                        }
                    }
                }
            }
            else
            {
                password = password.DecryptData(); //系統參數密碼預設有加密，所以需要解密！
            }

            /*                
             * 從帳號中取得網域或伺服器名稱(僅UNC傳輸模式使用)
             * 例如: CSharp.com.tw\Jacky 或 Server01\Jacky
             */
            if (errMsg.Trim() == "" && account.IndexOf('\\') > -1)
            {
                domain = account.Split('\\')[0]; //網域或伺服器名稱
                account = account.Split('\\')[1];
            }

            return new Tuple<string, string, string>(errMsg.Trim() == "" ? domain : ERRMSG, errMsg.Trim() == "" ? account : errMsg, errMsg.Trim() == "" ? password : errMsg);
        }
    }
}
