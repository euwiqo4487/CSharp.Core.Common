using System;
using System.Collections;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp定義來給Client使用交換訊息的靜態方法
    /// </summary>
    public static class AppContext
    {
        /// <summary>
        /// 跟WCF溝通用的Token
        /// </summary>
        public static string AppToken { get; set; }
        /// <summary>
        /// AP用的語系  zh-tw  us   zh-cn
        /// </summary>
        public static string CultureInfo { get; set; }
        /// <summary>
        /// 使用者中文名稱
        /// </summary>
        public static string UserName { get; set; }
        /// <summary>
        /// 使用者英文名稱
        /// </summary>
        public static string UserEName { get; set; }
        /// <summary>
        /// 使用者代號
        /// </summary>
        public static string UserID { get; set; }
        /// <summary>
        /// 使用者內部代號
        /// </summary>
        public static int Uno { get; set; }
        /// <summary>
        /// 員編
        /// </summary>
        public static string EmpID { get; set; }
        /// <summary>
        /// 使用者IP
        /// </summary>
        public static string UserIP { get; set; }
        /// <summary>
        /// 設定為操作時間上限.單位:分鐘
        /// </summary>
        public static int TimeOutMinute { get; set; }
        /// <summary>
        /// 設定目前操作時間
        /// </summary>
        public static DateTime ServiceTime { get; set; }
        /// <summary>
        /// 選擇性資訊
        /// </summary>
        public static Hashtable Options { get; set; }
        /// <summary>
        /// 是否超過逾時時間,如未超過逾時,則更新 ServiceTime 
        /// </summary>       
        /// <returns>true:未逾時 false:逾時</returns>
        public static bool IsValid()
        {
            if (DateTime.Now.Subtract(ServiceTime).TotalMinutes <= TimeOutMinute)
            {
                ServiceTime = DateTime.Now;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 登入時間
        /// </summary>
        public static DateTime LoginTime { get; set; }
    }
}
