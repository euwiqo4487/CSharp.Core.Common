using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 執行CMD視窗指令無畫面
    /// </summary>
    public class CmdProcess:IDisposable
    {
        string outputData;
        /// <summary>
        /// Process
        /// </summary>
        private Process myProcess;        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="fileName">執行檔</param>
        /// <param name="args">參數</param>        
        public CmdProcess(string fileName, string args = "")
        {             
            myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;            
            myProcess.StartInfo.FileName = fileName;
            myProcess.StartInfo.Arguments = args;            
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.ErrorDialog = false;
            myProcess.StartInfo.RedirectStandardError = true;
            myProcess.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
        }
        /// <summary>
        /// 提供參數設定
        /// </summary>
        public string Arguments
        {
            set
            {
                myProcess.StartInfo.Arguments = value;
            }
        }
        private void ErrorDataHandler(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {                 
                this.ErrorData += Environment.NewLine + e.Data;//Console.Error.WriteLine
            }
        }
        private void OutputDataHandler(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                this.AllOutputData += Environment.NewLine + e.Data;
                this.outputData = e.Data;//只給最後一筆有資料顯示的,因為所有的  Console.WriteLine都會顯示出來
            }
        }
        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="millisecondsTimeout">Timeout</param>
        public void Run(int millisecondsTimeout = 0)
        {                         
            myProcess.Start();//start                      
            myProcess.BeginOutputReadLine();
            myProcess.BeginErrorReadLine();
            if (millisecondsTimeout != 0)
            {
                myProcess.WaitForExit(millisecondsTimeout);//等待完成
            }
            else
            {
                myProcess.WaitForExit();//等待完成
            }               
        }
        /// <summary>
        /// 取得相關處理序終止時指定的值
        /// </summary>
        public int ExitCode
        {
            get
            {
                return myProcess.ExitCode;
            }
        }
        /// <summary>
        /// AllOutputData
        /// </summary>
        public string AllOutputData
        {
            get;
            private set;
        }
        /// <summary>
        /// OutputData
        /// </summary>
        public string OutputData
        {
            get
            {
                return this.outputData;
                //return CsvHelper.ReadLines(this.outputData).LastOrDefault(s => !string.IsNullOrEmpty(s));           
            }             
        }
        /// <summary>
        /// ErrorData
        /// </summary>
        public string ErrorData
        {
            get;
            private set;
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
        /// 實作Dispose
        /// </summary>
        /// <param name="disposing">true of false 皆將queue關閉</param>
        protected virtual void Dispose(bool disposing)
        {
            if (myProcess != null) myProcess.Close();           
        }
    }
}
