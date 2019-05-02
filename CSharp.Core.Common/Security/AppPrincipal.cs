using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{

    /// <summary>
    /// 身分識別繼承至IPrincipal
    /// </summary>
    public class AppPrincipal : IPrincipal
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="identity">識別</param>
        public AppPrincipal(IIdentity identity)
        {
            this.Identity = identity;
        }
        /// <summary>
        /// 識別
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// 是否為合法角色
        /// </summary>
        /// <param name="role">角色名稱</param>
        /// <returns>true:合法</returns>
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 角色字串陣列
        /// </summary>
        public string[] roles { get; set; }

    }
}
