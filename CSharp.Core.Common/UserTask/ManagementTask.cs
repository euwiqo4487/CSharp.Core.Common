using System;
using System.Collections;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 管理使用者工作
    /// </summary>
    public static class ManagementTask
    {
        /// <summary>
        /// 用來放使用者工作任務識別集合(安全執行緒)
        /// </summary>
        public static Hashtable TaskList = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 新增工作
        /// </summary>
        /// <param name="task"></param>
        public static void AddTask(UserTask task)
        {
            string guid = task.GUID;
            try
            {
                if (!TaskList.ContainsKey(guid))
                    TaskList.Add(guid, task);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("新增使用者工作{0}發生錯誤!", guid), ex);
            }
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <param name="guid">工作識別碼</param>
        public static void RemoveTask(string guid)
        {
            if (TaskList.ContainsKey(guid))
                TaskList.Remove(guid);
        }

        /// <summary>
        /// 取得工作項目
        /// </summary>
        /// <param name="guid">工作識別碼</param>
        /// <returns>工作項目</returns>
        public static UserTask GetTask(string guid)
        {
            if (TaskList.ContainsKey(guid))
                return (TaskList[guid] as UserTask);
            else
                return null;
        }
    }
}
