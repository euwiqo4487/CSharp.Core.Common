using CSharp.Core.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 文化特性協助靜態類別,如設定語系 或 取得語系等
    /// </summary>
    public static class CultureHelper
    {
        private static readonly IList<string> cultures = new List<string> { "zh-tw","zh-cn","en-us"};
        /// <summary>
        /// 取得語系預設值
        /// </summary>
        /// <returns>語系預設值</returns>
        public static string GetDefaultCulture()
        {
            return cultures[0];
        }
        /// <summary>
        /// 取得相近似名稱如  en-  開頭的  如 en-sg 新加坡
        /// </summary>
        /// <param name="name">語系名</param>
        /// <returns>語系頭</returns>
        public static string GetNeutralCulture(string name)
        {
            if (name.Length <= 2) return name;

            return name.Substring(0,2);
        }
        /// <summary>
        /// 取得正確語系
        /// </summary>
        /// <param name="name">語系名</param>
        /// <returns>語系值</returns>
        public static string GetCulture(string name)
        {
            if(string.IsNullOrEmpty(name)) return GetDefaultCulture();//預設值
            if (cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return name;//命中
            }

            string neutralCultureName = GetNeutralCulture(name);//取得相近似名稱
            foreach (string cultureName in cultures)
            {
                if (cultureName.StartsWith(neutralCultureName)) return cultureName;//命中
            }
            return GetDefaultCulture();//都沒中  預設值
        }

        /// <summary>
        /// 取得目前執行緒文化特性
        /// </summary>
        /// <returns>文化特性</returns>
        public static string GetThreadCulture()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// 取得目前執行緒文化特性
        /// </summary>
        /// <returns>文化特性</returns>
        public static CultureInfo ThreadCultureInfo 
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }
        /// <summary>
        /// 自行指定語系 zh-cn 中国 或 zh-hk 香港 或 en-us 美国 或 en-sg 新加坡 或 en-ph 菲律宾 或 en-my 马来西亚 或 h-TH 泰国 或 vi-VN 越南
        /// </summary>
        /// <param name="CultureInfoVal">zh-cn</param>
        public static void SetCulture(string CultureInfoVal)
        {             
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(CultureInfoVal);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureInfoVal);
        }

    }
}
