using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common.Threading
{
    /// <summary>
    /// 有timeout的執行序鎖
    /// </summary>
    /// <example>
    /// object lockHandle = new object;
    /// using(LockHolder objecT lockObj = new LockHolder object (lockHandle,1000))
    /// {
    ///     if(lockObj.LockSuccessful)
    ///     {
    ///      //write code
    ///     }
    /// } //Dispose()
    /// </example>
    /// <typeparam name="T"></typeparam>
    public sealed class LockHolder<T> : IDisposable where T:class
    {
        private T handle;
        private bool holdsLock;
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="handle">鎖定物件</param>
        /// <param name="millisecondsTimeout">milliseconds timeout</param>
        public LockHolder(T handle, int millisecondsTimeout)
        {
            this.handle = handle;
            this.holdsLock = System.Threading.Monitor.TryEnter(this.handle, millisecondsTimeout);
        }
        /// <summary>
        /// 是否鎖定中
        /// </summary>
        public bool LockSuccessful
        {
            get { return holdsLock; }
        }
        /// <summary>
        /// 解鎖
        /// </summary>
        public void Dispose()
        {
            if (holdsLock) System.Threading.Monitor.Exit(this.handle);
            this.holdsLock = false;
        }
    }
}
