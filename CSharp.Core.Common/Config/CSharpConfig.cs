using System;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp Config  組態檔
    /// </summary>
    public static class CSharpConfig
    {
        private static System.Configuration.Configuration config;
        private static CSharpSection JS;         
        static CSharpConfig()
        {
            config = ConfigHelper.AutoGetConfig();
            JS = GetSection<CSharpSection> ();
        }
        /// <summary>
        /// Web or Win
        /// </summary>
        /// <example>
        /// <code language="cs" title="取得CSharp設定檔資訊">
        /// Console.WriteLine(CSharpConfig.AppInfo);
        /// Console.WriteLine(CSharpConfig.Mail.SmtpServer);
        /// </code>
        /// </example>
        public static string AppInfo
        {
            get
            {
                return JS.AppInfo;
            }
            //set
            //{
            //    JS.CultureInfo = value;
            //}
        }
        /// <summary>
        /// 文化特性
        /// </summary>
        public static string CultureInfo
        {
            get
            {
                return JS.CultureInfo;
            }
            //set
            //{
            //    JS.CultureInfo = value;
            //}
        }
        /// <summary>
        /// 文化特性
        /// </summary>
        public static string Env
        {
            get
            {
                return JS.Env;
            }           
        }
        /// <summary>
        /// 是否使用浮水印    true:使用  false:不使用
        /// </summary>
        public static string Watermark
        {
            get
            {
                return JS.Watermark;
            }
        }         
        /// <summary>
        /// 客製化dll名稱
        /// </summary>
        public static string CustomerDllName
        {
            get
            {
                return JS.CustomerDllName;
            }
            //set
            //{
            //    JS.CustomerDllName = value;
            //}
        }
        /// <summary>
        /// 叢集伺服器名稱
        /// </summary>
        public static string ClusterServer
        {
            get
            {
                return JS.ClusterServer;
            }
        }
        /// <summary>
        /// 叢集角色
        /// </summary>
        public static string ClusterRole
        {
            get
            {
                return JS.ClusterRole;
            }
        }
        /// <summary>
        /// Mail 設定
        /// </summary>
        public static MailElement Mail { 
            get {return JS.Mail;} 
        }
        /// <summary>
        /// Shard 設定
        /// </summary>
        public static ShardElement Shard
        {
            get { return JS.Shard; }
        }      
        /// <summary>
        /// 由Section Name區段名稱 取得CSharpSection
        /// </summary>
        /// <typeparam name="TSection">型別為 ConfigurationSection 類別</typeparam>       
        /// <returns>CSharpSection</returns>
        private static TSection GetSection<TSection>()  where TSection : System.Configuration.ConfigurationSection
        {
            try
            {
                return config.GetSection("CSharp") as TSection;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
