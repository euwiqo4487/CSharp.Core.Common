using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 非同步工作項目
    /// </summary>
    public class UserTask : INotifyPropertyChanged
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public UserTask()
        {
            GUID = Guid.NewGuid().ToString();
            Status = UserTaskStatus.None;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="title">工作標題</param>
        /// <param name="source">呼叫來源</param>
        /// <param name="querykey">查詢鍵值</param>
        public UserTask(string title, object source, string querykey = "") : this()
        {
            this.Title = title;
            if (source != null)
            {
                this.Source = source;
                this.AssemblyName = source.GetType().Assembly.FullName.Split(',')[0];
                this.FormClass = source.GetType().Name;
                if (source is IUserTaskCallback)
                    this.CallBack = (source as IUserTaskCallback).TaskCallBack;
            }
            this.QueryKey = querykey;
        }

        /// <summary>
        /// 設定執行結果訊息
        /// </summary>
        public Action<UserTask> SetResultMessage;

        /// <summary>
        /// 回呼函數
        /// </summary>
        /// <param name="task">工作項目</param>
        public delegate void TaskCallBack(UserTask task);

        /// <summary>
        /// 工作識別碼
        /// </summary>
        public string GUID;
        
        /// <summary>
        /// 工作標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 呼叫來源
        /// </summary>
        public object Source;

        /// <summary>
        /// 呼叫來源組件名稱
        /// </summary>
        public string AssemblyName = "";

        /// <summary>
        /// 呼叫來源類別名稱
        /// </summary>
        public string FormClass = "";

        /// <summary>
        /// 呼叫來源查詢鍵值
        /// </summary>
        public string QueryKey = "";

        /// <summary>
        /// 遠端服務名稱
        /// </summary>
        public string ContractName = "";

        /// <summary>
        /// 遠端作業名稱
        /// </summary>
        public string OperationName = "";

        /// <summary>
        /// 傳遞參數
        /// </summary>
        public object[] Parameters;

        Dictionary<int, Type> _Types = new Dictionary<int,Type>();

        /// <summary>
        /// 傳遞參數的資料型態
        /// </summary>
        public Type[] Types
        {
            get { return _Types.Select(x => x.Value).ToArray(); }
        }

        /// <summary>
        /// 設定傳遞參數的資料型態
        /// </summary>
        public void SetTypes<T>(T obj)
        {
            _Types.Add(_Types.Count, typeof(T));  //此法可避免obj為null或Nullable時,無法透過obj.GetType()取得型態
        }

        /// <summary>
        /// 回呼函數
        /// </summary>
        public TaskCallBack CallBack;

        /// <summary>
        /// 工作開始時間
        /// </summary>
        public DateTime StartTime { get; set; }

        DateTime _EndTime;
        /// <summary>
        /// 工作結束時間
        /// </summary>
        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
                NotifiyPropertyChanged("EndTime");
            }
        }
        
        UserTaskStatus _Status;

        /// <summary>
        /// 工作狀態
        /// </summary>
        public UserTaskStatus Status
        { 
            get { return _Status; }
            set
            {
                _Status = value;
                NotifiyPropertyChanged("Status");
            }
        }

        string _ResultMessage;
        /// <summary>
        /// 執行結果訊息
        /// </summary>
        public string ResultMessage
        {
            get { return _ResultMessage; }
            set
            {
                _ResultMessage = value;
                NotifiyPropertyChanged("ResultMessage");
            }
        }

        /// <summary>
        /// 屬性值變更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message = "";

        /// <summary>
        /// 資料集
        /// </summary>
        public IEnumerable<Dictionary<string, object>> Data;

        /// <summary>
        /// 多個資料集
        /// </summary>
        public DataSet DataTables;

        /// <summary>
        /// 設定回傳資料
        /// </summary>
        /// <param name="data"></param>
        public void SetData(object data)
        {
            if (data.GetType() == typeof(IEnumerable<>))
                this.Data = EntitiesToDictionaries(data);
            else if (data.GetType() == typeof(DataSet))
                this.DataTables = data as DataSet;
            else if (data.GetType() == typeof(DataTable))
            {
                var ds = new DataSet();
                ds.Tables.Add(data as DataTable);
                this.DataTables = ds;
            }
        }

        IEnumerable<Dictionary<string, object>> EntitiesToDictionaries(object data)
        {
            List<Dictionary<string, object>> dictionaries = new List<Dictionary<string, object>>();
            if (data.GetType() == typeof(IEnumerable<>))
            {
                Type TEntity = data.GetType().GenericTypeArguments[0];
                PropertyInfo[] propInfos = TEntity.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                dynamic entities = Convert.ChangeType(data, data.GetType());
                foreach (var entity in entities)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>(propInfos.Length);
                    foreach (PropertyInfo propInfo in propInfos)
                    {
                        dictionary.Add(propInfo.Name, propInfo.GetValue(entity));
                    }
                    dictionaries.Add(dictionary);
                }
            }
            return dictionaries;
        }

        /// <summary>
        /// 將物件陣列轉為字串
        /// </summary>
        /// <param name="objs">物件陣列</param>
        /// <returns>字串</returns>
        public string GetObjString(object[] objs)
        {
            string result = "";
            if (objs != null && objs.Length != 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    object tmp = objs[i];
                    Type t = _Types[i];
                    string isNullable = "";
                    if (t.Name == "Nullable`1" && t.GenericTypeArguments != null && t.GenericTypeArguments.Length > 0)
                    {
                        isNullable = ",Nullable";
                        t = t.GenericTypeArguments[0];
                    }
                    result += (result == "" ? "" : "|") + string.Format("{0}{1}={2}", t.FullName, isNullable, (tmp == null ? "null" : tmp.ToString()));
                }
            }
            return result;
        }

        /// <summary>
        /// 將字串轉為物件陣列
        /// </summary>
        /// <param name="objString">字串</param>
        /// <returns>物件陣列</returns>
        public object[] GetObjectArray(string objString)
        {
            if (string.IsNullOrEmpty(objString)) return null;

            string[] tmp = objString.Split('|');
            object[] objs = new object[tmp.Length];
            int i = 0;
            _Types.Clear();
            foreach (string s in tmp)
            {
                string s2 = s;
                bool isNullable = false;
                if (s2.IndexOf("Nullable") > 0)
                {
                    isNullable = true;
                    s2 = s2.Replace(",Nullable", "");
                }

                Type t = Type.GetType(s2.Split('=')[0]);
                Type t2 = t;
                if (isNullable)
                    t2 = typeof(Nullable<>).MakeGenericType(t);

                _Types.Add(i, t2);
                if (s2.Split('=')[1] == "null")
                    objs[i++] = null;
                else
                    objs[i++] = Convert.ChangeType(s2.Split('=')[1], t);
            }
            return objs;
        }

        /// <summary>
        /// 取得工作內容Hashtable
        /// </summary>
        /// <returns></returns>
        public Hashtable ToHashtable()
        {
            Hashtable ht = new Hashtable();
            ht.Add("Guid", this.GUID);
            ht.Add("Name", this.Title);
            ht.Add("Assembly", this.AssemblyName);
            ht.Add("FormClass", this.FormClass);
            ht.Add("QueryKey", this.QueryKey);
            ht.Add("Service", this.ContractName);
            ht.Add("Operation", this.OperationName);
            ht.Add("Parameters", GetObjString(this.Parameters));
            ht.Add("StartTime", this.StartTime);
            ht.Add("EndTime", this.EndTime);
            ht.Add("Status", (int)this.Status);
            ht.Add("Remark", this.Message);
            return ht;
        }

        /// <summary>
        /// 由Hashtable建立工作
        /// </summary>
        /// <param name="ht"></param>
        public void Load(Hashtable ht)
        {
            this.GUID = ht["Guid"].ToString();
            this.Title = ht["Name"].ToString();
            this.AssemblyName = ht["Assembly"].ToString();
            this.FormClass = ht["FormClass"].ToString();
            this.QueryKey = ht["QueryKey"].ToString();
            this.ContractName = ht["Service"].ToString();
            this.OperationName = ht["Operation"].ToString();
            this.Parameters = GetObjectArray(ht["Parameters"].ToString());
            this.StartTime = (DateTime)ht["StartTime"];
            this.EndTime = (DateTime)ht["EndTime"];
            this.Status = (UserTaskStatus)ht["Status"];
            this.Message = ht["Remark"].ToString();
        }
    }

    /// <summary>
    /// 工作狀態
    /// </summary>
    public enum UserTaskStatus
    {
        /// <summary>
        /// 尚未開始
        /// </summary>
        None,
        /// <summary>
        /// 執行中
        /// </summary>
        Running,
        /// <summary>
        /// 完成
        /// </summary>
        Finish,
        /// <summary>
        /// 失敗
        /// </summary>
        Fail,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }
}
