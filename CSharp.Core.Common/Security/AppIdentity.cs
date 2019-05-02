using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 身分物件,繼承至IIdentity
    /// </summary>
    public class AppIdentity : IIdentity
    {
        private IIdentity identity;
        private DateTime serviceTime;
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="authenticationType">認證類型 如 Forms</param>
        public AppIdentity(string name, string authenticationType)
        {
            identity = new GenericIdentity(name, authenticationType);
            serviceTime = DateTime.Now;
        }        
        #region IIdentity 實作
        /// <summary>
        /// 認證類型
        /// </summary>
        public string AuthenticationType
        {
            get { return identity.AuthenticationType; }
        }
        /// <summary>
        /// 是否認證
        /// </summary>
        public bool IsAuthenticated { get { return identity.IsAuthenticated; } }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get { return identity.Name; } }
        #endregion              
        /// <summary>
        /// 跟WCF溝通用的Token
        /// </summary>
        public string AppToken { get; set; }        
        /// <summary>
        /// 使用者中文名稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 使用者英文名稱
        /// </summary>
        public string UserEName { get; set; }
        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 員編
        /// </summary>
        public string EmpID { get; set; }
        /// <summary>
        /// 使用者內部代號
        /// </summary>
        public int Uno { get; set; }
        /// <summary>
        /// 使用者IP
        /// </summary>
        public string UserIP { get; set; }
        /// <summary>
        /// 服務時間
        /// </summary>
        public string ServiceTime { get { return serviceTime.ToString(); } }
        /// <summary>
        /// 額外資訊
        /// </summary>
        public Hashtable Options { get; set; }
        /// <summary>
        /// 是否超過逾時時間,如未超過逾時,則更新 ServiceTime 
        /// </summary>
        /// <param name="minute">逾時時間 : 單位分鐘</param>
        /// <returns>true:未逾時 false:逾時</returns>
        public bool IsValid(int minute)
        {
            if (DateTime.Now.Subtract(serviceTime).TotalMinutes <= minute)
            {
                serviceTime = DateTime.Now;
                return true;
            }
            return false;
        } 
    }
}
