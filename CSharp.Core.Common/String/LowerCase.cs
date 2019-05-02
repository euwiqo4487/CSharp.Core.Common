using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 用於小寫規則比較
    /// </summary>
    public class LowerCase : IEquatable<LowerCase>, IComparable<LowerCase>
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public LowerCase()
        {
        }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="text">內容字串</param>
        public LowerCase(string text)
        {
            this.Text = text;
        }
        /// <summary>
        /// 內容字串小寫
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="other">比較對象</param>
        /// <returns>true:相同   false:不同</returns>
        public bool Equals(LowerCase other)
        {
            if (other == null) return false;
            if (this.Text.ToLower() == other.Text.ToLower())
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
            if (this.Text.ToLower() == other.ToLower())
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
            LowerCase lowerCaseObj = obj as LowerCase;
            if (lowerCaseObj == null) return false;
            return this.Equals(lowerCaseObj);
        }
        /// <summary>
        /// HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Text.ToLower().GetHashCode();
        }
        ///// <summary>
        ///// ==
        ///// </summary>
        ///// <param name="lowerCase1"></param>
        ///// <param name="lowerCase2"></param>
        ///// <returns></returns>
        //public static bool operator == (LowerCase lowerCase1, LowerCase lowerCase2)
        //{
        //    if (lowerCase1 == null || lowerCase2 == null) return Object.Equals(lowerCase1, lowerCase2);
        //    return lowerCase1.Equals(lowerCase2);
        //}
        ///// <summary>
        ///// !=
        ///// </summary>
        ///// <param name="lowerCase1"></param>
        ///// <param name="lowerCase2"></param>
        ///// <returns></returns>
        //public static bool operator != (LowerCase lowerCase1, LowerCase lowerCase2)
        //{
        //    if (lowerCase1 == null || lowerCase2 == null) return !Object.Equals(lowerCase1, lowerCase2);
        //    return !lowerCase1.Equals(lowerCase2);
        //}
        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(LowerCase other)
        {
            if (other == null) return 1;
            return this.Text.ToLower().CompareTo(other.Text.ToLower());
        }
        ///// <summary>
        ///// 小於
        ///// </summary>
        ///// <param name="lowerCase1"></param>
        ///// <param name="lowerCase2"></param>
        ///// <returns></returns>
        //public static bool operator < (LowerCase lowerCase1, LowerCase lowerCase2)
        //{
        //    if (lowerCase1 == null || lowerCase2 == null) return false;
        //    return lowerCase1.CompareTo(lowerCase2) <0;
        //}
        ///// <summary>
        ///// 大於
        ///// </summary>
        ///// <param name="lowerCase1"></param>
        ///// <param name="lowerCase2"></param>
        ///// <returns></returns>
        //public static bool operator > (LowerCase lowerCase1, LowerCase lowerCase2)
        //{
        //    if (lowerCase1 == null || lowerCase2 == null) return false;
        //    return lowerCase1.CompareTo(lowerCase2) > 0;
        //}
        ///// <summary>
        ///// 小於等於
        ///// </summary>
        ///// <param name="lowerCase1"></param>
        ///// <param name="lowerCase2"></param>
        ///// <returns></returns>
        //public static bool operator <= (LowerCase lowerCase1, LowerCase lowerCase2)
        //{
        //    if (lowerCase1 == null || lowerCase2 == null) return false;
        //    return lowerCase1.CompareTo(lowerCase2) <= 0;
        //}
        ///// <summary>
        ///// 大於等於
        ///// </summary>
        ///// <param name="lowerCase1"></param>
        ///// <param name="lowerCase2"></param>
        ///// <returns></returns>
        //public static bool operator >= (LowerCase lowerCase1, LowerCase lowerCase2)
        //{
        //    if (lowerCase1 == null || lowerCase2 == null) return false;
        //    return lowerCase1.CompareTo(lowerCase2) >= 0;
        //}
    }
}
