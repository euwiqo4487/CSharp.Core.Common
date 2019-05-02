using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 針對Log紀錄 增強行為設定
    /// </summary>
    public interface ILogBehaviorSetting
    {
        /// <summary>
        /// 取的Log紀錄 增強行為
        /// </summary>
        /// <returns></returns>
        Func<Dictionary<string, string>, Dictionary<string, string>, Hashtable> GetLogBehavior();
    }
}
