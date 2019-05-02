using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 靜態類別 協助判斷是否存在Office軟體
    /// </summary>
    public class OfficeHelper
    {
        /// <summary>
        /// 是否存在Word文件
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsExistWord()
        {
            Type tp = Type.GetTypeFromProgID("Word.Application");
            if (tp == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 是否存在Excel文件
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsExistExcel()
        {
            Type tp = Type.GetTypeFromProgID("Excel.Application");
            if (tp == null)
                return false;
            else
                return true;
        }
    }
}
