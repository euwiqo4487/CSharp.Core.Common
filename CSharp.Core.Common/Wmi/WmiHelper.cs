using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Windows Management Infrastructure  Helper
    /// </summary>
    public static class WmiHelper
    {
        /// <summary>
        /// 取得Cluster名稱
        /// </summary>
        /// <param name="serverName">Server Name</param>
        /// <param name="userName">帳號</param>
        /// <param name="passWord">密碼</param>
        /// <returns>Cluster Name</returns>
        public static string GetClusterNames(string serverName, string userName = "", string passWord = "")
        {
            List<string> clusterNames = new List<string>();
            foreach (ManagementObject item in Query("\\\\" + serverName + "\\root\\MSCluster", "SELECT name FROM MSCluster_cluster", userName, passWord))
            {
                return item["name"].ToString();
            }
            return "";
        }
        /// <summary>
        /// 取得Cluster底下 Node名稱
        /// </summary>
        /// <param name="serverName">Server Name</param>
        /// <param name="userName">帳號</param>
        /// <param name="passWord">密碼</param>
        /// <returns>Cluster Node Name List</returns>
        public static List<string> GetClusterNodeNames(string serverName, string userName = "", string passWord = "")
        {
            List<string> clusterNames = new List<string>();
            foreach (ManagementObject item in Query("\\\\" + serverName + "\\root\\MSCluster", "SELECT name FROM MSCluster_node", userName, passWord))
            {
                clusterNames.Add(item["name"].ToString());
            }
            return clusterNames;
        }
        /// <summary>
        /// 取得Cluster Role名稱
        /// </summary>
        /// <param name="serverName">Server Name</param>
        /// <param name="userName">帳號</param>
        /// <param name="passWord">密碼</param>
        /// <returns>Cluster Node Name List</returns>
        public static List<string> GetClusterRoleNames(string serverName = ".", string userName = "", string passWord = "")
        {
            List<string> roleNames = new List<string>();
            foreach (ManagementObject item in Query("\\\\" + serverName + "\\root\\MSCluster", "SELECT name FROM MSCluster_ResourceGroup", userName, passWord))
            {
                roleNames.Add(item["name"].ToString());
            }
            return roleNames;
        }         
        /// <summary>
        /// 取得Cluster Role state
        /// </summary>
        /// <param name="serverName">Server Name</param>
        /// <param name="name">Role name</param>
        /// <param name="userName">帳號</param>
        /// <param name="passWord">密碼</param>
        /// <returns>Cluster State</returns>
        public static ClusterState GetClusterRoleState(string serverName, string name, string userName = "", string passWord = "")
        {
            foreach (ManagementObject item in Query("\\\\" + serverName + "\\root\\MSCluster", "SELECT state FROM MSCluster_ResourceGroup WHERE name = '" + name + "'",userName,passWord))
            {
                return (ClusterState)Enum.Parse(typeof(ClusterState), item["state"].ToString());
            }
            return ClusterState.StateUnknown;
        }
        /// <summary>
        /// 取得Cluster Role Active Node 字典清單
        /// </summary>
        /// <param name="serverName">server Name</param>
        /// <param name="userName">帳號</param>
        /// <param name="passWord">密碼</param>
        /// <returns>Get Cluster Role Active Node</returns>
        public static Dictionary<string, string> GetClusterRolesActiveNode(string serverName, string userName = "", string passWord = "")
        {
            Dictionary<string, string> rolesActiveNode = new Dictionary<string, string>();
            foreach (ManagementObject item in Query("\\\\" + serverName + "\\root\\MSCluster", "SELECT GroupComponent,PartComponent FROM MSCluster_NodeToActiveGroup", userName, passWord))
            {
                string role = item["GroupComponent"].ToString().Replace("MSCluster_ResourceGroup.Name=", "").Replace("\"", "");
                string node = item["PartCOmponent"].ToString().Replace("MSCluster_Node.Name=", "").Replace("\"", "");               
                rolesActiveNode.Add(role, node);
            }
            return rolesActiveNode;
        }
        /// <summary>
        /// WMI查詢
        /// </summary>
        /// <param name="path">伺服器和命名空間</param>
        /// <param name="wql">WMI Query Language</param>
        /// <param name="userName">帳號</param>
        /// <param name="passWord">密碼</param>
        /// <returns></returns>
        public static ManagementObjectCollection Query(string path, string wql, string userName = "", string passWord = "")
        {
            ConnectionOptions connOptions = new ConnectionOptions(); //New connection option
            connOptions.Authentication = System.Management.AuthenticationLevel.PacketPrivacy;
            if (userName != "") 
            {
                connOptions.Username = userName;
                connOptions.Password = passWord;
            }
            ManagementScope scope = new ManagementScope(path, connOptions);
            ObjectQuery query = new ObjectQuery(wql);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            return searcher.Get();
        }
    }
}
