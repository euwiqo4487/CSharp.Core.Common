using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// ThreadHelper
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// 不同條執行續執行一個 action,可設定 timeout
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="millisecondsTimeout">Timeout</param>
        public static void Execute(Action action, int millisecondsTimeout)
        {
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    exception = e;
                }
            });
            thread.Start();
            bool completed = thread.Join(millisecondsTimeout);
            if (!completed)
            {
                thread.Abort();
                throw new TimeoutException("ThreadHelper.Execute Timeout!!");
            }
            if (exception != null)
            {
                throw exception;
            }
        }
        /// <summary>
        /// 同一條執行續
        /// </summary>
        /// <param name="action"></param>
        /// <param name="millisecondsTimeout"></param>
        public static void CallWithTimeout(Action action, int millisecondsTimeout)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(millisecondsTimeout))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException("ThreadHelper.CallWithTimeout Timeout!!");
            }
        }
    }
}
