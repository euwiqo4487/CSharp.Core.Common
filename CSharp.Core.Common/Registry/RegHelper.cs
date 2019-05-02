using System;
using Microsoft.Win32;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 機碼協助靜態類別 
    /// </summary>
    public static class RegHelper
    {
        static RegistryKey baseRegistryKey;

        /// <summary>
        /// Base registry 
        /// Default: HKEY_CURRENT_USER\Software\CSharp
        /// </summary>
        public static RegistryKey BaseRegistryKey
        {
            get
            {
                if (baseRegistryKey == null)
                    baseRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\CSharp");
                return baseRegistryKey;
            }
            set
            {
                baseRegistryKey = value;
            }
        }

        static string subKey = "";

        /// <summary>
        /// sub registry path under base registry
        /// </summary>
        public static string SubKey
        {
            get
            {
                return subKey;
            }
            set
            {
                subKey = value;
            }
        }

        /// <summary>
        /// Read registry key from sub registry (if sub not provided then base registry)
        /// </summary>
        /// <param name="KeyName">Key Name</param>
        /// <returns>值</returns>
        public static string Read(string KeyName)
        {
            // Opening the registry key
            RegistryKey rk = BaseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return (string)sk1.GetValue(KeyName.ToUpper());
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ThrowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
                    return null;
                }
            }
        }

        /// <summary>
        /// write key/value under sub registry
        /// </summary>
        /// <param name="KeyName">Key Name</param>
        /// <param name="Value">值</param>
        /// <returns>true:寫入成功</returns>
        public static bool Write(string KeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ThrowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
                return false;
            }
        }

        /// <summary>
        /// delete key/value pair from sub registry
        /// </summary>
        /// <param name="KeyName">Key Name</param>
        /// <returns>true:刪除成功</returns>
        public static bool DeleteKey(string KeyName)
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                else
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ThrowErrorMessage(e, "Deleting SubKey " + subKey);
                return false;
            }
        }

        /// <summary>
        /// delete the entire sub registry
        /// </summary>
        /// <returns>true:刪除成功</returns>
        public static bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ThrowErrorMessage(e, "Deleting SubKey " + subKey);
                return false;
            }
        }

        /// <summary>
        /// count the subkey from sub registry
        /// note:not the key/value count (use ValueCount() for this purpose)
        /// </summary>
        /// <returns>count the subkey</returns>
        public static int SubKeyCount()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.SubKeyCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ThrowErrorMessage(e, "Retriving subkeys of " + subKey);
                return 0;
            }
        }

        /// <summary>
        /// count the key/value pairs from sub registry
        /// </summary>
        /// <returns>count the key/value pairs</returns>
        public static int ValueCount()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.ValueCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ThrowErrorMessage(e, "Retriving keys of " + subKey);
                return 0;
            }
        }

        /// <summary>
        /// throw the exception with given title
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="Title">名稱</param>
        private static void ThrowErrorMessage(Exception e, string Title)
        {
            throw new Exception(string.Format("{0}:{1}", Title, e.Message));
        }
    }
}
