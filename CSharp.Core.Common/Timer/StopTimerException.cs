using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 提供SimplexWorkerTiming工作排程計時器,拋出一個停止計時錯誤讓計時器停止計時
    /// </summary>
    [Serializable]
    public class StopTimerException : Exception, ISerializable
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public StopTimerException() : base("停止SimplexWorkerTiming計時") { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="message">錯誤原因</param>
        public StopTimerException(string message) : base(message) { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="message">錯誤原因</param>
        /// <param name="inner">Inner Exception</param>
        public StopTimerException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// This constructor is needed for serialization.
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        protected StopTimerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        /// <summary>
        /// 使用序列化目標的所有例外狀況物件資料來設定 SerializationInfo。 在還原序列化期間，從在資料流上傳輸的 SerializationInfo 重新組成例外狀況。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
