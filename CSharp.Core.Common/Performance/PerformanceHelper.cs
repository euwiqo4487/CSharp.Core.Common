using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 效能計數器協助靜態類別
    /// </summary>
    public static class PerformanceHelper
    {
        /// <summary>
        /// Windows NT 效能計數器元件取得這個計數器的未經處理或未經計算的值
        /// </summary>
        /// <example>
        /// <code language="cs" title="效能計數器元件取得這個計數器的未經處理或未經計算的值">
        /// Console.WriteLine(PerformanceHelper.GetRawValue(CategoryName.ServiceModelService, CounterName.PercentOfMaxConcurrentSessions, "SystemService@net.tcp:||localhost:818|SystemService"));
        /// </code>
        /// </example>
        /// <param name="categoryName">與這個效能計數器相關的效能計數器分類 (效能物件) 名稱</param>
        /// <param name="counterName">效能計數器的名稱</param>
        /// <param name="instanceName">能計數器分類執行個體的名稱；如果分類含有單一執行個體，則為空字串 ("")</param>
        /// <returns>值</returns>
        public static long GetRawValue(string categoryName, string counterName, string instanceName = "")
        {
            try
            {                
                PerformanceCounter PC = new PerformanceCounter(categoryName, counterName, instanceName.Replace('/', '|'));                 
                return PC.RawValue;
            }
            catch (Exception)
            {
            }
            return 0;
        }
        /// <summary>
        /// 判斷分類是否登錄在指定電腦上
        /// </summary>
        /// <param name="categoryName">要尋找的效能計數器分類名稱</param>
        /// <returns>果分類已登錄，則為 true，否則為 false</returns>
        public static bool IsCategoryExists(string categoryName)
        {
            return PerformanceCounterCategory.Exists(categoryName);
        }
        /// <summary>
        /// 擷取登錄在本機電腦上的效能計數器分類清單
        /// </summary>
        /// <example>
        /// <code language="cs" title="計數器分類清單">
        /// foreach(string name in PerformanceHelper.GetCategoryNames())
        ///  {
        ///      Console.WriteLine(name);
        ///  }
        ///  Console.ReadLine();
        /// </code>
        /// </example>
        /// <returns>計數器分類清單</returns>
        public static List<string> GetCategoryNames()
        {
            List<string> names = new List<string>();
            PerformanceCounterCategory[] pccs = PerformanceCounterCategory.GetCategories();
            for (int i = 0; i < pccs.Length; i++)
            {
                names.Add(pccs[i].CategoryName);
            }
            return names;
        }
        /// <summary>
        /// 擷取與這個分類相關的效能物件執行個體清單
        /// </summary>
        /// <example>
        /// <code language="cs" title="執行個體清單">
        /// foreach(string name in PerformanceHelper.GetInstanceNames(PerfmonCategoryName.ServiceModelService))
        ///  {
        ///      Console.WriteLine(name);
        ///  }
        ///  Console.ReadLine();
        /// </code>
        /// </example>
        /// <param name="categoryName">與這個效能計數器相關的效能計數器分類 (效能物件) 名稱</param>
        /// <returns>表示與這個分類相關的效能物件執行個體名稱，或者包含空字串 ("") 的單一項目陣列 (如果分類只包含一個效能物件執行個體)</returns>
        public static List<string> GetInstanceNames(string categoryName)
        {            
            PerformanceCounterCategory pcc = new PerformanceCounterCategory(categoryName);
            return pcc.GetInstanceNames().ToList();             
        }
        /// <summary>
        /// 擷取只包含一個執行個體的效能計數器分類中的計數器清單
        /// </summary>
        /// <example>
        /// <code language="cs" title="計數器清單">
        /// foreach(string name in PerformanceHelper.GetCounterNames(PerfmonCategoryName.Processor, PerfmonInstanceName.Total))
        ///  {
        ///      Console.WriteLine(name);
        ///  }
        ///  Console.ReadLine();
        /// </code>
        /// </example>
        /// <param name="categoryName">與這個效能計數器相關的效能計數器分類 (效能物件) 名稱</param>
        /// <param name="instanceName">表示與這個分類相關的效能物件執行個體名稱，或者包含空字串 ("") 的單一項目陣列 (如果分類只包含一個效能物件執行個體)</param>
        /// <returns>計數器清單</returns>
        public static List<string> GetCounterNames(string categoryName,string instanceName="")
        {
            PerformanceCounter[] counters;
            List<string> names = new List<string>();
            PerformanceCounterCategory pcc = new PerformanceCounterCategory(categoryName);
            if (instanceName.Length > 0)
            {
                counters = pcc.GetCounters(instanceName);
            }
            else
            {
                counters = pcc.GetCounters();
            }
            for (int i = 0; i < counters.Length; i++)
            {
                names.Add(counters[i].CounterName);
            }
            return names;            
        }
    }
}
