using System;
using System.Configuration;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp組態檔 通用元素
    /// </summary>
    public abstract class GenericElement : ConfigurationElement
    {
        /// <summary>元素的主鍵</summary>
        public abstract object ElementKey { get; }
    }
    /// <summary>
    /// CSharp組態檔 通用元素集合
    /// </summary>
    /// <typeparam name="ItemType">元素型別</typeparam>
    public abstract class GenericElementCollection<ItemType> : ConfigurationElementCollection where ItemType : GenericElement, new()
    {
        /// <summary>
        /// 元素型別 索引子 依據index
        /// </summary>
        public ItemType this[int index] { 
            get 
            { 
                return (ItemType)BaseGet(index); 
            } 
            set
            { 
                if (BaseGet(index) != null) { BaseRemoveAt(index); } BaseAdd(index, value); 
            } 
        }
        /// <summary>
        /// 元素型別 索引子依據key
        /// </summary>
        public new ItemType this[String key] 
        { 
            get 
            { 
                return BaseGet(key) as ItemType; 
            } 
            set 
            { 
                if (BaseGet(key) != null) 
                { 
                    BaseRemove(key); 
                } 
                BaseAdd(value); 
            } 
        }
        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="element">元素</param>
        public void Add(ItemType element) 
        { 
            BaseAdd(element); 
        }
        /// <summary>
        /// Remove 
        /// </summary>
        /// <param name="key">key</param>
        public void Remove(object key) 
        { 
            BaseRemove(key);
        }
        /// <summary>
        /// RemoveAt
        /// </summary>
        /// <param name="index">index</param>
        public void RemoveAt(int index) 
        { 
            BaseRemoveAt(index);
        }
        /// <summary>
        /// Clear
        /// </summary>
        public void Clear() 
        { 
            BaseClear(); 
        }

        #region override
        /// <summary>
        /// Collection Type
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType 
        { 
            get 
            { 
                return ConfigurationElementCollectionType.AddRemoveClearMap; 
            } 
        }
        /// <summary>
        /// Create New Element
        /// </summary>
        /// <returns>New Element</returns>
        protected override ConfigurationElement CreateNewElement() 
        { 
            return new ItemType(); 
        }
       /// <summary>
        /// 取得元素key值
       /// </summary>
        /// <param name="element">元素</param>
        /// <returns>元素key值</returns>
        protected override object GetElementKey(ConfigurationElement element) 
        { 
            return ((ItemType)element).ElementKey; 
        }
        #endregion
    }
}
