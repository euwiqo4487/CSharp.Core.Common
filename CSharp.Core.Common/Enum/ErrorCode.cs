
namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp定義給Client端使用服務時,產生的錯誤碼,讓Client程式繼續處理 
    /// 這裡的Client端可為  WPF  WEB  等使用服務的...
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 沒有
        /// </summary>
        none,
        /// <summary>
        /// 系統例外
        /// </summary>
        Exception,
        /// <summary>
        /// 自訂訊息
        /// </summary>
        Custom,
        /// <summary>
        /// 使用者超過時間未使用
        /// </summary>
        IdentityTimeOut,
        /// <summary>
        /// 不正確的使用者token
        /// </summary>
        InvalidToken
    }

}
