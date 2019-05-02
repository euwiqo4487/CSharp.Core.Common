using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// File協助靜態類別 ,如 base64字串 轉換成 串流或檔案轉換成位元組 等IO操作
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Base64 String To Stream
        /// </summary>
        /// <param name="base64String">base64字串</param>
        /// <returns>Stream</returns>
        public static Stream Base64StringToStream(this string base64String)
        {
            return base64String.Base64StringToBytes().BytesToStream();
        }
        /// <summary>
        /// Base64 String To File
        /// </summary>
        /// <param name="base64String">base64字串</param>
        /// <param name="pathNew">檔名含路徑</param>
        public static void Base64StringToFile(this string base64String,string pathNew)
        {
           BytesToFile(pathNew,base64String.Base64StringToBytes());
        }
        /// <summary>
        /// File To Base 64 String
        /// </summary>
        /// <param name="pathSource">檔名含路徑</param>
        /// <returns>base64字串</returns>
        public static string FileToBase64String(string pathSource)
        {
            return FileToBytes(pathSource).BytesToBase64String();
        }         
        /// <summary>
        /// File To Bytes
        /// </summary>
        /// <example>
        /// <code language="cs" title="檔案和位元組">
        /// FileHelper.BytesToFile("d:\\test.pdf", FileHelper.FileToBytes("D:\\codes\\PDF\\The Art of Readable Code.pdf"));
        /// </code>
        /// </example>
        /// <param name="pathSource">檔名含路徑</param>
        /// <returns>位元組</returns>
        public static byte[] FileToBytes(string pathSource)
        {
            byte[] bytes;
            using (FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                    // Break when the end of the file is reached.
                    if (n == 0) break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
            }
            return bytes;
        }
        /// <summary>
        /// File To Stream
        /// </summary>
        /// <param name="pathSource">檔名含路徑</param>
        /// <returns>Stream</returns>
        public static Stream FileToStream(string pathSource)
        {             
            return FileToBytes(pathSource).BytesToStream();
        }
        /// <summary>
        /// Bytes To File
        /// </summary>
        /// <param name="pathNew">檔名含路徑</param>
        /// <param name="bytes">Bytes</param>
        public static void BytesToFile(string pathNew, byte[] bytes)
        {
            int numBytesToRead = bytes.Length;
            using (FileStream fsNew = new FileStream(pathNew,FileMode.Create, FileAccess.Write))
            {
                fsNew.Write(bytes, 0, numBytesToRead);
            }
        }
        /// <summary>
        /// 確認檔案是否存在
        /// </summary>
        /// <param name="fileName">檔案名稱 如  test.dll </param>
        /// <param name="path">路徑 如 D:\\test\\ 不輸入時 使用 System.AppDomain.CurrentDomain.BaseDirectory  </param>
        /// <returns> true:存在 </returns>
        public static bool IsExist(string fileName, string path = "")
        {
            string appPath = path;
            if (String.IsNullOrEmpty(appPath))
            {
                appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            }
            return File.Exists(appPath + fileName);            
        }
        /// <summary>
        /// 取得App執行路徑
        /// </summary>
        /// <returns>App執行路徑  如 D:\\test\\ </returns>
        public static string GetAppPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
             
    }
}
