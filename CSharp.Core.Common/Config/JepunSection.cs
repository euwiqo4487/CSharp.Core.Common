using System.Configuration;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp Section 區段組態檔 
    /// </summary>
    public class CSharpSection : ConfigurationSection
    {
        /// <summary>
        /// AppInfo屬性
        /// </summary>
        [ConfigurationProperty("AppInfo", DefaultValue = "WEB", IsRequired = true)]
        public string AppInfo
        {
            get
            {
                return base["AppInfo"] as string;
            }
            set
            {
                base["AppInfo"] = value;
            }
        }
        /// <summary>
        /// CultureInfo屬性
        /// </summary>
        [ConfigurationProperty("CultureInfo", DefaultValue = "zh-tw", IsRequired = true)]         
        public string CultureInfo
        {
            get
            {
                return base["CultureInfo"] as string;
            }
            set
            {
                base["CultureInfo"] = value;
            }
        }
        /// <summary>
        /// ClusterServer屬性
        /// </summary>
        [ConfigurationProperty("ClusterServer", DefaultValue = "", IsRequired = false)]
        public string ClusterServer
        {
            get
            {
                return base["ClusterServer"] as string;
            }
            set
            {
                base["ClusterServer"] = value;
            }
        }
        /// <summary>
        /// Watermark true or false
        /// </summary>
        [ConfigurationProperty("Watermark", DefaultValue = "false", IsRequired = false)]
        public string Watermark
        {
            get
            {
                return base["Watermark"] as string;
            }
            set
            {
                base["Watermark"] = value;
            }
        }
        /// <summary>
        /// ClusterRole屬性
        /// </summary>
        [ConfigurationProperty("ClusterRole", DefaultValue = "", IsRequired = false)]
        public string ClusterRole
        {
            get
            {
                return base["ClusterRole"] as string;
            }
            set
            {
                base["ClusterRole"] = value;
            }
        }
        /// <summary>
        /// Env屬性 用來區分正式或測試
        /// </summary>
        [ConfigurationProperty("Env", DefaultValue = "", IsRequired = false)]
        public string Env
        {
            get
            {
                return base["Env"] as string;
            }
            set
            {
                base["Env"] = value;
            }
        }
        /// <summary>
        /// CustomerDllName屬性
        /// </summary>
        [ConfigurationProperty("CustomerDllName", DefaultValue = "CSharp.Customer.dll", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string CustomerDllName
        {
            get
            {
                return base["CustomerDllName"] as string;
            }
            set
            {
                base["CustomerDllName"] = value;
            }
        }
        /// <summary>
        /// Mail區段
        /// </summary>
        [ConfigurationProperty("Mail")]
        public MailElement Mail
        {
            get { return base["Mail"] as MailElement; }
        }
        /// <summary>
        /// Shard區段
        /// </summary>
        [ConfigurationProperty("Shard")]
        public ShardElement Shard
        {
            get { return base["Shard"] as ShardElement; }
        }
    }
}
