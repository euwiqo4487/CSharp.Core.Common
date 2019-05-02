using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharp.Core.Common
{
    /// <summary>
    /// String Helper
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 將字串第一個位置 轉成 ASCII
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64 ToASCII(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            return Convert.ToInt64(str[0]);
        }
        /// <summary>
        /// 將空字串取代
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replaceStr">取代字串</param>
        /// <returns></returns>
        public static string ReplaceEmpty(this string str, string replaceStr = "0")
        {
            if (string.IsNullOrEmpty(str))
                return replaceStr;
            return str;
        }
        /// <summary>
        /// 從位置0依長度截取字串
        /// 有處理雙位元字元截斷問題
        /// </summary>
        /// <param name="input">原始字串</param>
        /// <param name="len">截取長度</param>
        /// <returns></returns>
        public static string DoubleByteSubstr(this string input, int len)
        {
            var encoding = Encoding.Default;
            byte[] b = encoding.GetBytes(input);
            if (b.Length <= len) //未超長，直接傳回
                return input;
            else
            {
                string res = encoding.GetString(b, 0, len);
                //由於可能最後一個字元可能切到中文字的前一碼形成亂碼
                //透過截斷的亂碼與完整轉換結果會有出入的原理來偵測
                if (!encoding.GetString(b).StartsWith(res))
                    res = encoding.GetString(b, 0, len - 1);
                return res;
            }
        }
        /// <summary>
        /// 同string.Replace()
        /// 但無視大小寫
        /// </summary>
        /// <param name="input">輸入字串</param>
        /// <param name="search">尋找比對用字串</param>
        /// <param name="replacement">替代字串</param>
        /// <returns></returns>
        public static string ReplaceIgnoreCase(this string input, string search, string replacement)
        {
            string result = Regex.Replace(
                input,
                Regex.Escape(search),
                replacement.Replace("$", "$$"),
                RegexOptions.IgnoreCase
            );
            return result;
        }
        /// <summary>
        /// 取得位元長度
        /// 用於中(或全形)英混合字串
        /// </summary>
        /// <param name="input">原始字串</param>
        /// <returns></returns>
        public static int ByteLength(this string input)
        {
            return Encoding.Default.GetByteCount(input);
        }
        /// <summary>
        /// 編碼轉換 Unicode to Big5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UTF8toBIG5(this string input)
        {
            return Encoding.GetEncoding(950).GetString(Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(950), Encoding.Unicode.GetBytes(input)));

        }
        /// <summary>
        /// 字串強制轉全形
        /// len若非0,則可能會截斷
        /// isFill=true,則會全形空白向右填滿
        /// </summary>
        /// <param name="input">原始字串</param>
        /// <param name="bLen">byte長度</param>
        /// <param name="isFill">是否依據bLen長度填滿,預設是false</param>
        /// <returns></returns>
        public static string StrToFullWidth(this string input, int bLen = 0, bool isFill = false)
        {
            if (bLen > 0 && isFill && bLen > input.ByteLength())
                input = input.PadRight(bLen);

            var sb = new StringBuilder();
            foreach (char c in input)
            {
                if (Convert.ToInt32(c) < 127)
                    sb.Append(CharWidthConv(c.ToString()));
                else
                    sb.Append(c.ToString());
            }

            string tmp = sb.ToString();
            if (bLen > 0)
                tmp = tmp.DoubleByteSubstr(bLen);
            return tmp;
        }

        static char[] MixWhiteList = new char[] {
            'a','b','c', 'd','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9', ' '
        };

        /// <summary>
        /// 字串強制轉全形 (排除英數)
        /// len若非0,則可能會截斷
        /// isFill=true,則會半形空白向右填滿
        /// </summary>
        /// <param name="input">原始字串</param>
        /// <param name="bLen">byte長度</param>
        /// <param name="isFill">是否依據bLen長度填滿,預設是false</param>
        /// <returns></returns>
        public static string StrToFullWidthWL(this string input, int bLen = 0, bool isFill = false)
        {
            var sb = new StringBuilder();
            int alen = 0;
            foreach (char c in input)
            {
                string tmp = string.Empty;
                if (Array.Exists(MixWhiteList, el => el == c || el.ToString().Trim().ToUpper() == c.ToString().Trim().ToUpper()))
                    tmp = c.ToString();
                else
                {
                    if (Convert.ToInt32(c) < 127)
                        tmp = CharWidthConv(c.ToString());
                    else
                        tmp = c.ToString();
                }

                if (bLen > 0 && isFill && bLen < alen + tmp.ByteLength())
                    break;                    

                alen += tmp.ByteLength();
                sb.Append(tmp);
            }
            if (bLen > 0 && isFill && bLen-alen>0)
            {
                for (int i = 0; i < bLen - alen; i++)
                    sb.Append(" ");
            }
               
            return sb.ToString();
        }

        private static string CharWidthConv(string unicodeString, bool isFullWidth = true)
        {
            var sb = new StringBuilder(256);
            LCMapString(LOCALE_SYSTEM_DEFAULT, isFullWidth ? LCMAP_FULLWIDTH : LCMAP_HALFWIDTH, unicodeString, -1, sb, sb.Capacity);
            return sb.ToString();
        }
        private const uint LCMAP_FULLWIDTH = 0x00800000;
        private const uint LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const uint LCMAP_HALFWIDTH = 0x00400000;
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int LCMapString(uint Locale, uint dwMapFlags, string lpSrcStr, int cchSrc, StringBuilder lpDestStr, int cchDest);

        /// <summary>
        /// 字串"1"轉換成Bool true, 其他皆為false
        /// </summary>
        /// <param name="input">原始字串</param>
        /// <returns></returns>
        public static bool StrToBool(this string input)
        {
            return input.Trim() == "1";
        }
    }
}
