using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Core.Common.Threading
{
    /// <summary>
    /// producer/consumer queue
    /// </summary>
    public class PCQueue
    {
        readonly object _locker = new object();
        Thread[] _workers;
        Queue<Action> _itemQ = new Queue<Action>();
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="workerCount">生成Thread數目</param>
        public PCQueue(int workerCount)
        {
            _workers = new Thread[workerCount];            
            for (int i = 0; i < workerCount; i++)
                (_workers[i] = new Thread(Consume)).Start();
        }
        /// <summary>
        /// 關閉PCQueue
        /// </summary>
        /// <param name="waitForWorkers">是否等待完成</param>
        public void Shutdown(bool waitForWorkers)
        {
            // Enqueue one null item per worker to make each exit.
            foreach (Thread worker in _workers)
                EnqueueItem(null);             
            //Wait for workers to finish
            if (waitForWorkers)
                foreach (Thread worker in _workers) 
                    worker.Join();           
        }
        /// <summary>
        /// 加入工作
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueItem(Action item)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(item); // We must pulse because we're changing a blocking condition.
                Monitor.Pulse(_locker); // 解鎖定 _locker 但  停止阻塞
            }
        }
        void Consume()
        {
            while (true) 
            { 
                Action item;
                lock (_locker)
                {
                    while (_itemQ.Count == 0) Monitor.Wait(_locker);   // 解鎖定 _locker使起可以EnqueueItem 但 阻塞                     
                    item = _itemQ.Dequeue();
                }
                if (item == null) return; // This signals our exit.
                item(); // Execute item.                
            }
        }
    }
}
