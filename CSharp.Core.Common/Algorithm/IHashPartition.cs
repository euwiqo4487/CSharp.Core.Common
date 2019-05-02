using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 分片介面
    /// </summary>
    public interface IHashPartition
    {
        /// <summary>
        /// 加入新結點
        /// </summary>
        /// <param name="item">節點名稱</param>
        void AddNode(string item);
        /// <summary>
        /// 移除結點
        /// </summary>
        /// <param name="item">節點名稱</param>
        void RemoveNode(string item);
        /// <summary>
        /// 節點清單
        /// </summary>
        List<string> NodeList { get; }
        /// <summary>
        ///  取得節點名稱
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回節點名稱</returns>
        string GetPrimary(string input);
    }
}
