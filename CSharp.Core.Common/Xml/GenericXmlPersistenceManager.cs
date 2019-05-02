using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 泛型XML序列/反序列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class GenericXmlPersistenceManager<T>
    {
        /// <summary>
        /// 創建後緩存
        /// </summary>
        private static XmlSerializer factory;
        /// <summary>
        /// 檔案載入 反序列 成 T
        /// </summary>
        /// <param name="filePath">檔名路徑</param>
        /// <returns>T</returns>
        public static T LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (XmlReader inputStream = XmlReader.Create(filePath))
                {
                    return ReadFromStream(inputStream);
                }
            }
            return default(T);
        }
        /// <summary>
        /// 串流載入 反序列 成 T
        /// </summary>
        /// <param name="inputStream">串流</param>
        /// <returns>T</returns>
        public static T ReadFromStream(XmlReader inputStream)
        {
            if (factory == null) factory = new XmlSerializer(typeof(T));            
            return (T)factory.Deserialize(inputStream);
        }
        /// <summary>
        /// 存成檔案
        /// </summary>
        /// <param name="filePath">檔名路徑</param>
        /// <param name="data">物件T</param>
        public static void SaveToFile(string filePath, T data)
        {
            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                AddToStream(writer,data);
            }
        }
        /// <summary>
        /// 加到串流
        /// </summary>
        /// <param name="ouputStream">串流</param>
        /// <param name="data">物件T</param>
        public static void AddToStream(XmlWriter ouputStream, T data)
        {
            if (factory == null) factory = new XmlSerializer(typeof(T));
            factory.Serialize(ouputStream,data);
        }
    }
}
