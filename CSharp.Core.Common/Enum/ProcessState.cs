using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// process 執行狀態
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// 等待
        /// </summary>
        Wait = 99,
        /// <summary>
        /// 成功
        /// </summary>
        Success =1,
        /// <summary>
        /// 失敗
        /// </summary>
        Failed = -1
    }
}
