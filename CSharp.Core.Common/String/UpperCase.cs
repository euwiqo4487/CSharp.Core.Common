using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 用於大寫規則比較
    /// </summary>
    public class UpperCase : IEquatable<UpperCase>, IComparable<UpperCase>
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public UpperCase()
        {            
        }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="text">內容字串</param>
        public UpperCase(string text)
        {
            this.Text = text;
        }
        /// <summary>
        /// 內容字串大寫
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="other">比較對象</param>
        /// <returns>true:相同   false:不同</returns>
        public bool Equals(UpperCase other)
        {
            if (other == null) return false;
            if (this.Text.ToUpper() == other.Text.ToUpper())
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 比較字串
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(string other)
        {
            if (this.Text.ToUpper() == other.ToUpper())
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 比較Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null) return false;
            UpperCase upperCaseObj = obj as UpperCase;
            if (upperCaseObj == null) return false;
            return this.Equals(upperCaseObj);
        }
        /// <summary>
        /// HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Text.ToUpper().GetHashCode();
        }
        ///// <summary>
        ///// ==
        ///// </summary>
        ///// <param name="upperCase1"></param>
        ///// <param name="upperCase2"></param>
        ///// <returns></returns>
        //public static bool operator == (UpperCase upperCase1, UpperCase upperCase2)
        //{
        //    if (upperCase1 == null || upperCase2 == null) return Object.Equals(upperCase1, upperCase2);
        //    return upperCase1.Equals(upperCase2);
        //}
        ///// <summary>
        ///// !=
        ///// </summary>
        ///// <param name="upperCase1"></param>
        ///// <param name="upperCase2"></param>
        ///// <returns></returns>
        //public static bool operator != (UpperCase upperCase1, UpperCase upperCase2)
        //{
        //    if (upperCase1 == null || upperCase2 == null) return !Object.Equals(upperCase1, upperCase2);
        //    return !upperCase1.Equals(upperCase2);
        //}
        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(UpperCase other)
        {
            if (other == null) return 1;
            return this.Text.ToLower().CompareTo(other.Text.ToLower());
        }
        ///// <summary>
        ///// 小於
        ///// </summary>
        ///// <param name="upperCase1"></param>
        ///// <param name="upperCase2"></param>
        ///// <returns></returns>
        //public static bool operator < (UpperCase upperCase1, UpperCase upperCase2)
        //{
        //    if (upperCase1 == null || upperCase2 == null) return false;
        //    return upperCase1.CompareTo(upperCase2) < 0;
        //}
        ///// <summary>
        ///// 大於
        ///// </summary>
        ///// <param name="upperCase1"></param>
        ///// <param name="upperCase2"></param>
        ///// <returns></returns>
        //public static bool operator > (UpperCase upperCase1, UpperCase upperCase2)
        //{
        //    if (upperCase1 == null || upperCase2 == null) return false;
        //    return upperCase1.CompareTo(upperCase2) > 0;
        //}
        ///// <summary>
        ///// 小於等於
        ///// </summary>
        ///// <param name="upperCase1"></param>
        ///// <param name="upperCase2"></param>
        ///// <returns></returns>
        //public static bool operator <= (UpperCase upperCase1, UpperCase upperCase2)
        //{
        //    if (upperCase1 == null || upperCase2 == null) return false;
        //    return upperCase1.CompareTo(upperCase2) <= 0;
        //}
        ///// <summary>
        ///// 大於等於
        ///// </summary>
        ///// <param name="upperCase1"></param>
        ///// <param name="upperCase2"></param>
        ///// <returns></returns>
        //public static bool operator >= (UpperCase upperCase1, UpperCase upperCase2)
        //{
        //    if (upperCase1 == null || upperCase2 == null) return false;
        //    return upperCase1.CompareTo(upperCase2) >= 0;
        //}
    }
}
