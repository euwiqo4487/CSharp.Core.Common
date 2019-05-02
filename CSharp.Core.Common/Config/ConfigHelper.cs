using System;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Config協助靜態類別,判斷取得WEB或WINDOW類型的 Configuration
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// 自動判斷取得WEB或WINDOW類型的 Configuration
        /// </summary>
        /// <returns>WEB或WINDOW類型的 Configuration</returns>
        public static System.Configuration.Configuration AutoGetConfig()
        {
            System.Configuration.Configuration config = ConfigHelper.GetAppConfig();
            if (config == null)
            {
                // App.config 開啟方式和Web.config不同,TYPE定義連組件名稱和版本和文化和PublicKeyToken都要加上去
                config = ConfigHelper.GetWebConfig();
            }
            return config;
        }
        /// <summary>
        /// 取得App.config 組態
        /// </summary>
        /// <returns>WINDOW類型的 Configuration</returns>
        public static System.Configuration.Configuration GetAppConfig()
        {
            try
            {
                return System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            }
            catch
            {//找不到App.config
                return null;
            }
        }
        /// <summary>
        /// 取得 Web.config 組態
        /// </summary>
        /// <returns> WEB 類型的 Configuration</returns>
        public static System.Configuration.Configuration GetWebConfig()
        {
            try
            {
                return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
