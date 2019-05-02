using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    ///  提供給public IEnumerable TEntity  ExecuteReaderEx TEntity (String commandText, CommandType commandType, params DbParameter[] args) 方法使用 
    ///  ,TEntity需實作Initialize方法,reader的欄位名稱不須和資料物件一樣,因為邏輯在Initialize方法裡
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 初始化資料物件值
        /// </summary>
        /// <param name="args">參數</param>
        void Initialize(params object[] args);
    }
}
