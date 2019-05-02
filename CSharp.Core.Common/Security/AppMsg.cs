using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp定義來放MSMQ訊息的交換物件
    /// </summary>
    public class AppMsg
    {
        private Dictionary<string, string> data = new Dictionary<string, string>();

        /// <summary>
        /// 訊息種類
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否主動打開
        /// </summary>
        public bool AutoOpen { get; set; }
        /// <summary>
        /// 寄送時間
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// AssemblyName
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// ClassName
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 表單名稱
        /// </summary>
        public string FormName { get; set; }
        /// <summary>
        /// 寄件者
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 收件者
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 主體內容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 夾帶參數資料
        /// </summary>
        public Dictionary<string, string> ParameterData
        {
            get { return data; }
        }
        /// <summary>
        /// 加入參數資料
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void AddOrReplaceParameterData(string key, string value)
        {
            data.AddOrSet(key, value);
        }
        /// <summary>
        /// 移除參數資料
        /// </summary>
        /// <param name="key">key</param>
        public void RemoveParameterData(string key)
        {
            data.Remove(key);
        }

        /// <summary>
        /// 取得參數資料
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public string GetParameterData(string key)
        {
            if (ParameterData.ContainsKey(key))
                return ParameterData[key];
            return string.Empty;
        }
    }
}
