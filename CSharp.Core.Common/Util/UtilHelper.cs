using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.ComponentModel;
using System.Collections;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 工具協助
    /// </summary>
    public static class UtilHelper
    {
        
        /// <summary>
        /// 合併,將兩個列舉項目合併,依據委讓方法
        /// </summary>
        /// <typeparam name="T1">列舉項目1</typeparam>
        /// <typeparam name="T2">列舉項目2</typeparam>
        /// <typeparam name="TOutput">新的輸出</typeparam>
        /// <param name="left">列舉項目</param>
        /// <param name="right">列舉項目</param>
        /// <param name="generator">委讓方法</param>
        /// <returns>新的輸出</returns>
        public static IEnumerable<TOutput> Merge<T1,T2,TOutput>(this IEnumerable<T1> left,IEnumerable<T2> right,Func<T1,T2,TOutput> generator)
        {
            IEnumerator<T1> leftSequence = left.GetEnumerator();
            IEnumerator<T2> rightSequence = right.GetEnumerator();
            while (leftSequence.MoveNext() && rightSequence.MoveNext())
            {
                yield return generator(leftSequence.Current, rightSequence.Current);
            }
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Max<T>(this T left, T right)
        {
            return Comparer<T>.Default.Compare(left, right) < 0 ? right : left;
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double Max(this double left, double right)
        {
            return Math.Max(left, right);
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Min<T>(this T left, T right)
        {
            return Comparer<T>.Default.Compare(left, right) < 0 ? left : right;
        }
        /// <summary>
        /// Min 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double Min(this double left, double right)
        {
            return Math.Min(left, right);
        }
        /// <summary>
        /// 取出不重複的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">來源清單</param>
        /// <returns>清單</returns>
        public static IEnumerable<T> Unique<T>(this IEnumerable<T> sequence)
        {
            Dictionary<T,T> uniqueVals = new Dictionary<T,T>();
            foreach(T item in sequence)
            {
                if (!uniqueVals.ContainsKey(item))
                {
                    uniqueVals.Add(item,item);
                    yield return item;
                }
            }
        }
        ///// <summary>
        ///// 過濾
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="sequence">來源清單</param>
        ///// <param name="filterFunc">委讓方法</param>
        ///// <returns>清單</returns>
        //public static IEnumerable<T> Filter<T>(this IEnumerable<T> sequence,Predicate<T> filterFunc)
        //{
        //    if (filterFunc == null) throw new ArgumentNullException("Predicate must not null");             
        //    foreach (T item in sequence)
        //    {
        //        if (filterFunc(item)) yield return item;                
        //    }
        //}
        /// <summary>
        /// 每 period 筆 採樣一次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">來源清單</param>
        /// <param name="period">每 n 筆</param>
        /// <returns>清單</returns>
        public static IEnumerable<T> EveryNthItem<T>(this IEnumerable<T> sequence, int period)
        {
            int count = 0;
            foreach (T item in sequence)
            {
                if (++count % period ==0) yield return item;
            }
        }
        /// <summary>
        /// 產生集合
        /// </summary>
        /// <typeparam name="TResult">傳回型別</typeparam>
        /// <param name="numnber">集合筆數</param>
        /// <param name="generator"></param>
        /// <returns>清單</returns>
        public static IEnumerable<TResult> Generator<TResult>(int numnber, Func<TResult> generator)
        {
            for (int i = 0; i < numnber; i++)
                yield return generator();
            
        }
        /// <summary>
        /// 轉換
        /// </summary>
        /// <typeparam name="Tin">型別</typeparam>
        /// <typeparam name="Tout">傳回型別</typeparam>
        /// <param name="sequence">來源清單</param>
        /// <param name="mapFunc">轉換function</param>
        /// <returns>清單</returns>
        public static IEnumerable<Tout> Map<Tin, Tout>(this IEnumerable<Tin> sequence, Func<Tin, Tout> mapFunc)
        {
            if (mapFunc == null) throw new ArgumentNullException("mapFunc must not null");  
            foreach (Tin item in sequence)
            {
                yield return mapFunc(item);
            }
        }
        /// <summary>
        /// ForEach 和 Map 相似
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="forEachFunc"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> sequence, Func<T, T> forEachFunc)
        {
            if (forEachFunc == null) throw new ArgumentNullException("forEachFunc must not null");
            foreach (T item in sequence)
            {
                yield return forEachFunc(item);
            }
        }
        /// <summary>
        /// 找尋到條件資料後返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="findSingleFunc"></param>
        /// <returns></returns>
        public static T FindSingle<T>(this IEnumerable<T> sequence, Func<T, bool> findSingleFunc)
        {
            if (findSingleFunc == null) throw new ArgumentNullException("findSingleFunc must not null");
            foreach (T item in sequence)
            {
                if (findSingleFunc(item)) return item;
            }
            return default(T);
        }
        /// <summary>
        /// 轉換
        /// </summary>
        /// <typeparam name="Tin">型別</typeparam>
        /// <typeparam name="Tout">傳回型別</typeparam>
        /// <param name="element">來源</param>
        /// <param name="mapFunc">轉換function</param>
        /// <returns>傳回型別</returns>
        public static Tout Map<Tin, Tout>(this Tin element, Func<Tin, Tout> mapFunc)
        {
            if (mapFunc == null) throw new ArgumentNullException("mapFunc must not null");
            return mapFunc(element);           
        }       
        /// <summary>
        /// 累加
        /// </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <typeparam name="TResult">傳回型別</typeparam>
        /// <param name="sequence">來源清單</param>
        /// <param name="total">總和</param>
        /// <param name="accumnlator">總和function</param>
        /// <returns>總和</returns>
        public static TResult Sum<T, TResult>(this IEnumerable<T> sequence, TResult total, Func<T, TResult, TResult> accumnlator)
        {
            if (accumnlator == null) throw new ArgumentNullException("accumnlator must not null");
            foreach (T item in sequence)
            {
                total = accumnlator(item,total);
            }
            return total;
        }
        /// <summary>
        /// 串連集合的成員，並在每個成員之間使用指定的分隔符號
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="separator">分隔符號</param>
        /// <returns></returns>
        public static string JoinCollection<T>(this IEnumerable<T> values, string separator = ",")
        {
            return string.Join<T>(separator, values);
        }
        /// <summary>
        /// 取得由字串轉換後的正確型別值
        /// </summary>
        /// <typeparam name="T">轉換型別</typeparam>
        /// <param name="input">字串值</param>
        /// <param name="defVal">預設值</param>
        /// <returns>型別值</returns>
        public static T DefaultParse<T>(this string input, string defVal = "") 
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            string val = string.IsNullOrEmpty(input) ? defVal : input;
            if(converter != null){
                if (converter.IsValid(val))
                {
                    return (T)converter.ConvertFromString(val);
                }
                else if (defVal != "" && converter.IsValid(defVal))
                {
                    return (T)converter.ConvertFromString(defVal);
                }    
            }
            return default(T);
        }
        /// <summary>
        /// 取得由object轉換後的正確型別值
        /// </summary>
        /// <typeparam name="T">轉換型別</typeparam>
        /// <param name="obj">object</param>
        /// <param name="defVal">預設值</param>
        /// <returns>型別值</returns>
        public static T DefaultParse<T>(this object obj, string defVal = "")
        {
            if (typeof(DBNull) != obj.GetType())
            {
                return (T)Convert.ChangeType(obj, typeof(T)); ;
            }
            return defVal.DefaultParse<T>();
        }
        /// <summary>
        /// 給DB Param 使用 ,簡化 null 時 判斷 語句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DefaultParse(this object obj)
        {             
            return obj??DBNull.Value;
        }
        /// <summary>
        /// 針對 IDictionary 的 類別 ,免去判斷是否包含key ,如果包含則取代
        /// </summary>
        /// <param name="dictionary">IDictionary</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>         
        public static void AddOrSet(this IDictionary dictionary, object key, object value)
        {
            if (dictionary.Contains(key))
            {                
                dictionary[key] = value;                
                return;
            }
            dictionary.Add(key, value);
        }
        /// <summary>
        /// 針對 IDictionary 的 類別 ,免去判斷是否包含key ,如果包含則離開
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOnly(this IDictionary dictionary, object key, object value)
        {
            if (dictionary.Contains(key))
            {                 
                return;
            }
            dictionary.Add(key, value);
        }
        /// <summary>
        /// 針對 IDictionary 的 類別 ,免去判斷是否包含key ,如果包含則取代
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetOnly(this IDictionary dictionary, object key, object value)
        {
            if (dictionary.Contains(key))
            {
                dictionary[key] = value;                
            }             
        }
        /// <summary>
        /// 偵測byte[]是否為BIG5編碼
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>true: big5</returns>
        public static bool IsBig5Encoding(byte[] bytes)
        {
            Encoding big5 = Encoding.GetEncoding(950);
            //將byte[]轉為string再轉回byte[]看位元數是否有變
            return bytes.Length == big5.GetByteCount(big5.GetString(bytes));
        }
        /// <summary>
        /// 偵測檔案否為BIG5編碼
        /// </summary>
        /// <param name="file">完整檔名</param>
        /// <returns>true: big5</returns>
        public static bool IsBig5Encoding(string file)
        {
            if (!File.Exists(file)) return false;
            return IsBig5Encoding(File.ReadAllBytes(file));
        }
        /// <summary>
        /// 將雙引號可顯示到Console的args
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToConsoleArgs(this string input)
        {
            return input.Replace("\"", "\"\"\"");
        }
    }
}
