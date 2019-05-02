
namespace CSharp.Core.Common
{
    /// <summary>
    /// 非同步工作項目回呼界面
    /// </summary>
    public interface IUserTaskCallback
    {
        /// <summary>
        /// 非同步工作項目回呼函式
        /// </summary>
        /// <param name="task">非同步工作項目</param>
        void TaskCallBack(UserTask task);
    }
}
