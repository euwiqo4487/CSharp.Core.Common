using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common.Util
{
    /// <summary>
    /// 比較 擴充方法
    /// </summary>
    public static class Comparable
    {
        /// <summary>
        /// 小於 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LessThen<T>(this T left, T right)where T:IComparable<T>
        {
            return left.CompareTo(right) < 0;
        }
        /// <summary>
        /// 大於
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GreaterThen<T>(this T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) > 0;
        }
        /// <summary>
        /// 小於等於
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LessThenEqual<T>(this T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) <= 0;
        }
        /// <summary>
        /// 大於等於
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GreaterThenEqual<T>(this T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
