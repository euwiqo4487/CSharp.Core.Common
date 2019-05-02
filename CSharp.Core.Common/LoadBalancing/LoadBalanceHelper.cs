using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// LoadBalanceHelper
    /// </summary>
    public static class LoadBalanceHelper
    {
        private readonly static Lazy<ConsistentHashing> instanceSignalr = new Lazy<ConsistentHashing>(() => new ConsistentHashing(ManagementAppSettings.GetVal("CSharp.SignalrServiceMapping").JsonToObj<Dictionary<string, string>>().Keys.ToList()));
        private readonly static Lazy<ConsistentHashing> instanceMq = new Lazy<ConsistentHashing>(() => new ConsistentHashing(ManagementAppSettings.GetVal("CSharp.QueueServiceMapping").Split(',').ToList()));
        private readonly static Lazy<ConsistentHashing> instanceWcf = new Lazy<ConsistentHashing>(() => {
            List<string> nodes = new List<string>();// 節點集合
            WcfHelper.GetClientContracts().ForEach((pair) =>
            {// [AuthorityService,CSharp.Services.AP]   [SystemService,CSharp.Services.AP]
                string[] keyArr = pair.Key.Split('_');
                string[] valArr = pair.Value.Split('.');
                nodes.Add(keyArr.Length == 1 ? "" : "_" + keyArr[1]);  //   _1  _2                   
            });
            return new ConsistentHashing(nodes.Distinct().ToList()); 
        });
        /// <summary>
        /// 取得此uno下的Signalr Current Locator
        /// </summary>
        /// <param name="uno"></param>
        /// <returns></returns>
        public static string GetSignalrCurrentLocator(string uno)
        {
            return instanceSignalr.Value.GetPrimary(uno);
        }
        /// <summary>
        /// 取得此uno下的Mq Current Locator
        /// </summary>
        /// <param name="uno"></param>
        /// <returns></returns>
        public static string GetMqCurrentLocator(string uno)
        {
            return instanceMq.Value.GetPrimary(uno);
        }
        /// <summary>
        /// 取得此uno下的Wcf Current Locator
        /// </summary>
        /// <param name="uno"></param>
        /// <returns></returns>
        public static string GetWcfCurrentLocator(string uno)
        {
            return instanceWcf.Value.GetPrimary(uno);
        }
    }
}
