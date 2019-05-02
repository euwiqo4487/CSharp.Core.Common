using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// csv helper
    /// </summary>
    public static class CsvHelper
    {
        /// <summary>
        /// Parse Line
        /// </summary>
        /// <param name="line">Line</param>
        /// <param name="separator">分隔符號</param>
        /// <returns></returns>
        public static IEnumerable<string> ParseLine(this string line,char separator = ',')
        {
            string[] fields = line.Split(separator);
            foreach (string s in fields)
                yield return s;
        }
        /// <summary>
        /// Parse Line
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="separator">分隔符號</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<string>> ParseLine(this IEnumerable<string> lines, char separator = ',')
        {
            foreach (string line in lines)
            {
                yield return line.ParseLine(separator);
            }                
        }
        /// <summary>
        /// 讀出CSV
        /// </summary>
        /// <code>
        /// var values = from line in src.ReadLines(path, encoding,appendFields)
        ///              select line.ParseLine();
        ///              or
        /// var values = (from line in src.ReadLines(path, encoding,appendFields)
        ///              select line.ParseLine()).TakeWhile((lineOfValues) => lineOfValues.Contains(""));              
        /// </code> 
        /// <param name="path"></param>
        /// <param name="encoding">編碼</param>
        /// <param name="appendFields">擴充欄位和值</param>
        /// <param name="separator">分隔符號</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string path, Encoding encoding, Dictionary<string, string> appendFields = null, string separator = ",")
        {
            string header = "";
            string field = "";
            if (appendFields != null)
            {
                header = separator + appendFields.Keys.JoinCollection(separator);
                field = separator + appendFields.Values.JoinCollection(separator);
            }
            using (StreamReader reader = new StreamReader(path, encoding))
            {//確保全部讀回後 才將串流關閉,配合 延遲載入特性
                string line = reader.ReadLine();                    
                if (null != line)
                {
                    yield return line + header;//擴充表頭加尾巴
                    line = reader.ReadLine();
                }                
                while (null != line)
                {
                    yield return line + field;//擴充固定值加尾巴
                    line = reader.ReadLine();
                }
            }
        }
        /// <summary>
        /// 給client將ResponseMsg的CSV格式字串形成列舉
        /// </summary>
        /// <param name="csv">CSV格式字串</param>  
        /// <param name="appendFields">擴充欄位和值</param>
        /// <param name="separator">分隔符號</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string csv, Dictionary<string, string> appendFields = null, string separator = ",")
        {
            string header = "";
            string field = "";
            if (appendFields != null)
            {
                header = separator + appendFields.Keys.JoinCollection(separator);
                field = separator + appendFields.Values.JoinCollection(separator);
            }
            IEnumerator<string> iterator = csv.Split('\n').ForEach(L => L.Replace("\r", string.Empty)).GetEnumerator();
            if (iterator.MoveNext()){
                yield return iterator.Current + header;
            }
            while (iterator.MoveNext())
            {
                yield return iterator.Current + field;    
            }             
        } 
        /// <summary>
        /// Csv To DataTable
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="path">path</param>
        /// <param name="encoding">編碼</param>
        /// <param name="appendFields">擴充欄位和值</param>
        /// <param name="separator">分隔符號</param>
        /// <returns>DataTable</returns>
        public static DataTable CsvToDataTable(string tableName, string path, Encoding encoding, Dictionary<string, string> appendFields = null, char separator = ',')
        {
            //IEnumerator<IEnumerable<string>> iterator = (from line in reader.ReadLines(path, encoding,appendFields)
            //                                             select line.ParseLine()).GetEnumerator();
            IEnumerator<IEnumerable<string>> iterator = ReadLines(path, encoding, appendFields).ParseLine(separator).GetEnumerator(); 
            if (!iterator.MoveNext()) return null;
            DataTable dt = new DataTable(tableName);
            foreach (string header in iterator.Current)
            {
                dt.Columns.Add(header);//先表頭 
            }             
            while (iterator.MoveNext())
            {
                int i = 0;
                DataRow row = dt.NewRow();
                //row.ItemArray = iterator.Current.ToArray<object>();
                foreach (string val in iterator.Current)
                {
                    if (string.IsNullOrEmpty(val.Trim()))
                    {
                        row[i++] = DBNull.Value;
                    }
                    else
                    {
                        row[i++] = val;
                    }
                }
                dt.Rows.Add(row);//欄位值                            
            }
            return dt;
        }
        /// <summary>
        /// Csv To DataTable
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="csv">CSV格式字串</param>
        /// <param name="appendFields">擴充欄位和值</param>
        /// <param name="separator">分隔符號</param>
        /// <returns></returns>
        public static DataTable CsvToDataTable(string tableName, string csv, Dictionary<string, string> appendFields = null, char separator = ',')
        {
            IEnumerator<IEnumerable<string>> iterator = ReadLines(csv, appendFields, separator.ToString()).ParseLine(separator).GetEnumerator();
            if (!iterator.MoveNext()) return null;            
            DataTable dt = new DataTable(tableName);
            foreach (string header in iterator.Current)
            {
                dt.Columns.Add(header);//先表頭 
            }           
            while (iterator.MoveNext())
            {
                int i = 0;
                DataRow row = dt.NewRow();                
                //row.ItemArray = iterator.Current.ToArray<object>();
                foreach ( string val in iterator.Current )
                {
                    if (string.IsNullOrEmpty(val.Trim()))
                    {
                        row[i++] = DBNull.Value;
                    }
                    else
                    {
                        row[i++] = val;
                    }                    
                }
                    dt.Rows.Add(row);//欄位值                       
            }             
            return dt;
        }
    }
}
