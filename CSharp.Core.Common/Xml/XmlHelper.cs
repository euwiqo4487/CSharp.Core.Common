using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Xml協助靜態類別 ,如將對象序列化為XML或由XML序列化為對象和XSL轉換編譯
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 將一個對象序列化為XML字串
        /// </summary>
        /// <param name="obj">要序列化的對象</param>
        /// <param name="encoding">編碼方式</param>
        /// <returns>序列化產生的XML字串</returns>
        public static string XmlSerialize(this object obj, string encoding = "utf-8")
        {
           return XmlSerialize(obj, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 將一個對象序列化為XML字串
        /// </summary>
        /// <param name="obj">要序列化的對象</param>
        /// <param name="encoding">編碼方式</param>
        /// <returns>序列化產生的XML字串</returns>
        public static string XmlSerialize(this object obj, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, obj, encoding);
                stream.Position = 0;//回到位置0
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 將一個對象按XML序列化的方式写入到一個文件
        /// </summary>
        /// <param name="obj">要序列化的對象</param>
        /// <param name="path">保存文件路徑</param>
        /// <param name="encoding">編碼方式</param>
        public static void XmlSerializeToFile(this object obj, string path, string encoding = "utf-8")
        {
            XmlSerializeToFile(obj, path, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 將一個對象按XML序列化的方式写入到一個文件
        /// </summary>
        /// <param name="obj">要序列化的對象</param>
        /// <param name="path">保存文件路徑</param>
        /// <param name="encoding">編碼方式</param>
        public static void XmlSerializeToFile(this object obj, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, obj, encoding);
            }
        }
        /// <summary>
        /// 从XML字串中反序列化對象
        /// </summary>
        /// <typeparam name="T">结果對象類型</typeparam>
        /// <param name="xmlMarkup">包含對象的XML字串</param>
        /// <param name="encoding">編碼方式</param>
        /// <returns>反序列化得到的對象</returns>
        public static T XmlDeserialize<T>(this string xmlMarkup, string encoding = "utf-8")
        {
            return XmlDeserialize<T>(xmlMarkup, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 从XML字串中反序列化對象
        /// </summary>
        /// <typeparam name="T">结果對象類型</typeparam>
        /// <param name="xmlMarkup">包含對象的XML字串</param>
        /// <param name="encoding">編碼方式</param>
        /// <returns>反序列化得到的對象</returns>
        public static T XmlDeserialize<T>(this string xmlMarkup, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlMarkup))
                throw new ArgumentNullException("xml");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xmlMarkup)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }
        /// <summary>
        /// 讀入一個文件，按XML的方式反序列化對象。
        /// </summary>
        /// <typeparam name="T">结果對象類型</typeparam>
        /// <param name="path">文件路徑</param>
        /// <param name="encoding">編碼方式</param>
        /// <returns>反序列化得到的對象</returns>
        public static T XmlDeserializeFromFile<T>(string path, string encoding = "utf-8")
        {
            return XmlDeserializeFromFile<T>(path, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 讀入一個文件，按XML的方式反序列化對象。
        /// </summary>
        /// <typeparam name="T">结果對象類型</typeparam>
        /// <param name="path">文件路徑</param>
        /// <param name="encoding">編碼方式</param>
        /// <returns>反序列化得到的對象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xmlMarkup = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xmlMarkup, encoding);
        }
        private static void XmlSerializeInternal(Stream stream, object obj, Encoding encoding)
        {
            if (obj == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, obj);
                //writer.Close();
            }
        }
        //======================XSLT 
        /// <summary>
        /// XSL轉換編譯
        /// </summary>
        /// <param name="xmlMarkup">xml資料格式</param>
        /// <param name="stylesheet">XSLT檔案位置 或 長相</param>       
        /// <param name="argList">XSLT 參數</param>
        /// <param name="encoding">編碼,預設UTF-8</param>
        /// <returns>XSL轉換編譯後的格式</returns>
        public static string XslTrans(string xmlMarkup, string stylesheet, XsltArgumentList argList = null, string encoding = "utf-8")
        {
            return XslTrans(xmlMarkup, stylesheet, argList, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// XSL轉換編譯
        /// </summary>
        /// <param name="xmlMarkup">xml</param>
        /// <param name="stylesheet">XSLT檔案位置 或 長相</param>       
        /// <param name="argList">XSLT 參數</param>
        /// <param name="encoding">編碼 </param>
        /// <returns>XSL轉換編譯後的格式</returns>
        public static string XslTrans(string xmlMarkup, string stylesheet, XsltArgumentList argList, Encoding encoding)
        {
            XPathDocument xPath;
            XmlReader doc = GetXmlReader(xmlMarkup);
            if (File.Exists(stylesheet))
            {//XSLT檔案位置
                xPath = new XPathDocument(stylesheet);
            }
            else
            {//XSLT  長相
                xPath = new XPathDocument(GetXmlReader(stylesheet));
            }
            return XslTransInternal(doc,xPath, argList, encoding);
        }       
        /// <summary>
        /// 將 DataSet資料 XSL轉換編譯後的格式
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="stylesheet">XSLT檔案位置 或 長相</param>
        /// <param name="encoding">編碼,預設UTF-8</param>
        /// <returns>XSL轉換編譯後的格式</returns>
        public static string XslTrans(this DataSet ds, string stylesheet,string encoding = "utf-8")
        {
            return XslTrans(ds.GetXml(), stylesheet,null, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 將 DataSet資料 XSL轉換編譯後的格式
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="stylesheetUri">XSLT檔案位置 或 長相</param>
        /// <param name="encoding">編碼</param>
        /// <returns>XSL轉換編譯後的格式</returns>
        public static string XslTrans(this DataSet ds, string stylesheetUri, Encoding encoding)
        {
            return XslTrans(ds.GetXml(), stylesheetUri,null, encoding);
        }
        /// <summary>
        /// DataTable資料 和 XSLT格式  XSL轉換編譯後的格式
        /// </summary>
        /// <param name="table"> DataTable </param>
        /// <param name="stylesheetUri">XSLT檔案位置 或 長相</param>
        /// <param name="encoding">編碼,預設UTF-8</param>
        /// <returns>XSL轉換編譯後的格式</returns>
        public static string XslTrans(this DataTable table, string stylesheetUri,string encoding = "utf-8")
        {
            return XslTrans(table,stylesheetUri, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// DataTable資料 和 XSLT格式  XSL轉換編譯後的格式
        /// </summary>
        /// <param name="table"> DataTable </param>
        /// <param name="stylesheet">XSLT檔案位置 或 長相</param>
        /// <param name="encoding">編碼</param>
        /// <returns>XSL轉換編譯後的格式</returns>
        public static string XslTrans(this DataTable table, string stylesheet, Encoding encoding)
        {
            string xmlMarkup = XmlSerialize(table, encoding);
            return XslTrans(xmlMarkup, stylesheet, null, encoding);
        }
        /// <summary>
        /// XSL轉換編譯
        /// </summary>
        /// <param name="doc">XmlReader</param>
        /// <param name="xPath">XSLT檔案位置 或 長相</param>       
        /// <param name="argList">XSLT 參數</param>
        /// <param name="encoding">編碼</param>
        /// <returns>XSL轉換編譯後的格式</returns>
        private static string XslTransInternal(this XmlReader doc, XPathDocument xPath, XsltArgumentList argList, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Create the XsltSettings object with script enabled.
                XsltSettings settings = new XsltSettings(false, true);
                // Create the XslCompiledTransform object and load the style sheet.
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xPath, settings, new XmlUrlResolver());
                xslt.Transform(doc, argList, stream);
                stream.Position = 0;//回到位置0
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }   
        /// <summary>
        /// 取得XmlReader
        /// </summary>
        /// <param name="makeup">XML 或 XSLT 長相</param>
        /// <returns>XmlReader</returns>
        public static XmlReader GetXmlReader(string makeup)
        {
            return XmlReader.Create(new StringReader(makeup));
        }

        static string XmlToXslt(XmlDocument xmlDoc, string strXSLTPath)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xtw = new XmlTextWriter(sw);
            xtw.Formatting = Formatting.Indented;
            xtw.Indentation = 3;
            xtw.IndentChar = ' ';
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            xslt.Load(strXSLTPath);
            xslt.Transform(xmlDoc, xtw);
            xtw.Close();
            return sw.ToString();
        }
        /// <summary>
        /// Data To HTML
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="tempfile">樣式檔</param>
        /// <returns>網頁格式</returns>
        public static string DataToHTML(DataSet ds, string tempfile)
        {
            //取得XML
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(ds.GetXml());
            //以XSLT檔進行轉換取得HTML
            string html = "<html><head><meta http-equiv=content-type content=\"text/html; charset=UTF-8\"></head><body>" + XmlToXslt(xml, tempfile) + "</body></html>";
            //html = html.Replace(" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" xmlns:user=\"urn:my-scripts\"", "");
            return html;
        }
    }
}
