using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 連線參數
    /// </summary>
    public class ConnectionNodeEventArgs : EventArgs
    {
       
        /// <summary>
        ///  
        /// </summary>
        public readonly string NodeName;
        /// <summary>
        /// 建構子
        /// </summary>       
        /// <param name="nodeName">節點名稱</param>
        public ConnectionNodeEventArgs(string nodeName)
        {
            NodeName = nodeName;
        }
    }
}
