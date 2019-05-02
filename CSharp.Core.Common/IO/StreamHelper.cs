using System.IO;
using System.Text;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Stream協助靜態類別 ,如將串流轉換成位元組,反之
    /// </summary>
    public static class StreamHelper
    {        
        /// <summary>
        /// 將BYTES 轉換 Stream
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <returns>Stream</returns>
        public static Stream BytesToStream(this byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;//注意歸 0,不然後面使用時會讀不到資料
            return stream;
        }
        /// <summary>
        /// Stream To Bytes
        /// </summary>
        /// <param name="stream">stream</param>
        /// <returns>Bytes</returns>
        public static byte[] StreamToBytes(this Stream stream)
        {             
            byte[] bytes = new byte[stream.Length];
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {

                int n = stream.Read(bytes, numBytesRead, numBytesToRead > 100 ? 100 : numBytesToRead);                 
                if (n == 0)
                {
                    break;
                }
                numBytesRead += n;
                numBytesToRead -= n;
            }
            stream.Close();
            return bytes;
        }
        /// <summary>
        /// 將 Stream 轉換 字串
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="encoding">編碼</param>
        /// <returns></returns>
        public static string StreamToString(this Stream stream, Encoding encoding)
        {
            return encoding.GetString(stream.StreamToBytes());
            //stream.Position = 0;
            //using (StreamReader reader = new StreamReader(stream, encoding))
            //{
            //    return reader.ReadToEnd();
            //}
        }
        /// <summary>
        /// 將 字串 轉換 Stream
        /// </summary>
        /// <param name="aString">a String</param>
        /// <param name="encoding">編碼</param>
        /// <returns></returns>
        public static Stream StringToStream(this string aString, Encoding encoding)
        {
            return encoding.GetBytes(aString).BytesToStream();
        }
        /// <summary>
        /// 將 Stream 轉換 StreamReader
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="encoding">編碼</param>
        /// <returns></returns>
        public static StreamReader StreamToStreamReader(this Stream stream, Encoding encoding)
        {
            return new StreamReader(stream, encoding);
        }
        /// <summary>
        /// 將 Stream 轉換 StreamWriter
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="encoding">編碼</param>
        /// <returns></returns>
        public static StreamWriter StreamToStreamWriter(this Stream stream, Encoding encoding)
        {
            return new StreamWriter(stream, encoding);
        }
    }
}
