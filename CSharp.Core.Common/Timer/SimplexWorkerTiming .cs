using System;
using System.Timers;


namespace CSharp.Core.Common
{
    /// <summary>
    /// 工作排程計時器,做完在做下一個工作,會開一條新的執行緒
    /// </summary>
    public class SimplexWorkerTiming : IDisposable
    {
        private bool isStart = false;
        private Timer aTimer;
        /// <summary>
        /// WorkAction 執行內容
        /// </summary>
        public Action<ElapsedEventArgs> WorkAction;
        /// <summary>
        /// 建構子
        /// </summary>
        public SimplexWorkerTiming() : this(100) { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="interval">時間間隔 毫秒</param>
        public SimplexWorkerTiming(double interval)
        {
            aTimer = new Timer(interval);
            aTimer.AutoReset = false; //不自動引發,要靠自己控制
            aTimer.Enabled = false;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                WorkAction(e);
            }
            catch { }
            finally
            {
                if(isStart) Start();//啟動下一次
            }
        }
        //public bool AutoReset
        //{
        //    get
        //    {
        //        return aTimer.AutoReset;
        //    }
        //    set
        //    {
        //        aTimer.AutoReset = value;
        //    }
        //}
        //public bool Enabled
        //{
        //    get
        //    {
        //        return aTimer.Enabled;
        //    }
        //    set
        //    {
        //        aTimer.Enabled = value;
        //    }
        //}
        /// <summary>
        /// 取得或設定引發 Elapsed 事件的間隔 (單位為毫秒)
        /// </summary>
        public double Interval
        {
            get
            {
                return aTimer.Interval;
            }
            set
            {
                aTimer.Interval = value;
            }
        }
        /// <summary>
        /// Start
        /// </summary>
        /// <example>
        /// <code language="cs" title="工作排程計時器">
        /// SimplexWorkerTiming aTimer = new SimplexWorkerTiming(1000); //間隔1秒
        /// aTimer.WorkAction = (e) =>
        /// {
        ///      Console.WriteLine("工作5秒");
        ///      Thread.Sleep(5000);
        /// };
        /// aTimer.Start();
        /// Console.ReadLine();
        /// </code>
        /// </example>
        public void Start()
        {
            isStart = true;
            aTimer.Start();
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            isStart = false;
            aTimer.Stop();
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing"> true:要釋放資源 false:要釋放資源</param>
        protected virtual void Dispose(bool disposing)
        {
            WorkAction = null;
            aTimer.Close();
            aTimer.Dispose();
        }
    }
}
