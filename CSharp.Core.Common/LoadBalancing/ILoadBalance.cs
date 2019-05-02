using System;
using System.Collections.Generic;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 負載平衡介面定義
    /// </summary>
    public interface ILoadBalance
    {
        /// <summary>
        /// 用來判斷是否已經加事件了
        /// </summary>
        bool IsDisconnectedExists { get; }
        /// <summary>
        /// 用來判斷是否已經加事件了
        /// </summary>
        bool IsReconnectedExists { get; }
        /// <summary>
        /// 發生重新連線  事件
        /// </summary>
        event EventHandler<ConnectionNodeEventArgs> Reconnected;
        /// <summary>
        /// 發生斷線   事件
        /// </summary>
        event EventHandler<ConnectionNodeEventArgs> Disconnected;
        /// <summary>
        /// 取得  節點
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回節點名稱</returns>
        string GetLocator(string input = "");
        /// <summary>
        /// 取得原本節點
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回原本節點名稱</returns>
        string GetCurrentLocator(string input = "");
        /// <summary>
        /// 主動移除節點
        /// </summary>
        /// <param name="nodeName">節點名稱</param>         
        void RemoveNode(string nodeName);
        ///// <summary>
        ///// 啟動偵測
        ///// </summary>
        ///// <param name="interval">偵測時間間隔</param>
        //void StartHeartBeat(double interval);        
        /// <summary>
        /// 重新整理節點
        /// </summary>
        void Refresh();
    }
}
