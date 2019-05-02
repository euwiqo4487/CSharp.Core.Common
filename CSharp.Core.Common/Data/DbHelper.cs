using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Db協助靜態類別 ,做 資料物件的ORM 由 DbDataReader 或 DataTable 轉成資料物件
    /// </summary>
    public static class DbHelper
    {
        #region SetPropertyValue 做 mapping         
        /// <summary>
        /// 對泛型資料物件欄位做mapping
        /// </summary>
        /// <typeparam name="TEntity">泛型資料型別</typeparam>
        /// <param name="entity">泛型資料物件</param>
        /// <param name="propName">屬性名稱</param>
        /// <param name="val">值</param>
        public static void SetPropertyValue<TEntity>(TEntity entity, string propName, object val)
        {
            //PropertyInfo info = typeof(TEntity).GetProperty(propyName);
            PropertyInfo info = entity.GetType().GetProperty(propName);
            if (null == info) return;
            try
            {
                if (info.PropertyType.Equals(typeof(string)))
                {
                    info.SetValue(entity, val.ToString());
                    //info.SetValue(entity, val.ToString(), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(decimal)) || info.PropertyType.Equals(typeof(decimal?)))
                {
                    info.SetValue(entity, Convert.ToDecimal(val));
                    //info.SetValue(entity, Convert.ToDecimal(val), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(int)) || info.PropertyType.Equals(typeof(int?)))
                {
                    info.SetValue(entity, Convert.ToInt32(val));
                    //info.SetValue(entity, Convert.ToInt32(val), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(byte)))
                {
                    info.SetValue(entity, Convert.ToByte(val));
                    //info.SetValue(entity, val.ToString(), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(Boolean)))
                {
                    info.SetValue(entity, Convert.ToBoolean(val));
                    //info.SetValue(entity, Convert.ToBoolean(val), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(DateTime)))
                {
                    info.SetValue(entity, Convert.ToDateTime(val));
                    //info.SetValue(entity, Convert.ToDateTime(val), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(double)) || info.PropertyType.Equals(typeof(double?)))
                {
                    info.SetValue(entity, Convert.ToDouble(val));
                    //info.SetValue(entity, Convert.ToDouble(val), new object[0]);
                    return;
                }

                if (info.PropertyType.Equals(typeof(Int64)) || info.PropertyType.Equals(typeof(Int64?)))
                {
                    info.SetValue(entity, Convert.ToInt64(val));
                    return;
                }

                if (info.PropertyType.Equals(typeof(Int16)) || info.PropertyType.Equals(typeof(Int16?)))
                {
                    info.SetValue(entity, Convert.ToInt16(val));
                    return;
                }

                if (info.PropertyType.Equals(typeof(char)))
                {
                    info.SetValue(entity, Convert.ToChar(val));
                    return;
                }

                if (info.PropertyType.Equals(typeof(sbyte)))
                {
                    info.SetValue(entity, Convert.ToSByte(val));
                    return;
                }

                if (info.PropertyType.Equals(typeof(Single)))
                {
                    info.SetValue(entity, Convert.ToSingle(val));
                    return;
                }

                if (info.PropertyType.Equals(typeof(byte[])))
                {
                    info.SetValue(entity, (byte[])val);
                    return;
                }
            }
            catch
            {
                throw;
            }
        }
        //public static void SetPropertyValues<TEntity>(Type type, Dictionary<PropertyInfo, int> entityPairings, TEntity entity, params object[] args)
        //{
        //    foreach (PropertyInfo propInfo in entityPairings.Keys)
        //    {               
        //        switch (propInfo.PropertyType.Name)
        //        {
        //            case "String":
        //                type.GetProperty(propInfo.Name).SetValue(entity, args[entityPairings[propInfo]].ToString());
        //                break;
        //            case "Int32":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToInt32(args[entityPairings[propInfo]]));
        //                break;
        //            case "Decimal":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToDecimal(args[entityPairings[propInfo]]));
        //                break;
        //            case "Byte":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToByte(args[entityPairings[propInfo]]));
        //                break;
        //            case "Int64":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToInt64(args[entityPairings[propInfo]]));
        //                break;
        //            case "Int16":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToInt16(args[entityPairings[propInfo]]));
        //                break;
        //            case "Boolean":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToBoolean(args[entityPairings[propInfo]]));
        //                break;
        //            case "DateTime":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToDateTime(args[entityPairings[propInfo]]));
        //                break;
        //            case "Double":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToDouble(args[entityPairings[propInfo]]));
        //                break;
        //            case "Char":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToChar(args[entityPairings[propInfo]]));
        //                break;
        //            case "SByte":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToSByte(args[entityPairings[propInfo]]));
        //                break;
        //            case "Single":
        //                type.GetProperty(propInfo.Name).SetValue(entity, Convert.ToSingle(args[entityPairings[propInfo]]));
        //                break;
        //            case "Byte[]":
        //                type.GetProperty(propInfo.Name).SetValue(entity, (Byte[])args[entityPairings[propInfo]]);
        //                break;
        //            case "undefined":
        //                break;
        //            default:
        //                type.GetProperty(propInfo.Name).SetValue(entity, null);
        //                break;
        //        }                                  
        //    }
        //}
        #endregion  
        /// <summary>
        /// 提供 將Entity轉成  Hashtable 方便更新資料庫
        /// </summary>         
        /// <param name="entity"></param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetEntityHashtable(this object entity)
        {
            Hashtable htData = new Hashtable();
            PropertyInfo[] propInfos = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < propInfos.Length; i++)
            {
                htData.Add(propInfos[i].Name, propInfos[i].GetValue(entity));
            }
            return htData;
        }
        /// <summary>
        /// 物件型別實體和資料欄位配對
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns></returns>
        public static Dictionary<int, PropertyInfo> GetEntityPairing<TEntity>(this DbDataReader reader)
        {
            Type type = typeof(TEntity);
            Dictionary<int, PropertyInfo> entityPairings = new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                PropertyInfo info = type.GetProperty(reader.GetName(i));
                if (null == info)
                {
                    continue;
                }
                else
                {
                    entityPairings.Add(i, info);
                }
            }
            return entityPairings;
        }
        /// <summary>
        /// 將DbDataReader Binding 到資料物件方式
        /// </summary>         
        /// <typeparam name="TEntity">泛型資料物件型別</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>列舉集合</returns>
        public static IEnumerable<TEntity> DataReaderBinding<TEntity>(this DbDataReader reader) where TEntity : class, new()
        {
            List<TEntity> Entities = new List<TEntity>();
            int fieldCount = reader.FieldCount;
            Dictionary<int, PropertyInfo> entityPairings = reader.GetEntityPairing<TEntity>();
            while (reader.Read())
            {
                TEntity entity = new TEntity();
                object[] values = new object[fieldCount];
                reader.GetValues(values);
                foreach (int index in entityPairings.Keys)
                {
                    object val = values[index];
                    if (val == DBNull.Value) continue;
                    entityPairings[index].SetValue(entity, val);
                }
                Entities.Add(entity);
            }
            return Entities;
        }
        ///// <summary>
        ///// 將DbDataReader Binding 到資料物件方式
        ///// </summary>
        ///// <seealso cref="DbDataReader"/>
        ///// <typeparam name="TEntity">泛型資料物件型別</typeparam>
        ///// <param name="reader">DbDataReader</param>
        ///// <returns>列舉集合</returns>
        //public static IEnumerable<TEntity> DataReaderBinding<TEntity>(this DbDataReader reader)
        //    where TEntity : class,new()
        //{            
        //    List<TEntity> Entities = new List<TEntity>();
        //    while (reader.Read())
        //    {
        //        TEntity entity = new TEntity();                
        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            if (!reader.GetValue(i).Equals(DBNull.Value))
        //            {
        //                DbHelper.SetPropertyValue<TEntity>(entity, reader.GetName(i), reader.GetValue(i));
        //            }
        //        }
        //        Entities.Add(entity);
        //    }        
        //    return Entities;
        //}
        /// <summary>
        /// DataReaderBindingEx 針對泛型型別且具有IEntity介面的,但要確保Reader的欄位順序和Initialize參數一致  
        /// 泛型型別的欄位需允許null 例如    int?  bool?    
        /// reader的欄位名稱不須和資料物件一樣,因為邏輯在Initialize方法裡
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> DataReaderBindingEx<TEntity>(this DbDataReader reader) where TEntity : class, IEntity, new()
        {
            //Func<object> createObj = AssemblyHelp.DynamicNew<TEntity>();//產生委讓方法給迴圈反覆呼叫
            List<TEntity> Entities = new List<TEntity>();
            int fieldCount = reader.FieldCount;
            while (reader.Read())
            {
                object[] values = new object[fieldCount];
                reader.GetValues(values);
                TEntity entity = new TEntity();
                //TEntity entity = createObj() as TEntity;
                entity.Initialize(values);
                Entities.Add(entity);
            }
            return Entities;
        }
        /// <summary>
        /// DataTable Binding 到資料物件方式 ,資源耗損多 建議使用 DataReaderBinding
        /// </summary>
        /// <typeparam name="TEntity">泛型資料物件型別</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>列舉集合</returns>
        public static IEnumerable<TEntity> DataTableBinding<TEntity>(this DataTable table) where TEntity : class, new()
        {
            Func<object> createObj = AssemblyHelp.DynamicNew<TEntity>();//產生委讓方法給迴圈反覆呼叫
            List<TEntity> Entities = new List<TEntity>();
            foreach (DataRow row in table.Rows)
            {
                //TEntity entity = new TEntity();
                TEntity entity = createObj() as TEntity;
                foreach (DataColumn column in table.Columns)
                {
                    //Console.WriteLine(row[column]);
                    SetPropertyValue<TEntity>(entity, column.ColumnName, row[column]);
                }
                Entities.Add(entity);
            }
            return Entities;
        }
        /// <summary>
        /// 將實體清單轉換成字典清單
        /// </summary>
        /// <typeparam name="TEntity">實體型別</typeparam>
        /// <param name="entities">實體清單</param>
        /// <returns>字典清單</returns>
        public static IEnumerable<Dictionary<string, object>> EntitiesToDictionaries<TEntity>(this IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            List<Dictionary<string, object>> dictionaries = new List<Dictionary<string, object>>();
            PropertyInfo[] propInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (TEntity entity in entities)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>(propInfos.Length);
                foreach (PropertyInfo propInfo in propInfos)
                {
                    dictionary.Add(propInfo.Name, propInfo.GetValue(entity));
                }
                dictionaries.Add(dictionary);
            }
            return dictionaries;
        }
        /// <summary>
        /// 將字典清單轉換成實體清單
        /// </summary>
        /// <typeparam name="TEntity">實體型別</typeparam>
        /// <param name="dictionaries">字典清單</param>
        /// <returns>實體清單</returns>
        public static IEnumerable<TEntity> DictionariesToEntities<TEntity>(this IEnumerable<Dictionary<string, object>> dictionaries) where TEntity : class, new()
        {
            List<TEntity> entities = new List<TEntity>();
            PropertyInfo[] propInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (Dictionary<string, object> dictionary in dictionaries)
            {
                TEntity entity = new TEntity();
                foreach (PropertyInfo propInfo in propInfos)
                {
                    propInfo.SetValue(entity, dictionary[propInfo.Name]);
                }
                entities.Add(entity);
            }
            return entities;
        }
        /// <summary>
        /// 產生查詢語法,使用參數方式
        /// </summary>
        /// <param name="queryData">查詢參數</param>
        /// <param name="args">外部參數</param>
        /// <param name="alias">別名</param>
        /// <returns>查詢參數字串</returns>
        public static string GeneratedQuery(this Hashtable queryData, ref Hashtable args, string alias = "")
        {
            int i = 0;
            string fieldName, fieldOperator, fieldObj, argName, query = "";
            foreach (string key in queryData.Keys)
            {
                string[] fieldAttrs = key.Split('|');
                fieldName = fieldAttrs[0];
                fieldObj = fieldAttrs[2].ToLower();
                if (fieldObj == "select")
                {
                    if (Convert.ToString(queryData[key]) == "0")
                    {
                        continue;
                    }
                }
                fieldOperator = fieldAttrs[3].ToLower();
                argName = "q" + i.ToString();
                query += " and " + (alias == "" ? "" : alias + ".") + fieldName + " " + fieldOperator + " @" + argName;
                if (fieldOperator == "like")
                {
                    args.Add(argName, "%" + Convert.ToString(queryData[key]) + "%");
                }
                else
                {
                    args.Add(argName, queryData[key]);
                }
                i++;
            }
            return query;
        }
        /// <summary>
        /// 產生查詢語法,使用字串方式
        /// </summary>
        /// <param name="queryData">查詢參數</param>
        /// <param name="alias">別名</param>
        /// <returns>查詢 字串</returns>
        public static string GeneratedQuery(this Hashtable queryData, string alias = "")
        {
            string fieldName, fieldType, fieldOperator, fieldObj, fieldValue, query = "";
            foreach (string key in queryData.Keys)
            {
                string[] fieldAttrs = key.Split('|');
                fieldName = fieldAttrs[0];
                fieldType = fieldAttrs[1].ToLower(); ;
                fieldObj = fieldAttrs[2].ToLower();
                if (fieldObj == "select")
                {
                    if (Convert.ToString(queryData[key]) == "0")
                    {//沒選
                        continue;
                    }
                }
                fieldOperator = fieldAttrs[3].ToLower();
                fieldValue = Convert.ToString(queryData[key]).Replace("--", "").Replace("'", "").Replace("=", "").Replace(@"""", "");
                if (fieldOperator == "like")
                {
                    fieldValue = (fieldOperator == "like" ? "%" + fieldValue + "%" : fieldValue);
                }
                string format = (alias == "" ? "" : alias + ".") + ((fieldType.IndexOf("string") > -1 || fieldType.IndexOf("datetime") > -1 || fieldType.IndexOf("date") > -1) ? "{0} {1} '{2}'" : "{0} {1} {2}");
                query += " and " + string.Format(format, fieldName, fieldOperator, fieldValue);
            }
            return query;
        }
    }
}
