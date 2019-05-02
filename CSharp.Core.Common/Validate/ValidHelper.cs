using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{

    /// <summary>
    /// Valid協助靜態類別 ,如有定義物件模型屬性的檢核屬性,可使用
    /// </summary>
    public static class ValidHelper
    {
        /// <summary>
        /// 將物件模型屬性做檢核
        /// </summary>
        /// <example>
        /// <code language="cs" title=" ">
        /// List&lt;ValidationResult&gt; vv;
        /// Employee emp = new Employee();
        /// if (!emp.IsValid(out vv))
        /// {
        ///     Console.WriteLine(vv.ToString());
        /// }
        ///     Console.ReadKey();
        /// </code>
        /// </example>
        ///  <param name="obj">物件模型</param>
        /// <param name="validations">檢核結果清單</param>         
        /// <returns>true:檢核OK</returns>
        public static bool IsValid(this object obj, out List<ValidationResult> validations)
        {
            //ValidationContext context = new ValidationContext(obj, null, null);
            ValidationContext context = new ValidationContext(obj);
            validations = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, context, validations, true);
        }
    }
}
