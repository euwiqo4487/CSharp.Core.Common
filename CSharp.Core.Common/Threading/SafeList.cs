using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 執行緒安全Lisr集合
    /// </summary>
    /// <typeparam name="T">泛型型別</typeparam>
    public class SafeList<T> : IEnumerable,IEnumerable<T>
    {
        private readonly object _lock = new object();
        private List<T> _list;
        /// <summary>
        /// 傳入泛型型別集合
        /// </summary>
        /// <param name="list">泛型型別集合</param>
        public SafeList(List<T> list)
        {
            _list = list;
        }
        /// <summary>
        /// 建構子
        /// </summary>
        public SafeList()
        {
            _list = new List<T>();
        }
        /// <summary>
        /// Add Item
        /// </summary>
        /// <param name="item">泛型型別項目</param>
        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }
        /// <summary>
        /// Remove Item
        /// </summary>
        /// <param name="item">泛型型別項目</param>
        public void Remove(T item)
        {
            lock (_lock)
            {
                _list.Remove(item);
            }
        }
        /// <summary>
        /// 傳回或設定項目索引
        /// </summary>
        /// <param name="index">項目索引</param>
        /// <returns>泛型型別項目</returns>
        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _list[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    _list[index] = value;
                }
            }
        }
        /// <summary>
        /// 是否包含項目
        /// </summary>
        /// <param name="item">泛型型別項目</param>
        /// <returns>true:包含項目</returns>
        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _list.Contains(item);
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
                return _list.GetEnumerator();
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }
        /// <summary>
        /// 逐一查看列舉值
        /// </summary>
        /// <returns>列舉值</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_lock)
            {
                return _list.GetEnumerator();
            }
        }
    }
}
