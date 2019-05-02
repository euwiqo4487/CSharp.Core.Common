using System;
using System.Configuration;
using System.Text;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp.Config 的 Mail Element 設定區段 
    /// </summary>
    public class MailElement : ConfigurationElement
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public MailElement()
        {
        }
        /// <summary>
        /// 郵件設定參數
        /// </summary>
        /// <param name="smtpServer">Smtp Server</param>
        /// <param name="port">port</param>
        /// <param name="timeOut">單位:ms</param>
        /// <param name="interval">單位:ms</param>
        /// <param name="authenticate">0:不驗證 1:Basic驗證  2:NTLM驗證</param>
        /// <param name="userName">使用者ID</param>
        /// <param name="passWord">使用者密碼</param>
        /// <param name="encoding">編碼</param>
        /// <param name="leftTag">資料取代字串左方Tag符號</param>
        /// <param name="rightTag">資料取代字串右方Tag符號</param>
        /// <param name="enableSSL">enableSSL</param>
        public MailElement(string smtpServer, int port, int timeOut, double interval, string authenticate, string userName, string passWord, string encoding, string leftTag, string rightTag,bool enableSSL)
        {
            //this.FromDisplay = fromDisplay;
            //this.From = from;
            this.SmtpServer = smtpServer;
            this.Port = port;
            this.TimeOut = timeOut;
            this.Interval = interval;
            this.Authenticate = authenticate;
            this.UserName = userName;
            this.PassWord = passWord;
            this.Encoding = encoding;
            //this.ContentFilePath = contentFilePath;
            //this.ContentFileName = contentFileName;
            this.LeftTag = leftTag;
            this.RightTag = rightTag;
            this.EnableSSL = enableSSL;
        }
        
        //[ConfigurationProperty("FromDisplay", DefaultValue = "捷鵬國際", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        // public String FromDisplay
        //{
        //    get
        //    { 
        //        return (string)base["FromDisplay"]; 
        //    }
        //    set
        //    { 
        //        base["FromDisplay"] = value; 
        //    }
        //}

        //[ConfigurationProperty("From", DefaultValue = "srv@mail.CSharp.com.tw", IsRequired = true)]
        //[StringValidator(MinLength = 1, MaxLength = 60)]
        //public String From
        //{
        //    get
        //    { 
        //        return (string)base["From"]; 
        //    }
        //    set
        //    { 
        //        base["From"] = value; 
        //    }
        //}
        /// <summary>
        /// SmtpServer
        /// </summary>
        [ConfigurationProperty("SmtpServer", DefaultValue = "mail.CSharp.com.tw", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 60)]
        public String SmtpServer
        {
            get
            {
                return (string)base["SmtpServer"];
            }
            set
            {
                base["SmtpServer"] = value;
            }
        }

        /// <summary>
        /// 數字不要指定 StringValidator
        /// </summary>
        [ConfigurationProperty("Port", DefaultValue = "25", IsRequired = true)]        
        public int Port
        {
            get
            {
                return (int)base["Port"];
            }
            set
            {
                base["Port"] = value;
            }
        }

        /// <summary>
        /// 數字不要指定 StringValidator
        /// </summary>
        [ConfigurationProperty("TimeOut", DefaultValue = "6000", IsRequired = true)]        
        public int TimeOut
        {
            get
            {
                return (int)base["TimeOut"];
            }
            set
            {
                base["TimeOut"] = value;
            }
        }     
   
        /// <summary>
        /// 數字不要指定 StringValidator
        /// </summary>
        [ConfigurationProperty("Interval", DefaultValue = "60000", IsRequired = true)]
        public double Interval
        {
            get
            {
                return (double)base["Interval"];
            }
            set
            {
                base["Interval"] = value;
            }
        }

        /// <summary>
        /// 預設 SMTP 驗證   0:不驗證 1:Basic驗證  2:NTLM驗證
        /// </summary>
        [ConfigurationProperty("Authenticate", DefaultValue = "0", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 1)]
        public String Authenticate
        {
            get
            {
                return (string)base["Authenticate"];
            }
            set
            {
                base["Authenticate"] = value;
            }
        }
        /// <summary>
        /// UserName
        /// </summary>
        [ConfigurationProperty("UserName", DefaultValue = "")]
        [StringValidator(MinLength = 0, MaxLength = 255)]       
        public String UserName
        {
            get
            {
                return (string)base["UserName"];
            }
            set
            {
                base["UserName"] = value;
            }
        }
        /// <summary>
        /// PassWord
        /// </summary>
        [ConfigurationProperty("PassWord", DefaultValue = "")]
        [StringValidator(MinLength = 0, MaxLength = 255)]       
        public string PassWord
        {
            get
            {
                return (string)base["PassWord"];
            }
            set
            {
                base["PassWord"] = value;
            }
        }
        /// <summary>
        /// Encoding
        /// </summary>
        [ConfigurationProperty("Encoding", DefaultValue = "utf-8", IsRequired = true)]
        [StringValidator(MinLength = 3, MaxLength = 7)]       
        public string Encoding
        {
            get
            {               
                return (string)base["Encoding"];
            }
            set
            {
                base["Encoding"] = value;
            }
        }

        //[ConfigurationProperty("ContentFilePath", DefaultValue = "D:\\MailLibrary\\Template\\", IsRequired = true)]
        //[StringValidator(MinLength = 0, MaxLength = 100)]
        //public string ContentFilePath
        //{
        //    get
        //    {
        //        return (string)base["ContentFilePath"];
        //    }
        //    set
        //    {
        //        base["ContentFilePath"] = value;
        //    }
        //}

        //[ConfigurationProperty("ContentFileName", DefaultValue = "")]
        //[StringValidator(MinLength = 0, MaxLength = 60)]
        //public string ContentFileName
        //{
        //    get
        //    {
        //        return (string)base["ContentFileName"];
        //    }
        //    set
        //    {
        //        base["ContentFileName"] = value;
        //    }
        //}
        /// <summary>
        /// LeftTag
        /// </summary>
        [ConfigurationProperty("LeftTag", DefaultValue = "{", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 5)]        
        public string LeftTag
        {
            get
            {
                return (string)base["LeftTag"];
            }
            set
            {
                base["LeftTag"] = value;
            }
        }
        /// <summary>
        /// RightTag
        /// </summary>
        [ConfigurationProperty("RightTag", DefaultValue = "}", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 5)]
        public string RightTag
        {
            get
            {
                return (string)base["RightTag"];
            }
            set
            {
                base["RightTag"] = value;
            }
        }
    
        //[ConfigurationProperty("SendList")]
        //public SendList SendList
        //{
        //    get { return (SendList)base["SendList"]; }
        //}
        /// <summary>
        /// EnableSSL
        /// </summary>
        [ConfigurationProperty("EnableSSL", DefaultValue = false)]         
        public bool EnableSSL
        {
            get
            {
                return (bool)base["EnableSSL"];
            }
            set
            {
                base["EnableSSL"] = value;
            }
        }
    }
}
