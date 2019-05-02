using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Xsl包裝類別 ,輸出xsl樣板 加入參數後的長相
    /// </summary>
    public class XslAdapter:IDisposable
    {
        private string xmlMarkup;
        private XmlDocument doc = new XmlDocument();
        private XsltArgumentList argList = new XsltArgumentList();
        XsltSettings settings = new XsltSettings(false, true);
        XslCompiledTransform xslt = new XslCompiledTransform();
        /// <summary>
        /// XSLT 包裝物件  主要用途為NEW 一次後反覆使用 ,例如 參數改變
        /// </summary>
        public XslAdapter():this("<empty_root></empty_root>"){}         
        /// <summary>
        /// XSLT 包裝物件 主要用途為NEW 一次後反覆使用 ,例如 參數改變
        /// </summary>
        /// <param name="xmlMarkup">XML字串</param>
        public XslAdapter(string xmlMarkup) 
        {
            this.xmlMarkup = xmlMarkup;            
            doc.LoadXml(this.xmlMarkup);
        }
        /// <summary>
        /// 載入XSLT實體檔案位置
        /// </summary>
        /// <example>
        /// <code language="cs" title="XSLT 於 xml用法和傳入參數">
        /// string xml = "<order><book ISBN='10-861003-324'><title>The Handmaid's Tale</title><price>19.95</price></book><cd ISBN='2-3631-4'><title>Americana</title><price>16.95</price></cd></order>";
        /// XslAdapter xsla = new XslAdapter(xml);
        /// xsla.LoadByUri(FileHelper.GetAppPath() + "discount.xsl");
        /// DateTime orderDate = new DateTime(2004, 01, 15);
        /// DateTime discountDate = orderDate.AddDays(20);
        /// xsla.AddParam("discount", discountDate.ToString());
        /// Console.WriteLine(xsla.Transform("utf-8"));
        /// xsla.Clear();
        /// discountDate = orderDate.AddDays(130);
        /// xsla.AddParam("discount", discountDate.ToString());
        /// Console.WriteLine(xsla.Transform("utf-8"));
        /// </code>
        /// </example>
        /// <param name="stylesheetUri">xsl樣板Uri位置</param>
        public void LoadByUri(string stylesheetUri)
        {
            xslt.Load(stylesheetUri, settings, new XmlUrlResolver());
        }
        /// <summary>
        /// Load By Markup 傳入xsl樣板
        /// </summary>
        /// <param name="xslMarkup">xsl樣板長相</param>
        public void LoadByMarkup(string xslMarkup)
        {
            xslt.Load(XmlReader.Create(new StringReader(xslMarkup)), settings, new XmlUrlResolver());
        }
        /// <summary>
        /// 可改變Xml字串
        /// </summary>
        public string Xml
        {
            set
            {
                this.xmlMarkup = value;
                if(String.IsNullOrEmpty(value))
                {
                    this.xmlMarkup = "<empty_root></empty_root>";
                }                
                doc.LoadXml(this.xmlMarkup);
            }
        }  
        /// <summary>
        /// 將 XSLT 參數加入至 XsltArgumentList
        /// </summary>
        /// <param name="name">參數名</param>
        /// <param name="parameter">System.String System.Boolean System.Double 等</param>
        /// <param name="namespaceUri">命名空間 URI</param>
        public void AddParam(string name,object parameter,string namespaceUri = "")
        {
            argList.AddParam(name, namespaceUri,parameter);
        }
        /// <summary>
        /// 移除參數
        /// </summary>
        /// <param name="name">參數名</param>
        /// <param name="namespaceUri">命名空間 URI</param>
        public void RemoveParam(string name, string namespaceUri = "")
        {
            argList.RemoveParam(name, namespaceUri);
        }
        /// <summary>
        /// 清除參數
        /// </summary>
        public void Clear()
        {
            argList.Clear();
        }
        /// <summary>
        /// 轉換XSLT和XML 
        /// </summary>
        /// <param name="encoding">編碼</param>
        /// <returns>文件</returns>
        public string Transform(string encoding = "utf-8")
        {
            return Transform(Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 轉換XSLT和XML 
        /// </summary>
        /// <param name="encoding">編碼</param>
        /// <returns>文件</returns>
        public string Transform(Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                xslt.Transform(doc, argList, stream);
                stream.Position = 0;//回到位置0
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
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
        /// 關閉資源
        /// </summary>
        /// <param name="disposing"> true:要釋放資源 false:要釋放資源</param>
        protected virtual void Dispose(bool disposing)
        {
            argList = null;
            settings = null;
            doc = null;
            xslt = null;
        }
    }
}
