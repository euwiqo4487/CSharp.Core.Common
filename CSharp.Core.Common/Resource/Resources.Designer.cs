﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSharp.Core.Common.Resource {
    using System;
    
    
    /// <summary>
    ///   用於查詢當地語系化字串等的強類型資源類別。
    /// </summary>
    // 這個類別是自動產生的，是利用 StronglyTypedResourceBuilder
    // 類別透過 ResGen 或 Visual Studio 這類工具。
    // 若要加入或移除成員，請編輯您的 .ResX 檔，然後重新執行 ResGen
    // (利用 /str 選項)，或重建您的 VS 專案。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   傳回這個類別使用的快取的 ResourceManager 執行個體。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CSharp.Core.Common.Resource.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   覆寫目前執行緒的 CurrentUICulture 屬性，對象是所有
        ///   使用這個強類型資源類別的資源查閱。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查詢類似 不可包含ID 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_CantContainID {
            get {
                return ResourceManager.GetString("PatternManager_CantContainID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 同樣字元不能連續{0}次以上 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_CharsRepeatLimit {
            get {
                return ResourceManager.GetString("PatternManager_CharsRepeatLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 鍵盤順序連續「{0}」 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_CharsSeqLimit {
            get {
                return ResourceManager.GetString("PatternManager_CharsSeqLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 內容不符合規則! 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_ContentFormatFailed {
            get {
                return ResourceManager.GetString("PatternManager_ContentFormatFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 ID不可為空白 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_IDEmpty {
            get {
                return ResourceManager.GetString("PatternManager_IDEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 英文字  的當地語系化字串。
        /// </summary>
        internal static string PatternManager_IsAlphabet {
            get {
                return ResourceManager.GetString("PatternManager_IsAlphabet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 區分大小寫的英文字  的當地語系化字串。
        /// </summary>
        internal static string PatternManager_IsAlphabetSensitive {
            get {
                return ResourceManager.GetString("PatternManager_IsAlphabetSensitive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 數字  的當地語系化字串。
        /// </summary>
        internal static string PatternManager_IsNumeric {
            get {
                return ResourceManager.GetString("PatternManager_IsNumeric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 符號  的當地語系化字串。
        /// </summary>
        internal static string PatternManager_IsSymbol {
            get {
                return ResourceManager.GetString("PatternManager_IsSymbol", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 升冪降冪連續「{0}」 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_KeyboardOrderLimit {
            get {
                return ResourceManager.GetString("PatternManager_KeyboardOrderLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 長度必須位於{0}~{1}碼! 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_Length {
            get {
                return ResourceManager.GetString("PatternManager_Length", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 至少要{0}種不同的字元 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_MinUniqueChars {
            get {
                return ResourceManager.GetString("PatternManager_MinUniqueChars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 空白  的當地語系化字串。
        /// </summary>
        internal static string PatternManager_WhiteSpace {
            get {
                return ResourceManager.GetString("PatternManager_WhiteSpace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 目前設定不能包含空白字元! 的當地語系化字串。
        /// </summary>
        internal static string PatternManager_WhiteSpaceNotAllowed {
            get {
                return ResourceManager.GetString("PatternManager_WhiteSpaceNotAllowed", resourceCulture);
            }
        }
    }
}
