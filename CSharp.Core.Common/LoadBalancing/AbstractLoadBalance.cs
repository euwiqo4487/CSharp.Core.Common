using System;
using System.Threading.Tasks;
using System.Timers;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 實做負載平衡機制時的抽象類別,需被繼承
    /// </summary>
    public abstract class AbstractLoadBalance : ILoadBalance
    {        
        /// <summary>
        /// 發生重新連線  委讓事件
        /// </summary>
        public event EventHandler<ConnectionNodeEventArgs> Reconnected;
        /// <summary>
        /// 發生斷線   委讓事件
        /// </summary>
        public event EventHandler<ConnectionNodeEventArgs> Disconnected;
        /// <summary>
        /// 用來判斷是否已經加事件了
        /// </summary>
        public bool IsDisconnectedExists
        {
            get
            {
                return Disconnected == null ? false : true;
            }
        }
        /// <summary>
        /// 用來判斷是否已經加事件了
        /// </summary>
        public bool IsReconnectedExists
        {
            get
            {
                return Reconnected == null ? false : true;
            }
        }
        /// <summary>
        /// 是否多組負載平衡設定
        /// </summary>
        public bool IsMultiSettings
        {
            get;
            set;
        }
        private int _copies = 160;
        /// <summary>
        /// 複製次數
        /// </summary>
        public int Copies{
            get
            {
                return _copies;
            }
            set 
            {
                _copies = value;
            }
        }
        /// <summary>
        /// 發生重新連線
        /// </summary>
        /// <param name="e">ConnectionNodeEventArgs</param>
        protected virtual void OnReconnected(ConnectionNodeEventArgs e)
        {
            EventHandler<ConnectionNodeEventArgs> handler = Reconnected;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// 發生斷線
        /// </summary>
        /// <param name="e">ConnectionNodeEventArgs</param>
        protected virtual void OnDisconnected(ConnectionNodeEventArgs e)
        {
            EventHandler<ConnectionNodeEventArgs> handler = Disconnected;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// 重新連線節點名稱
        /// </summary>
        public string ReconnectedNode
        {
            set
            {                 
                OnReconnected(new ConnectionNodeEventArgs(value));
            }
        }
        /// <summary>
        /// 斷線節點名稱
        /// </summary>
        public string DisconnectedNode
        {
            set
            {
                OnDisconnected(new ConnectionNodeEventArgs(value));
            }
        }
        ///// <summary>
        ///// 取得  節點
        ///// </summary>
        ///// <param name="input">hash的依據</param>
        ///// <returns>傳回節點名稱</returns>
        //public abstract string GetLocator(string input="");  
        /// <summary>
        /// 偵測的實作
        /// </summary>
        protected Action HeartBeatDetection;
        /// <summary>
        /// 啟動偵測 interval=0 時不偵測
        /// </summary>
        /// <param name="interval">時間間隔:毫秒</param>
        protected void StartHeartBeat(double interval)
        {
            if (interval == 0) return;
            if (!this.IsMultiSettings) return;
            // 執行一次 執行緒
            Task.Run(() => {
                SimplexWorkerTiming aTimer = new SimplexWorkerTiming(Convert.ToDouble(interval));//計時器 完成後才 啟動下一個
                aTimer.WorkAction = (ElapsedEventArgs e) =>
                {
                    //Console.WriteLine(e.SignalTime);
                    //Console.WriteLine("Timing ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
                    HeartBeatDetection();//  如果 裡面的實作是多執行敘 會提前觸發  計時器 完成後才 啟動下一個    
                };
                aTimer.Start();//開始計時            
            });            
        }
        /// <summary>
        /// 取得  節點
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回節點名稱</returns>
        public abstract string GetLocator(string input = "");
        /// <summary>
        /// 取得原本節點
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回原本節點名稱</returns>
        public abstract string GetCurrentLocator(string input = "");
        /// <summary>
        /// 主動移除節點
        /// </summary>
        /// <param name="nodeName">節點名稱</param>    
        public abstract void RemoveNode(string nodeName);
        /// <summary>
        /// 重新整理節點
        /// </summary>
        public abstract void Refresh();
    }
}
