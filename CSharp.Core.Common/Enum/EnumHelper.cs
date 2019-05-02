using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Enum Helper
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 字串轉換 成 enum 
        /// </summary>
        /// <typeparam name="TEnum">enum </typeparam>
        /// <param name="value">包含要轉換的 名稱 或 數值</param>
        /// <param name="ignoreCase">true:忽略大小寫</param>
        /// <returns>列舉</returns>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum) throw new ArgumentException("T must be an enumerated type");
            return (TEnum)Convert.ChangeType(Enum.Parse(enumType, value, ignoreCase), enumType);
        }
        /// <summary>
        ///  ascii 的數值 比對出 列舉
        /// </summary>
        /// <typeparam name="TEnum">enum</typeparam>
        /// <param name="value">ascii數值</param>
        /// <param name="ignoreCase">true:忽略大小寫</param>
        /// <returns>列舉</returns>
        public static TEnum Parse<TEnum>(Int64 value, bool ignoreCase = false) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return Parse<TEnum>(value.ToString(), ignoreCase);
        }
        ///// <summary>
        ///// ascii 的數值 比對出 列舉
        ///// </summary>
        ///// <typeparam name="TEnum">enum</typeparam>
        ///// <param name="value">ascii</param>
        ///// <param name="ignoreCase">true:忽略大小寫</param>
        ///// <returns>列舉</returns>
        //public static TEnum ParseASC<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, IConvertible, IComparable, IFormattable
        //{
        //    return Parse<TEnum>(Convert.ToInt32(value[0]).ToString(), ignoreCase);             
        //}
        ///// <summary>
        ///// ascii 的數值 比對出 列舉
        ///// </summary>
        ///// <typeparam name="TEnum">enum</typeparam>
        ///// <param name="value">ascii</param>
        ///// <param name="ignoreCase">true:忽略大小寫</param>
        ///// <returns>列舉</returns>
        //public static TEnum ParseASC<TEnum>(char value, bool ignoreCase = false) where TEnum : struct, IConvertible, IComparable, IFormattable
        //{
        //    return Parse<TEnum>(Convert.ToInt32(value).ToString(), ignoreCase);       
        //}
        /// <summary>
        /// 字串轉換 成 enum 並傳回指定基本型別
        /// </summary>
        /// <typeparam name="TEnum">enum</typeparam>
        /// <typeparam name="T">指定基本型別</typeparam>
        /// <param name="value">包含要轉換的 名稱 或 數值</param>
        /// <param name="ignoreCase">true:忽略大小寫</param>
        /// <returns></returns>
        public static T Parse<TEnum,T>(string value, bool ignoreCase = false) 
            where TEnum : struct, IConvertible, IComparable, IFormattable
            where T : IConvertible

        {

            return (T)Convert.ChangeType(Parse<TEnum>(value, ignoreCase), typeof(T)); 
        }
        /// <summary>
        /// ascii 的數值 比對出 列舉 傳回指定的泛型
        /// </summary>
        /// <typeparam name="TEnum">列舉 泛型</typeparam>
        /// <typeparam name="T">傳回指定的泛型</typeparam>
        /// <param name="value"> ascii 的數值</param>
        /// <param name="ignoreCase">true:忽略大小寫</param>
        /// <returns>傳回指定的泛型</returns>
        public static T Parse<TEnum, T>(Int64 value, bool ignoreCase = false)
            where TEnum : struct, IConvertible, IComparable, IFormattable
            where T : IConvertible
        {

            return Parse<TEnum, T>(value.ToString(), ignoreCase);
        }
        ///// <summary>
        ///// 字串轉換 成 enum 並傳回指定基本型別
        ///// </summary>
        ///// <typeparam name="TEnum"></typeparam>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <param name="ignoreCase"></param>
        ///// <returns></returns>
        //public static T ParseASC<TEnum, T>(string value, bool ignoreCase = false)
        //    where TEnum : struct, IConvertible, IComparable, IFormattable
        //    where T : IConvertible
        //{

        //    return Parse<TEnum,T>(Convert.ToInt32(value[0]).ToString(), ignoreCase);
        //}
        ///// <summary>
        ///// 字串轉換 成 enum 並傳回指定基本型別
        ///// </summary>
        ///// <typeparam name="TEnum"></typeparam>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <param name="ignoreCase"></param>
        ///// <returns></returns>
        //public static T ParseASC<TEnum, T>(char value, bool ignoreCase = false)
        //    where TEnum : struct, IConvertible, IComparable, IFormattable
        //    where T : IConvertible
        //{

        //    return Parse<TEnum, T>(Convert.ToInt32(value).ToString(), ignoreCase);
        //}

    }
}
