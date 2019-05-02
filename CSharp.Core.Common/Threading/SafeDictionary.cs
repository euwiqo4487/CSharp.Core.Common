using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 執行緒安全字典集合
    /// </summary>
    /// <typeparam name="TKey">泛型 key</typeparam>
    /// <typeparam name="TValue">泛型 值</typeparam>
    public class SafeDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly object _lock = new object();
        private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        /// <summary>
        /// 數量
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return dictionary.Count;
                }
            }
        }
        /// <summary>
        /// 新增 或更新
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="val">val</param>
        public void Add(TKey key, TValue val)
        {
            lock (_lock)
            {
                //TValue value;
                //if (!dictionary.TryGetValue(key, out value))
                //{
                dictionary.Add(key, val);
                //}
                //else
                //{
                //    dictionary[key] = val;
                //}
            }
        }
        /// <summary>
        /// 索引子
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>val</returns>
        public TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return dictionary[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    dictionary[key] = value;
                }
            }
        }
        /// <summary>
        ///  keys 列舉清單
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                lock (_lock)
                {
                    return dictionary.Keys;
                }
            }
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>true:成功</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_lock)
            {
                return dictionary.TryGetValue(key, out value);
            }
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true:存在</returns>
        public bool ContainsKey(TKey key)
        {
            lock (_lock)
            {
                return dictionary.ContainsKey(key);
            }
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">key</param>       
        public void Remove(TKey key)
        {
            lock (_lock)
            {
                TValue value;
                if (!dictionary.TryGetValue(key, out value))
                {
                    return;
                }
                dictionary.Remove(key);
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                dictionary.Clear();
            }
        }
        /// <summary>
        /// 逐一查看列舉值
        /// </summary>
        /// <returns>列舉值</returns>
        public IEnumerator GetEnumerator()
        {
            lock (_lock)
            {
                return dictionary.GetEnumerator();
            }
        }

        /// <summary>
        /// 逐一查看列舉值
        /// </summary>
        /// <returns>列舉值</returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            lock (_lock)
            {
                return dictionary.GetEnumerator();
            }
        }
    }
}
