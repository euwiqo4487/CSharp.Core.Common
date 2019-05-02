using System.Data;
using System.Dynamic;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Dynamic Reader類別 ,利用Dynamic方式將IDataRecord型別的屬性值取出
    /// </summary>
    public class DynamicReader : DynamicObject
    {        
        readonly IDataRecord _dataRecord;
        /// <summary>
        /// 使用Dynamic讀出DatrReader的欄位
        /// </summary>
        /// <example>
        /// <code language="cs" title="DynamicReader 用法">
        /// var db = DbFactory.Create();
        /// var dr = db.ExecuteReaderByText("SELECT TOP 1000 [LastName],[FirstName],[BirthDate],[Address],[City] FROM [Northwind].[dbo].[Employees]");
        /// dynamic dc = new DynamicReader(dr);
        /// while (dr.Read())
        ///  {
        ///      Employee emp = new Employee();
        ///      emp.LastName = dc.LastName;
        ///      emp.FirstName = dc.FirstName;
        ///      emp.BirthDate = dc.BirthDate;
        ///      emp.Address = dc.Address;
        ///      emp.City = dcd.City;
        ///      Console.WriteLine(dc.LastName + dc.FirstName  + dc.BirthDate  + dc.Address  + dc.City );
        ///  }
        ///  dr.Close();
        ///  Console.ReadKey();
        /// </code>
        /// </example>
        /// <param name="dr">IDataRecord物件</param>
        public DynamicReader(IDataRecord dr)
        {
            _dataRecord = dr;
        }
        /// <summary>
        /// Dynamic 物件 試著取值除來 Try Get Member
        /// </summary>
        /// <param name="binder">表示呼叫位置上動態取得成員</param>
        /// <param name="result">結果值</param>
        /// <returns>true:取值成功 false:失敗</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dataRecord[binder.Name];
            return true;
        }
    }
}
