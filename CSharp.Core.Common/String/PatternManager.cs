using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CSharp.Core.Common.Resource;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 密碼字串模式檢查
    /// </summary>
    public class PatternManager
    {
        /// <summary>
        /// 密碼檢查所有規則
        /// </summary>
        /// <param name="input">使用者輸入</param>
        /// <param name="id">使用者帳號</param>
        /// <param name="parameters">資料庫設定資料</param>
        /// <returns>錯誤訊息(空白為成功)</returns>
        public static string PwdCheckAll(string input, string id = "", Dictionary<string, string> parameters = null)
        {
            var err = string.Empty;

            if (parameters != null)
            {
                err = Length(input, parameters); //長度限制
                if(string.IsNullOrEmpty(err))
                    err = Content(input, parameters); //密碼組合
                if (string.IsNullOrEmpty(err))
                    err = Special(input, id, parameters); //特殊限制
            }
            return err;
        }

        /// <summary>
        /// 檢查字串長度範圍
        /// </summary>
        /// <param name="input">使用者輸入</param>
        /// <param name="parameters">資料庫設定資料</param>
        /// <returns>錯誤訊息(空白為成功)</returns>
        public static string Length(string input, Dictionary<string, string> parameters = null)
        {
            string err = string.Empty;

            if (parameters != null)
            {
                string min = "", max = "", space = "";
                bool isSpace = false;

                //判斷資料設定
                if (parameters.ContainsKey("PasswordMinLength"))
                {
                    min = parameters["PasswordMinLength"];
                }
                if (parameters.ContainsKey("PasswordMaxLength"))
                {
                    max = parameters["PasswordMaxLength"];
                }
                if (parameters.ContainsKey("CanWhiteSpace"))
                {
                    isSpace = parameters["CanWhiteSpace"].Equals("1") ? true : false;
                    space = isSpace ? "\\s\\S" : "\\S";
                }
                //IsWhiteSpace is true then include white spaces
                string pat = "^[" + space + "]{" + min + "," + max + "}$";
                var match = Regex.IsMatch(input, pat);
                if (!match)
                {
                    err = string.Format(Resources.PatternManager_Length, min, max);
                    err += (isSpace) ? "" : Resources.PatternManager_WhiteSpaceNotAllowed;
                }
            }
            return err;
        }

        /// <summary>
        /// 密碼組合
        /// </summary>
        /// <param name="input">使用者輸入</param>
        /// <param name="parameters">資料庫設定資料</param>
        /// <returns>錯誤訊息(空白為成功)</returns>
        public static string Content(string input, Dictionary<string, string> parameters = null)
        {
            var err = string.Empty;

            if (parameters != null)
            {
                var pat = new StringBuilder();
                var tmp = input;

                bool isNumeric = true, isAlphabet = true, isAlphabetSensitive = true, isSymbol = true, canWhiteSpace = true;

                //判斷資料設定
                if (parameters.ContainsKey("CanNumeric"))
                {
                    isNumeric = parameters["CanNumeric"].Equals("1") ? true : false;
                }
                if (parameters.ContainsKey("CanAlphabet"))
                {
                    isAlphabet = parameters["CanAlphabet"].Equals("1") ? true : false;
                }
                if (parameters.ContainsKey("CanAlphabetSensitive"))
                {
                    isAlphabetSensitive = parameters["CanAlphabetSensitive"].Equals("1") ? true : false;
                }
                if (parameters.ContainsKey("CanSymbol"))
                {
                    isSymbol = parameters["CanSymbol"].Equals("1") ? true : false;
                }
                if (parameters.ContainsKey("CanWhiteSpace"))
                {
                    canWhiteSpace = parameters["CanWhiteSpace"].Equals("1") ? true : false;
                }

                if (isNumeric)
                    pat.Append("0-9");

                if (isAlphabet)
                {
                    tmp = tmp.ToLower();
                    pat.Append("a-z");
                }
                else if (isAlphabetSensitive)
                    pat.Append("a-zA-Z");

                if (isSymbol)
                    pat.Append(@"\W_");

                if (canWhiteSpace)
                    pat.Append(@" ");

                var result = string.Format(@"^[{0}]+$", pat.ToString());
                var match = Regex.IsMatch(tmp, result);

                if (!match)
                {
                    err = string.Format(Resources.PatternManager_ContentFormatFailed + " {0}{1}{2}{3}{4}",
                            isNumeric ? Resources.PatternManager_IsNumeric : "",
                            isAlphabet ? Resources.PatternManager_IsAlphabet : "",
                            isAlphabetSensitive ? Resources.PatternManager_IsAlphabetSensitive : "",
                            isSymbol ? Resources.PatternManager_IsSymbol : "",
                            canWhiteSpace ? Resources.PatternManager_WhiteSpace : "");
                }
            }
            return err;
        }

        /// <summary>
        /// 特殊限制
        /// </summary>
        /// <param name="input">使用者輸入</param>
        /// <param name="id">ID</param>
        /// <param name="parameters">資料庫設定資料</param>
        /// <returns>錯誤訊息(空白為成功)</returns>
        public static string Special(string input, string id = "", Dictionary<string, string> parameters = null)
        {
            var err = string.Empty;

            if (parameters != null)
            {
                bool canMatchID = true;
                if (parameters.ContainsKey("CanMatchID"))
                    canMatchID = parameters["CanMatchID"].Equals("1") ? true : false;

                if (!canMatchID)
                {
                    if (string.IsNullOrEmpty(id))
                        err = Resources.PatternManager_IDEmpty;
                    else if (input.Contains(id))
                        err = Resources.PatternManager_CantContainID;
                }

                if (parameters.ContainsKey("UniqueCharsMin") && string.IsNullOrEmpty(err))
                {
                    var uniqueChars = int.Parse(parameters["UniqueCharsMin"]);

                    if (uniqueChars > 0)
                    {
                        var tmpCount = input.Distinct().Count();

                        if (tmpCount < uniqueChars)
                            err = string.Format(Resources.PatternManager_MinUniqueChars, uniqueChars);
                    }
                }

                if (parameters.ContainsKey("CharRepeatLimit") && string.IsNullOrEmpty(err))
                {
                    var charRepeatLimit = int.Parse(parameters["CharRepeatLimit"]);

                    if (charRepeatLimit > 0)
                    {
                        var isConsecutiveRepeat = Regex.IsMatch(input, @"(.)\1{" + (charRepeatLimit - 1).ToString() + ",}");
                        if (isConsecutiveRepeat)
                            err = string.Format(Resources.PatternManager_CharsRepeatLimit, charRepeatLimit);
                    }
                }

                if (parameters.ContainsKey("CharSequentialLimit") && string.IsNullOrEmpty(err))
                {
                    var charSeqLimit = int.Parse(parameters["CharSequentialLimit"]);

                    if (charSeqLimit > 0)
                    {
                        err = Sequential(input, charSeqLimit);    
                    }
                }

                if (parameters.ContainsKey("KeyboardOrderLimit") && string.IsNullOrEmpty(err))
                {
                    var keyboardOrderLimit = int.Parse(parameters["KeyboardOrderLimit"]);

                    if (keyboardOrderLimit > 0)
                    {
                        err = KeyOrderCheck(input, keyboardOrderLimit);                    
                    }
                }
            }
            return err;
        }

        private static string KeyOrderCheck(string text, int count)
        {
            var pat = @"1qaz2wsx3edc4rfv5tgb6yhn7ujm8ik,9ol.0p;/-['=]\\]'/=[;.-pl,0okm9ijn8uhb7ygv6tfc5rdx4esz3wa2q1`1q2w3e4r5t6y7u8i9o0p-[=]\qawsedrftgyhujikolp;[']\azsxdcfvgbhnjmk,l.;/'~!@#$%^&*()_+`1234567890-=qwertyuiop[]\asdfghjkl;'zxcvbnm,./";
            var err = string.Empty;
            var result = SpecialPatternCheck(pat, text, count);
            var detected = !string.IsNullOrEmpty(result);
            if (detected)
                err = string.Format(Resources.PatternManager_CharsSeqLimit, result);
            return err;
        }

        private static string Sequential(string text, int seqCount)
        {
            string pat, result;
            bool detected = false;
            var err = string.Empty;
            pat = "01234567890";
            result = SpecialPatternCheck(pat, text, seqCount);
            detected = !string.IsNullOrEmpty(result);
            if (detected)
                err = string.Format(Resources.PatternManager_KeyboardOrderLimit, result);

            pat = "abcdefghijklmnopqrstuvwxyz";
            result = SpecialPatternCheck(pat, text, seqCount);
            detected = !string.IsNullOrEmpty(result);
            if (detected)
                err = string.Format(Resources.PatternManager_KeyboardOrderLimit, result);
            return err;
        }

        private static string SpecialPatternCheck(string pattern, string input, int occurCount)
        {
            string tmp = "";
            // upper case ascending
            tmp = SpecialMatch(pattern.ToUpper(), input, occurCount);
            if (!string.IsNullOrEmpty(tmp))
                return tmp;
            // upper case descending
            tmp = SpecialMatch(Reverse(pattern.ToUpper()), input, occurCount);
            if (!string.IsNullOrEmpty(tmp))
                return tmp;
            // lower case ascending
            tmp = SpecialMatch(pattern.ToLower(), input, occurCount);
            if (!string.IsNullOrEmpty(tmp))
                return tmp;
            // lower case descending
            tmp = SpecialMatch(Reverse(pattern.ToLower()), input, occurCount);
            if (!string.IsNullOrEmpty(tmp))
                return tmp;

            return string.Empty;
        }

        private static string SpecialMatch(string pattern, string input, int count)
        {
            var tmpItems = LookupItems(input, count);

            if (tmpItems.Count() > 0)
            {
                foreach (string source in tmpItems)
                {
                    if (pattern.IndexOf(source) != -1)
                        return source;
                }
            }

            return string.Empty;
        }

        private static IEnumerable<string> LookupItems(string input, int len)
        {
            if (!string.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    var chopIndex = i + len;
                    if (chopIndex < input.Length)
                        yield return input.Substring(i, len);
                }
            }
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
