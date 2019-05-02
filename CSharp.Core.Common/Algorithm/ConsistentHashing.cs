using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 一至性演算法實作
    /// 1、平衡性(Balance)：平衡性是指哈希的结果能够尽可能分布到所有的缓冲中去，这样可以使得所有的缓冲空间都得到利用。很多哈希算法都能够满足这一条件。
    /// 2、单调性(Monotonicity)：单调性是指如果已经有一些内容通过哈希分派到了相应的缓冲中，又有新的缓冲加入到系统中。哈希的结果应能够保证原有已分配的内容可以被映射到原有的或者新的缓冲中去，而不会被映射到旧的缓冲集合中的其他缓冲区。 
    /// 3、分散性(Spread)：在分布式环境中，终端有可能看不到所有的缓冲，而是只能看到其中的一部分。当终端希望通过哈希过程将内容映射到缓冲上时，由于不同终端所见的缓冲范围有可能不同，从而导致哈希的结果不一致，最终的结果是相同的内容被不同的终端映射到不同的缓冲区中。这种情况显然是应该避免的，因为它导致相同内容被存储到不同缓冲中去，降低了系统存储的效率。分散性的定义就是上述情况发生的严重程度。好的哈希算法应能够尽量避免不一致的情况发生，也就是尽量降低分散性。 
    /// 4、负载(Load)：负载问题实际上是从另一个角度看待分散性问题。既然不同的终端可能将相同的内容映射到不同的缓冲区中，那么对于一个特定的缓冲区而言，也可能被不同的用户映射为不同 的内容。与分散性一样，这种情况也是应当避免的，因此好的哈希算法应能够尽量降低缓冲的负荷。
    /// 一致性哈希算法在保持了单调性的同时，还是数据的迁移达到了最小，这样的算法对分布式集群来说是非常合适的，避免了大量数据迁移，减小了服务器的的压力
    /// </summary>
    public class ConsistentHashing
    {
        readonly object _locker = new object();
        private int nodeCopies;
        private List<string> nodes;
        private SortedList<long, string> nodeMap = new SortedList<long, string>();
        /// <summary>
        /// 建構子
        /// </summary>
        /// <example>
        /// <code language="cs" title=" ">
        /// List&lt;string&gt; nodes = new List&lt;string&gt;();
        /// nodes.Add("_1");
        /// nodes.Add("_2");
        /// nodes.Add("_3");
        /// ConsistentHashing ch = new ConsistentHashing(nodes.ToList(), 160);
        /// string nodeName = ch.GetPrimary("test");
        /// </code>
        /// </example>
        /// <param name="nodes">節點名稱</param>
        /// <param name="nodeCopies">節點虛擬次數</param>
        public ConsistentHashing(List<string> nodes, int nodeCopies = 160)
        {
            this.nodes = nodes;
            this.nodeCopies = nodeCopies;
            this.createNodeMap(this.nodes, this.nodeCopies);  //產生 map           
        }
        /// <summary>
        /// 加入新結點
        /// </summary>
        /// <param name="nodeName">節點名稱</param>
        public void AddNode(string nodeName)
        {
            lock (_locker)
            {
                this.nodes.Add(nodeName);
                this.addNodeMap(nodeName, this.nodeCopies); //加入新節點
                //this.createNodeMap(this.nodes, this.nodeCopies);//重新產生 map  
            }
        }
        /// <summary>
        /// 移除結點
        /// </summary>
        /// <param name="nodeName">節點名稱</param>
        public void RemoveNode(string nodeName)
        {
            lock (_locker)
            {
                this.nodes.Remove(nodeName);
                this.removeNodeMap(nodeName);
                //this.createNodeMap(this.nodes, this.nodeCopies);    //重新產生 map               
            }
        }
        /// <summary>
        /// 節點清單
        /// </summary>
        public List<string> NodeList
        {
            get
            {
                return this.nodes;                 
            }
        }
        /// <summary>
        /// 一至性演算法取得節點
        /// </summary>
        /// <param name="input">hash的依據</param>
        /// <returns>傳回節點名稱</returns>
        public string GetPrimary(string input)
        {
            string result;
            lock (_locker)
            {
                if (this.nodes.Count == 0) return "";
                result = this.GetNodeMapValue(hashAlgorithm(getSHA512(input), 0));
            }
            return result;
        }
        /// <summary>
        /// 取出節點
        /// </summary>
        /// <param name="key">hash值</param>
        /// <returns>傳回節點名稱</returns>
        private string GetNodeMapValue(long key)
        {            
            int pos = 0, low = 1, high, mid;
            if (this.nodeMap.ContainsKey(key))
            {
                return nodeMap[key];
            }
            high = nodeMap.Count - 1;
            while (low <= high)
            {
                mid = (low + high) / 2;
                if (key < this.nodeMap.Keys[mid])
                {
                    high = mid - 1;
                    pos = high;
                }
                else
                {
                    low = mid + 1;
                }
            }
            return this.nodeMap.Values[pos + 1].ToString();           
        }
        private long hashAlgorithm(byte[] hash, int nTime)
        {
            long result = ((long)(hash[3 + nTime * 4] & 0xFF) << 24)
                    | ((long)(hash[2 + nTime * 4] & 0xFF) << 16)
                    | ((long)(hash[1 + nTime * 4] & 0xFF) << 8)
                    | ((long)hash[0 + nTime * 4] & 0xFF);

            return result & 0xffffffffL; /* Truncate to 32-bits */
        }
        //private byte[] getMd5Hash(string input)
        //{
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(input));
        //    md5.Clear();
        //    return result;
        //}
        private byte[] getSHA512(string input)
        {
            SHA512 shaM = new SHA512CryptoServiceProvider();
            byte[] result = shaM.ComputeHash(Encoding.Default.GetBytes(input));
            shaM.Clear();
            return result;
        }
        private void createNodeMap(List<string> nodes, int nodeOfCopies)
        {            
            this.nodeMap.Clear();
            //對所有節點，生成nodeCopies個虛擬結點
            foreach (string nodeName in nodes)
            {
                this.addNodeMap(nodeName, nodeOfCopies);
            }
        }
        private void addNodeMap(string nodeName, int aNodeCopies)
        {
            int numberOfCopies = aNodeCopies == 0 ? 4 : aNodeCopies;
            //每四個虛擬結點為一組
            for (int i = 0; i < numberOfCopies / 4; i++)
            {
                byte[] hash = this.getSHA512(nodeName + i);//為這組虛擬結點得到惟一名稱
                for (int pos = 0; pos < 4; pos++)
                {//Md5是一個16字節長度的數組，將16字節的數組每四個字節一組，分別對應一個虛擬結點，這就是為什麼上面把虛擬結點四個劃分一組的原因
                    long m = this.hashAlgorithm(hash, pos);
                    this.nodeMap[m] = nodeName;
                }
            }
        }
        private void removeNodeMap(string nodeName)
        {
            foreach (long key in this.nodeMap.Keys.ToList())
            {
                if (this.nodeMap[key] == nodeName) 
                {
                    this.nodeMap.Remove(key);
                }
            }
        }
    }
}
