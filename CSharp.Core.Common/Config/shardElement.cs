using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common 
{
    /// <summary>
    /// 分片設定
    /// </summary>
    public class ShardElement : ConfigurationElement
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public ShardElement()
        {
        }
        /// <summary>
        /// ShardList
        /// </summary>
        [ConfigurationProperty("ShardList")]
        public ShardList ShardList
        {
            get { return (ShardList)base["ShardList"]; }
        }
    }
}
