using Newtonsoft.Json;
using System.Xml;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Json協助靜態類別 ,做 物件 轉為 JSON或由JSON STRING 轉為物件 等
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 透過物件轉成JSON序列化在反序列化傳回新的物件,像是Copy的行為
        /// </summary>
        /// <typeparam name="TObj">對象</typeparam>
        /// <param name="obj">Copy的對象</param>
        /// <returns>TObj</returns>
        public static TObj CopyObj<TObj>(this object obj)
        {
            return JsonConvert.DeserializeObject<TObj>(JsonConvert.SerializeObject(obj));
        }
        /// <summary>
        /// 物件 轉為 JSON STRING
        /// </summary>
        /// <param name="obj">物件</param>           
        /// <returns>JSON STRING</returns>
        public static string ObjToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None);
        }
        /// <summary>
        /// 由JSON STRING 轉為物件
        /// </summary>
        /// <typeparam name="TObj">物件TYPE</typeparam>
        /// <param name="json">JSON STRING</param>
        /// <returns>物件</returns>
        public static TObj JsonToObj<TObj>(this string json)
        {
            return JsonConvert.DeserializeObject<TObj>(json);
        }       
        /// <summary>
        /// 是標準的  XmlDocument 轉換過來的 json string
        /// </summary>
        /// <param name="doc">XmlDocumen</param>
        /// <returns>json string</returns>
        public static string XmlToJson(this XmlNode doc)
        {
            return JsonConvert.SerializeXmlNode(doc);
        }
        /// <summary>
        /// 必須是標準的  XmlDocument 轉換過來的 json string
        /// </summary>
        /// <param name="json">標準的  XmlDocument 轉換過來的 json string</param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument JsonToXml(this string json)
        {
            return JsonConvert.DeserializeXmlNode(json);
        }
        /// <summary>
        /// 一般的 json string,即原來並非是XML結構的 轉換為 XmlReader
        /// </summary>
        /// <param name="json"> 一般的 json string,即原來並非是XML結構</param>
        /// <param name="nodeName">節點名稱</param>
        /// <returns>XmlReader</returns>
        public static XmlReader JsonToXmlReader(this string json, string nodeName)
        {
            return JsonConvert.DeserializeXNode(json, nodeName).CreateReader();
        }
        
    }
}
