using System;
using System.Messaging;

namespace Jepun.Core.Common
{
    /// <summary>
    /// 交換訊息使用  (MSMQ使用)
    /// </summary>
    [Serializable]
    public class MsgQueueContent
    {
        public MsgQueueContent()
        {
            MessageType = MessageType.None;
            Priority = MessagePriority.Normal;
            From = "";
            FromName = "";
            To = "";
            ToName = "";
            FormID = "";
            OpenForm = false;
            ShowNotifyWindows = true;

            
            Content = "";
            Data = "";
            SendTime = DateTime.Now;            
        }
        public MsgQueueContent(MessageType msgType)
        {
            MessageType = msgType;
            Priority = MessagePriority.Normal;
            From = "";
            FromName = "";
            To = "";
            ToName = "";
            FormID = "";
            OpenForm = false;
            ShowNotifyWindows = true;

             
            Content = "";
            Data = "";
            SendTime = DateTime.Now;            
        }
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 寄件者     (MSMQ的Queue名稱)
        /// </summary>
        public string From { get; set; }     
        /// <summary>
        /// 寄件者,描述名稱
        /// </summary>
        public string FromName { get; set; }
        /// <summary>
        /// 收件者,若有多個請使用迴圈處理    (MSMQ的Queue名稱)
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 收件者,描述名稱
        /// </summary>
        public string ToName { get; set; }
        /// <summary>
        /// 指定接收訊息的form id
        /// </summary>
        public string FormID { get; set; }
        /// <summary>
        /// 是否強迫開啟指定的Form(可省略不指定,預設為false)
        /// </summary>
        public bool OpenForm { get; set; }
        /// <summary>
        /// 是否顯示Notify Window(可省略不指定,預設為true)
        /// </summary>
        public bool ShowNotifyWindows { get; set; }
        /// <summary>
        /// MSMQ的優先權
        /// </summary>
        public MessagePriority Priority { get; set; }
        /// <summary>
        /// 顯示的內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 資料
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 傳送時間
        /// </summary>
        public DateTime SendTime { get; set; }
       
    }
}
