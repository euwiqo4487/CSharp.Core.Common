using System.ComponentModel;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp SignalR訊息定義
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 無
        /// </summary>
        [Description("無")]
        None = 0,

        /// <summary>
        /// 廣播
        /// </summary>
        [Description("廣播")]
        Broadcast = 1,

        /// <summary>
        /// 個人
        /// </summary>
        [Description("個人")]
        Personal = 2,

        /// <summary>
        /// 更新使用者清單
        /// </summary>
        [Description("更新使用者清單")]
        RefreshUsers = 9,

        /// <summary>
        /// 系統
        /// </summary>
        [Description("系統")]
        System = 10,

        /// <summary>
        /// 資料快取更新
        /// </summary>
        [Description("資料快取更新")]
        DataCacheRefresh = 11,

        /// <summary>
        /// 自訂
        /// </summary>
        [Description("自訂")]
        Custom = 99
    }
}
