using System.Collections.Generic;
using System.Text;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 每列資料
    /// </summary>
    public class CsvRow : List<string>
    {         
        /// <summary>
        /// 列資料
        /// </summary>
        public string LineText {get;set;}
    }
}
