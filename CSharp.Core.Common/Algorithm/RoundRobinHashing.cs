using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Round Robin , 缺點 當機器(服務)回來時,會無法回到原機器(服務),因為隨時都在偵測服務是否斷掉heart Beat,一至性演算法可回來且分布較平均
    /// 除非去管理員使清單,太麻煩,
    /// 在分布式集群中，对机器的添加删除，或者机器故障后自动脱离集群这些操作是分布式集群管理最基本的功能。如果采用常用的hash(object)%N算法，
    /// 那么在有机器添加或者删除后，很多原有的数据就无法找到了，这样严重的违反了单调性原则。會造成大量数据迁移
    /// </summary>
    public class RoundRobinHashing : IHashPartition
    {
        readonly object _locker = new object();
        private List<string> nodeList;         
        /// <summary>
        /// 節點名稱清單
        /// </summary>
        /// <param name="nodes"></param>
        public RoundRobinHashing(List<string> nodes)
        {
            nodeList = nodes;           
        }
        /// <summary>
        /// 加入新結點
        /// </summary>
        /// <param name="item">節點名稱</param>
        public void AddNode(string item)
        {
            lock (_locker)
            {
                nodeList.Add(item);                 
            }
        }
        /// <summary>
        /// 移除結點
        /// </summary>
        /// <param name="item">節點名稱</param>
        public void RemoveNode(string item)
        {
            lock (_locker)
            {
                nodeList.Remove(item);                 
            }
        }
        /// <summary>
        /// 節點清單
        /// </summary>
        public List<string> NodeList
        {
            get { return nodeList; }
        }
        /// <summary>
        ///  取得節點名稱
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回節點名稱</returns>
        public string GetPrimary(string input)
        {
            string result = "";
            lock (_locker)
            {
                if (nodeList.Count == 0) return "";
                //       hash(value) mod K(機器數)
                result = nodeList[hashString(input) % nodeList.Count];
            }
            return result;
        }
        private Int32 hashString(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            md5.Clear();
            return BitConverter.ToInt32(hashed, 0);
        }
    }
}
