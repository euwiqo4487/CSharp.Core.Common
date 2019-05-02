using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;

namespace CSharp.Core.Common.Security
{
    /// <summary>
    /// 模擬帳號權限
    /// </summary>
    public class Impersonation : IDisposable
    {
        IntPtr token, tokenDuplicate;
        WindowsImpersonationContext impersonationContext;
        private Impersonation(string account, string password)
        {
            try
            {
                Result = null;
                int nErrorCode;
                Tuple<string, string, string> auth = CSharpCustomer.RequestFileTransferAuth(account, password);
                if (auth.Item1.Trim() == "<<ErrMsg>>")
                    throw new InvalidOperationException(auth.Item2);
                bool isLogonUser = NativeMethods.LogonUser(auth.Item2, auth.Item1, auth.Item3, NativeMethods.LogonType.NewCredentials, NativeMethods.LogonProvider.Default, out token);
                if (!isLogonUser)
                {
                    nErrorCode = NativeMethods.GetLastError();
                    Result = "LogonUser() failed with error code: " + nErrorCode + "\r\n";
                }

                bool isDupToken = NativeMethods.DuplicateToken(token, NativeMethods.SecurityImpersonationLevel.Impersonation, out tokenDuplicate);
                if (!isDupToken)
                {
                    nErrorCode = NativeMethods.GetLastError();
                    Result += "DuplicateToken() failed with error code: " + nErrorCode + "\r\n";
                }
                else
                {
                    var windowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = windowsIdentity.Impersonate();
                }

                if (!string.IsNullOrEmpty(Result))
                    throw new InvalidOperationException(Result);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 權限模擬結果(提供外部輸出)
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 模擬帳號登入驗證
        /// </summary>
        /// <example>
        /// <code language="cs" title="網域呼叫方式">
        /// using (var imp = Impersonation.LogOn("CSharp\JackWang", "12345"))
        /// {
        ///     //模擬帳號權限在此使用
        /// }
        /// </code>
        /// <code language="cs" title="單機呼叫方式">
        /// using (var imp = Impersonation.LogOn("Server01\JackWang", "12345"))
        /// {
        ///     //模擬帳號權限在此使用
        /// }
        /// </code>
        /// </example>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public static Impersonation LogOn(string account, string password)
        {
            return new Impersonation(account, password);
        }

        #region IDisposable 成員
        /// <summary>
        /// 模擬帳號登出
        /// 其權限隨之失效
        /// </summary>
        /// <returns>bool取消過程是否成功</returns>
        public bool LogOff()
        {
            bool success = true;
            if (impersonationContext != null)
            {
                impersonationContext.Undo();
                impersonationContext.Dispose();
            }

            if (tokenDuplicate != IntPtr.Zero)
            {
                if (!NativeMethods.CloseHandle(tokenDuplicate))
                    success = false;
            }

            if (token != IntPtr.Zero)
            {
                if (!NativeMethods.CloseHandle(token))
                    success = false;
            }
            return success;
        }

        /// <summary>
        /// 註記釋放可回收
        /// </summary>
        /// <param name="disposing">是否回收Managed物件</param>
        protected virtual void Dispose(bool disposing)
        {
            LogOff();
        }

        /// <summary>
        /// 註記釋放可回收
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 註銷子 (時間點依系統決定)
        /// 自動註記類別資源回收,如果使用者沒有執行Dispose()
        /// 內含LogOff()
        /// </summary>
        ~Impersonation() { Dispose(false); }
        #endregion

        #region Native Methods
        internal static class NativeMethods
        {
            /// <summary>
            /// The type of logon operation to perform.
            /// </summary>
            internal enum LogonType : int
            {
                /// <summary>
                /// This logon type is intended for users who will be interactively
                /// using the computer, such as a user being logged on by a
                /// terminal server, remote shell, or similar process.
                /// This logon type has the additional expense of caching logon
                /// information for disconnected operations; therefore, it is
                /// inappropriate for some client/server applications, such as a
                /// mail server.
                /// </summary>
                Interactive = 2,

                /// <summary>
                /// This logon type is intended for high performance servers to
                /// authenticate plaintext passwords.
                /// The LogonUser function does not cache credentials for this
                /// logon type.
                /// </summary>
                Network = 3,

                /// <summary>
                /// This logon type is intended for batch servers, where processes
                /// may be executing on behalf of a user without their direct
                /// intervention.  This type is also for higher performance servers
                /// that process many plaintext authentication attempts at a time,
                /// such as mail or Web servers.
                /// The LogonUser function does not cache credentials for this
                /// logon type.
                /// </summary>
                Batch = 4,

                /// <summary>
                /// Indicates a service-type logon.  The account provided must have
                /// the service privilege enabled.
                /// </summary>
                Service = 5,

                /// <summary>
                /// This logon type is for GINA DLLs that log on users who will be
                /// interactively using the computer.
                /// This logon type can generate a unique audit record that shows
                /// when the workstation was unlocked.
                /// </summary>
                Unlock = 7,

                /// <summary>
                /// This logon type preserves the name and password in the
                /// authentication package, which allows the server to make
                /// connections to other network servers while impersonating the
                /// client.  A server can accept plaintext credentials from a
                /// client, call LogonUser, verify that the user can access the
                /// system across the network, and still communicate with other
                /// servers.
                /// NOTE: Windows NT:  This value is not supported.
                /// </summary>
                NetworkCleartext = 8,

                /// <summary>
                /// This logon type allows the caller to clone its current token
                /// and specify new credentials for outbound connections.  The new
                /// logon session has the same local identifier but uses different
                /// credentials for other network connections.
                /// NOTE: This logon type is supported only by the
                /// LOGON32_PROVIDER_WINNT50 logon provider.
                /// NOTE: Windows NT:  This value is not supported.
                /// </summary>
                NewCredentials = 9
            }

            /// <summary>
            /// Specifies the logon provider.
            /// </summary>
            internal enum LogonProvider : int
            {
                /// <summary>
                /// Use the standard logon provider for the system.
                /// The default security provider is negotiate, unless you pass
                /// NULL for the domain name and the user name is not in UPN format.
                /// In this case, the default provider is NTLM.
                /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
                /// </summary>
                Default = 0,

                /// <summary>
                /// Use this provider if you'll be authenticating against a Windows
                /// NT 3.51 domain controller (uses the NT 3.51 logon provider).
                /// </summary>
                WinNT35 = 1,

                /// <summary>
                /// Use the NTLM logon provider.
                /// </summary>
                WinNT40 = 2,

                /// <summary>
                /// Use the negotiate logon provider.
                /// </summary>
                WinNT50 = 3
            }

            /// <summary>
            /// The type of logon operation to perform.
            /// </summary>
            internal enum SecurityImpersonationLevel : int
            {
                /// <summary>
                /// The server process cannot obtain identification information
                /// about the client, and it cannot impersonate the client.  It is
                /// defined with no value given, and thus, by ANSI C rules,
                /// defaults to a value of zero.
                /// </summary>
                Anonymous = 0,

                /// <summary>
                /// The server process can obtain information about the client,
                /// such as security identifiers and privileges, but it cannot
                /// impersonate the client.  This is useful for servers that export
                /// their own objects, for example, database products that export
                /// tables and views.  Using the retrieved client-security
                /// information, the server can make access-validation decisions
                /// without being able to use other services that are using the
                /// client's security context.
                /// </summary>
                Identification = 1,

                /// <summary>
                /// The server process can impersonate the client's security
                /// context on its local system.  The server cannot impersonate the
                /// client on remote systems.
                /// </summary>
                Impersonation = 2,

                /// <summary>
                /// The server process can impersonate the client's security
                /// context on remote systems.
                /// NOTE: Windows NT:  This impersonation level is not supported.
                /// </summary>
                Delegation = 3
            }

            /// <summary>
            /// Logs on the user.
            /// </summary>
            /// <param name="userName">Name of the user.</param>
            /// <param name="domain">The domain.</param>
            /// <param name="password">The password.</param>
            /// <param name="logonType">Type of the logon.</param>
            /// <param name="logonProvider">The logon provider.</param>
            /// <param name="token">The token.</param>
            /// <returns>True if the function succeeds, false if the function fails.
            /// To get extended error information, call GetLastError.</returns>
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool LogonUser(
                string userName,
                string domain,
                string password,
                LogonType logonType,
                LogonProvider logonProvider,
                out IntPtr token);

            /// <summary>
            /// Duplicates the token.
            /// </summary>
            /// <param name="existingTokenHandle">The existing token
            /// handle.</param>
            /// <param name="securityImpersonationLevel">The security impersonation
            /// level.</param>
            /// <param name="duplicateTokenHandle">The duplicate token
            /// handle.</param>
            /// <returns>True if the function succeeds, false if the function fails.
            /// To get extended error information, call GetLastError.</returns>
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DuplicateToken(
                IntPtr existingTokenHandle,
                SecurityImpersonationLevel securityImpersonationLevel,
                out IntPtr duplicateTokenHandle);

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ImpersonateLoggedOnUser(IntPtr hToken);
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool RevertToSelf();

            /// <summary>
            /// Closes the handle.
            /// </summary>
            /// <param name="handle">The handle.</param>
            /// <returns>True if the function succeeds, false if the function fails.
            /// To get extended error information, call GetLastError.</returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CloseHandle(IntPtr handle);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int GetLastError();
        }
        #endregion
    }
}
