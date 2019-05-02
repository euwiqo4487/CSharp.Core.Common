using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharp.Core.Common
{
    /// <summary>
    /// KeyValueStore
    /// </summary>
    public static class KeyValueStore
    {
        const string PRE = "{", POST = "}";
        static string[] _Keys = new string[] { "year", "yearmonth", "month", "day", "date", "datetime", "time" };

        /// <summary>
        /// 取得內部的字典值
        /// </summary>
        /// <param name="key">引索</param>
        /// <param name="prefix">前置詞</param>
        /// <param name="postfix">後置詞</param>
        /// <returns></returns>
        public static string GetValue(string key, string prefix = "", string postfix = "")
        {
            return InternalValue(key, prefix, postfix);
        }

        /// <summary>
        /// 整串字典取代
        /// </summary>
        /// <param name="str">欲取代字串</param>
        /// <param name="prefix">前置詞</param>
        /// <param name="postfix">後置詞</param>
        /// <returns></returns>
        public static string Replace(string str, string prefix = "", string postfix = "")
        {
            if (str.Trim() != "")
            {
                foreach (string key in _Keys)
                {
                    string tick = FullKey(key, prefix, postfix),
                        val = InternalValue(key, prefix, postfix);
                    if (val.Trim() != "") //matching keyword
                        str = str.Replace(tick, val);
                }
            }
            return str;
        }

        /// <summary>
        /// 整串字典取代 (加自訂取代)
        /// </summary>
        /// <param name="str">欲取代字串</param>
        /// <param name="ht">自訂字典</param>
        /// <param name="prefix">前置詞</param>
        /// <param name="postfix">後置詞</param>
        /// <returns></returns>
        public static string Replace(string str, Hashtable ht, string prefix = "", string postfix = "")
        {
            if (str.Trim() != "")
            {
                if (ht != null && ht.Count > 0)
                {
                    //user-defined key-values first
                    foreach (string key in ht.Keys)
                    {
                        string tick = FullKey(key, prefix, postfix);
                        if (ht[key] != null) // value can be empty, let user decide
                            str = str.Replace(tick, ht[key].ToString());
                    }
                }
                //default key-values
                str = Replace(str, prefix, postfix);
            }
            return str;
        }

        static string FullKey(string key, string prefix, string postfix)
        {
            return string.Format("{0}{1}{2}", prefix.Trim() == "" ? PRE : prefix, key, postfix.Trim() == "" ? POST : postfix);
        }

        static string InternalValue(string key, string prefix, string postfix)
        {
            string value = "";
            if (prefix.Trim() == "")
                prefix = PRE;
            if (postfix.Trim() == "")
                postfix = POST;
            if (key.Trim() != "")
            {
                switch (key.ToLower().Replace(prefix, "").Replace(postfix, ""))
                {
                    case "year":
                        value = DateTime.Now.Year.ToString();
                        break;
                    case "month":
                        value = DateTime.Now.Month.ToString();
                        break;
                    case "yearmonth":
                        value = DateTime.Now.ToString("yyyyMM");
                        break;
                    case "day":
                        value = DateTime.Now.Day.ToString();
                        break;
                    case "date":
                        value = DateTime.Now.ToString("yyyyMMdd");
                        break;
                    case "datetime":
                        value = DateTime.Now.ToString("yyyyMMddHHmmss");
                        break;
                    case "time":
                        value = DateTime.Now.ToString("HHmmss");
                        break;
                }
            }
            return value;
        }
    }
}
