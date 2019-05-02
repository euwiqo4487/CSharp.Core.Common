using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 效能計數器的名稱
    /// </summary>
    public static class PerfmonCounterName
    {
        /// <summary>
        /// 呼叫次數
        /// </summary>
        public static readonly string Calls = "Calls";
        /// <summary>
        /// 呼叫時間
        /// </summary>
        public static readonly string CallsDuration = "Calls Duration";
        /// <summary>
        /// 每秒調用
        /// </summary>
        public static readonly string CallsPerSecond = "Calls Per Second";
        /// <summary>
        /// 實例
        /// </summary>
        public static readonly string Instances = "Instances";
        /// <summary>
        /// 實例創建每秒
        /// </summary>
        public static readonly string InstancesCreatedPerSecond = "Instances Created Per Second";
        /// <summary>
        /// 百分比最大並發會話
        /// </summary>
        public static readonly string PercentOfMaxConcurrentSessions = "Percent Of Max Concurrent Sessions";
        /// <summary>
        /// 百分比最大並發實例
        /// </summary>
        public static readonly string PercentOfMaxConcurrentInstances = "Percent Of Max Concurrent Instances";
        /// <summary>
        /// CPU使用率
        /// </summary>
        public static readonly string ProcessorTime = "% Processor Time";
        /// <summary>
        /// 處理器的平均值
        /// </summary>
        public static readonly string TotalProcessorTime = "% Total Processor Time";
        /// <summary>
        /// 相當於處理器花費在執行 Microsoft Windows 核心命令 (如處理 SQL Server I/O 要求) 的時間百分比
        /// </summary>
        public static readonly string PrivilegedTime = "% Privileged Time";
        /// <summary>
        /// 相當於處理器花費在執行使用者處理序 (如 SQL Server) 的時間百分比
        /// </summary>
        public static readonly string UserTime = "% User Time";
        /// <summary>
        /// 相當於等候處理器時間的執行緒數目。 當處理序的執行緒所需的處理器循環超過可用數量時，就會形成處理器瓶頸
        /// </summary>
        public static readonly string ProcessorQueueLength = "Processor Queue Length";
        /// <summary>
        /// 計數器代表目前有多少記憶體位元組可供處理序使用,計數器的數值偏低，代表電腦整體地缺乏記憶體，或有某個應用程式沒有釋出記憶體
        /// </summary>
        public static readonly string AvailableBytes = "Available Bytes";
        /// <summary>
        /// 計數器數值過高可能代表過度分頁
        /// </summary>
        public static readonly string PagesSec = "Pages/sec";
    }
}
