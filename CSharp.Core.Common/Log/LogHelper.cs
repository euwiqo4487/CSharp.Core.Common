using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Log協助靜態類別 ,處裡Exception時,如何輸出 Log格式
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 產生LOG紀錄用的只走兩層,InnerException
        /// </summary>
        /// <param name="ex">系統Exception物件</param>
        /// <param name="faultCode">錯誤碼</param>
        /// <param name="path">服務位置</param>        
        /// <returns>LOG內容</returns>
        public static string BuildExceptionMessage(Exception ex, string faultCode = "", string path = "")
        {
            //取得更詳細的錯誤,如果存在!
            string err = FormatError(ex, faultCode, path);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                err += string.Format("{0}{1}", Environment.NewLine, FormatError(ex));
            }
            return err;
        }

        static string FormatError(Exception ex, string faultCode = "", string path = "")
        {
            var msg = new StringBuilder();
            msg.AppendLine();
            if (!string.IsNullOrEmpty(faultCode))
                msg.AppendLine("FaultCode : " + faultCode);
            if (!string.IsNullOrEmpty(path))
                msg.AppendLine("Error in Path : " + path);
            // Type of Exception
            msg.AppendLine("Type of Exception : " + ex.GetType().Name);
            // Get the error message
            msg.AppendLine("Message : " + ex.Message);
            // Source of the message
            msg.AppendLine("Source : " + ex.Source);
            // Stack Trace of the error
            msg.AppendLine("Stack Trace : ");
            msg.AppendLine(ex.StackTrace);
            // Method where the error occurred
            msg.AppendLine("TargetSite : " + ex.TargetSite);
            return msg.ToString();
        }

        /// <summary>
        /// 產生自訂LOG訊息格式
        /// </summary>
        /// <param name="reason">原因</param>
        /// <param name="faultCode">錯誤代碼</param>
        /// <param name="path">服務路徑</param>
        /// <returns>自訂LOG訊息格式</returns>
        public static string BuildCustomMessage(string reason, string faultCode, string path)
        {
            var msg = new StringBuilder();
            msg.AppendLine();
            msg.AppendLine("FaultCode : " + faultCode);
            msg.AppendLine("Error in Path : " + path);
            msg.AppendLine("Message : " + reason);
            return msg.ToString();
        }

        /// <summary>
        /// Exception Logger
        /// 非NLog, 自訂Logger專門寫log於"Assembly\App_Data\Logs\xxx.txt"
        /// </summary>
        /// <param name="ex">錯誤</param>
        /// <param name="faultCode">自訂錯誤代碼</param>
        /// <param name="path">自訂錯誤來源</param>
        /// <param name="logFileName">log檔名稱</param>
        public static void AppLogException(Exception ex, string faultCode = "", string path = "", string logFileName = "")
        {
            var tmp = BuildExceptionMessage(ex, faultCode, path);
            if (!string.IsNullOrWhiteSpace(tmp))
                AppendAppLog(tmp, logFileName);
        }

        /// <summary>
        /// Message Logger
        /// 非NLog, 自訂Logger專門寫log於"Assembly\App_Data\Logs\xxx.txt"
        /// </summary>
        /// <param name="message">訊息</param>
        /// <param name="logFileName">log檔名稱</param>
        public static void AppLogMessage(string message, string logFileName = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
                AppendAppLog(message, logFileName);
        }

        /// <summary>
        /// Format Message Logger
        /// 非NLog, 自訂Logger專門寫log於"Assembly\App_Data\Logs\xxx.txt"
        /// </summary>
        /// <param name="reason">自訂原因</param>
        /// <param name="faultCode">自訂代碼</param>
        /// <param name="path">自訂來源</param>
        /// <param name="logFileName">log檔名稱</param>
        public static void AppLogCustom(string reason, string faultCode, string path, string logFileName = "")
        {
            var tmp = BuildCustomMessage(reason, faultCode, path);
            if (!string.IsNullOrWhiteSpace(tmp))
                AppendAppLog(tmp, logFileName);
        }

        static ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        static void AppendAppLog(string message, string logFileName = "")
        {
            string tmp = Assembly.GetEntryAssembly().Location,
                    name = Path.GetFileNameWithoutExtension(tmp) + "_AppLog.txt",
                    path = Path.GetDirectoryName(tmp),
                    path2 = Path.Combine(path, "App_Data", "Logs", string.Format("{0:yyyy-MM-dd}", DateTime.Now)),
                    fname = Path.Combine(path2, string.IsNullOrWhiteSpace(logFileName) ? name : logFileName);
            var fi = new FileInfo(fname);
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            Task.Run(() =>
            {
                string msg = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} | {1}{2}", DateTime.Now, message, Environment.NewLine);
                byte[] buf = Encoding.Default.GetBytes(msg);
                lock_.EnterWriteLock();
                try
                {
                    using (var fs = new FileStream(fname, FileMode.Append, FileAccess.Write, FileShare.None, 4096, false))
                        fs.Write(buf, 0, buf.Length);
                }
                finally
                {
                    lock_.ExitWriteLock();
                }
            });
        }
    }
}
