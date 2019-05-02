using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp.Config的Shard 清單區段
    /// </summary>
    [ConfigurationCollection(typeof(ShardServerElement), AddItemName = "ShardServer", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ShardList : GenericElementCollection<ShardServerElement>
    {
        /// <summary>
        /// 取得SendToElement
        /// </summary>
        /// <param name="Name">名字</param>
        /// <returns>SendTo Element</returns>
        public ShardServerElement GetShardList(String Name)
        {
            return BaseGet(Name) as ShardServerElement;
        }
    }
    /// <summary>
    /// CSharp.Config的分片清單區段
    /// </summary>
    public class ShardServerElement : GenericElement
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public ShardServerElement() { }
        /// <summary>
        /// ShardServerElement
        /// </summary>
        /// <param name="name">DB連線名稱</param>
        /// <param name="map">對應值</param>
        public ShardServerElement(string name, string map)
        {
            this.Name = name;
            this.Map = map;
        }
        /// <summary>
        /// 注意,這裡要給 Key值,取得資料用
        /// </summary>
        public override object ElementKey
        {
            get { return this.Name; }
        }
        /// <summary>
        /// 當作識別的KEY
        /// </summary>
        [ConfigurationProperty("Name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }
        /// <summary>
        /// Map
        /// </summary>
        [ConfigurationProperty("Map", DefaultValue = "", IsRequired = true)]
        public string Map
        {
            get { return (string)base["Map"]; }
            set { base["Map"] = value; }
        }
    }
}
