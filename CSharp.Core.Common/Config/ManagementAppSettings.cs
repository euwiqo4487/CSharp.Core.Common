using CSharp.Core.Common.Resource;
using System;
using System.Collections;
using System.Configuration;
using System.IO;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Management AppSettings 協助靜態類別 ,用於管理組態檔AppSettings區段
    /// </summary>
    public static class ManagementAppSettings
    {
        /// <summary>
        /// 用來放AppSettings裡的值 (安全執行緒)
        /// </summary>
        private static readonly Hashtable AppSettingSettingCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// GetVal
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="isCache">是否快取</param>         
        /// <returns>值</returns>
        public static String GetVal(string key,bool isCache = true )
        {
            string cacheKey = key.ToUpper();//轉大寫
            if (isCache && AppSettingSettingCache.Contains(cacheKey))
            {
                try
                {
                    return (String)AppSettingSettingCache[cacheKey];
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(string.Format(ErrorResource.HashtableError, cacheKey), ex);
                }
            }
            else
            {
                try
                {
                    AppSettingSettingCache[cacheKey] = ConfigurationManager.AppSettings[cacheKey];
                    return (String)AppSettingSettingCache[cacheKey];
                }
                catch (ConfigurationErrorsException ex)
                {
                    throw new InvalidOperationException(string.Format(ErrorResource.AppSettingNotFound, cacheKey), ex);                   
                }                
            }             
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <example>
        /// <code language="cs" title="組態檔AppSettings">
        ///  ManagementAppSettings.Add("test", "121345678");
        ///  Console.WriteLine(ManagementAppSettings.GetVal("test"));  
        ///  ManagementAppSettings.Modify("test", "8654321");
        ///  Console.WriteLine(ManagementAppSettings.GetVal("test",false));// false : 不快取,一律重讀
        ///  ManagementAppSettings.Remove("test");
        /// </code>
        /// </example>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void Add(string key,string value)
        {
            Configuration config = ConfigHelper.AutoGetConfig();             
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings"); 
        }
        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="key">key</param>      
        public static void Remove(string key)
        {
            Configuration config = ConfigHelper.AutoGetConfig();
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings"); 
        } /// <summary>
        /// Modify
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void Modify(string key,string value)
        {
            Configuration config = ConfigHelper.AutoGetConfig();
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified,true);
            ConfigurationManager.RefreshSection("appSettings"); 
        }
        /// <summary>
        /// Web重新整理
        /// </summary>
        /// <returns>false:重新整理失敗</returns>
        public static bool WebRefresh() 
        {
            Configuration config = ConfigHelper.GetWebConfig();//有可能被其他執行續鎖住,導致無法RefreshSection
            if (config == null) return false;
            ConfigurationManager.RefreshSection("appSettings");
            return true;
        }
        /// <summary>
        /// App重新整理
        /// </summary>
        /// <returns>false:重新整理失敗</returns>
        public static bool AppRefresh()
        {
            Configuration config = ConfigHelper.GetAppConfig();//有可能被其他執行續鎖住,導致無法RefreshSection
            if (config == null) return false;
            ConfigurationManager.RefreshSection("appSettings");
            return true;
        }
    }
}
