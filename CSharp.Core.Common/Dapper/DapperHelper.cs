using Dapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace CSharp.Core.Common
{
    /// <summary>
    ///  DapperHelper
    /// </summary>
    public static class DapperHelper
    {
        private static DynamicParameters Parameters(DbParameter[] Params)
        {
            if (Params != null)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                foreach (DbParameter param in Params)
                {
                    dynamicParameters.Add(param.ParameterName,
                        param.Value,
                        param.DbType,
                        param.Direction,
                        param.Size);
                }
                return dynamicParameters;
            }
            return null;
        }

        /// <summary>
        /// Dapper靜態查詢
        /// </summary>
        /// <typeparam name="T">指定型別(必要)</typeparam>
        /// <param name="conn">Db連線(必要)</param>
        /// <param name="sql">(必要)</param>
        /// <param name="cmdType">(必要)</param>
        /// <param name="args">(必要)</param>
        /// <param name="trans">(必要)</param>
        /// <param name="buffer">預設是true</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(IDbConnection conn, string sql, CommandType cmdType, DbParameter[] args = null, IDbTransaction trans = null, bool buffer = true)
            where T : class,new()
        {
            return conn.Query<T>(sql, param: Parameters(args), commandType: cmdType, transaction: trans, buffered: buffer);
        }

        /// <summary>
        /// Dapper動態查詢
        /// </summary>
        /// <param name="conn">Db連線(必要)</param>
        /// <param name="sql">(必要)</param>
        /// <param name="cmdType">(必要)</param>
        /// <param name="args">(必要)</param>
        /// <param name="trans">(必要)</param>
        /// <param name="buffer">(必要)</param>
        /// <returns>預設型別:DapperRow</returns>
        public static IEnumerable<dynamic> Query(IDbConnection conn, string sql, CommandType cmdType, DbParameter[] args = null, IDbTransaction trans = null, bool buffer = true)
        {
            return conn.Query(sql, param: Parameters(args), commandType: cmdType, transaction: trans, buffered: buffer);
        }

        /// <summary>
        /// Dapper非查詢執行
        /// </summary>
        /// <param name="conn">Db連線(必要)</param>
        /// <param name="sql">(必要)</param>
        /// <param name="cmdType">(必要)</param>
        /// <param name="args">(必要)</param>
        /// <param name="trans">(必要)</param>
        /// <param name="buffer">(必要)</param>
        /// <returns>影響筆數</returns>
        public static int Execute(IDbConnection conn, string sql, CommandType cmdType, DbParameter[] args = null, IDbTransaction trans = null, bool buffer = true)
        {
            return conn.Execute(sql, param: Parameters(args), commandType: cmdType, transaction: trans);
        }

        /// <summary>
        /// IEnumerable轉型ObservableCollection(擴充方式)
        /// </summary>
        /// <typeparam name="T">(必要)</typeparam>
        /// <param name="source">(必要)</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }
    }
}
