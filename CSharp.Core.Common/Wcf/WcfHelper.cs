using CSharp.Core.Common.Resource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Wcf協助靜態類別 ,如取得 表頭字串資料或取得服務端點名稱集合等
    /// </summary>
    public static class WcfHelper
    {
        /// <summary>
        /// Get IP
        /// </summary>
        public static string GetIP
        {
            get
            {               
                //获取传进的消息属性
                MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
                //获取消息发送的远程终结点IP和端口
                RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                return  endpoint.Address +":" + endpoint.Port;
            }
        }
        /// <summary>
        /// 取得表頭字串資料,由CLIENT到SERVER
        /// </summary>
        /// <param name="headName">表頭名稱</param>
        /// <param name="ns">CSharp網址</param>
        /// <returns>表頭string</returns>
        public static string GetHeader(string headName = "AppToken", string ns = "http://CSharp-group.com/")
        {
            //一致的命名習慣
            int index = OperationContext.Current.IncomingMessageHeaders.FindHeader(headName,ns  + headName);
            if (index == -1)
            {
                throw new Exception(ErrorResource.HeaderIsEmptyError);
            }           
            return OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(headName, ns + headName);

        }
        /// <summary>
        /// 取得服務端點名稱集合
        /// </summary>
        /// <returns>服務端點名稱集合</returns>
        public static List<string> GetClientEndpointName()
        {
            List<string> items = new List<string>();
            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                items.Add(clientSection.Endpoints[i].Name);
            }
            return items;
        }
        /// <summary>
        /// 取得Client服務端點名稱和Uri字典集合
        /// </summary>
        /// <returns>服務端點名稱和Uri字典集合</returns>
        public static Dictionary<string,Uri> GetClientEndpoints()
        {
            Dictionary<string, Uri> items = new Dictionary<string, Uri>();
            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                items.Add(clientSection.Endpoints[i].Name, clientSection.Endpoints[i].Address);
            }
            return items;
        }

        /// <summary>
        /// 取得Client服務端點名稱和合約可重複集合
        /// </summary>
        /// <returns>服務端點名稱和合約可重複集合</returns>
        public static List<KeyValuePair<string, string>> GetClientContracts()
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                items.Add(new KeyValuePair<string, string>(clientSection.Endpoints[i].Name, clientSection.Endpoints[i].Contract));
            }
            return items;
        }
        /// <summary>
        /// 取得services服務名稱
        /// </summary>
        /// <typeparam name="T">LowerCase or String</typeparam>
        /// <returns></returns>
        public static List<T> GetServicesNames<T>() where T : IEquatable<T>
        {
            List<T> items = new List<T>();
            ServicesSection servicesSection = (ServicesSection)ConfigurationManager.GetSection("system.serviceModel/services");
            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                items.Add(AssemblyHelp.CreateObject<T>(new object[] { servicesSection.Services[i].Name }));                
            }
            return items;
        }
        /// <summary>
        /// 取得services Type
        /// </summary>
        /// <param name="serviceNames">集合</param>
        /// <param name="types">型別</param>
        /// <returns>型別</returns>
        public static Type[] GetConfigServiceTypes(List<string> serviceNames,Type[] types) 
        {
            List<Type> typeList = new List<Type>();
            foreach (string name in serviceNames)
            {
                typeList.Add(types.FindSingle(t => t.FullName == name));
            }
            return typeList.ToArray();
        }
        /// <summary>
        /// 取得執行個體主機元件
        /// </summary>
        public static string GetHost { get { return OperationContext.Current.IncomingMessageHeaders.To.Host; } }
        /// <summary>
        /// 取得Service絕對Uri
        /// </summary>
        public static string GetAbsoluteUri { get { return OperationContext.Current.IncomingMessageHeaders.To.AbsoluteUri; } }
        /// <summary>
        /// 取得Service   Port
        /// </summary>
        public static int GetPort { get { return OperationContext.Current.IncomingMessageHeaders.To.Port; } }
        /// <summary>
        /// 取得Operation加上address
        /// </summary>
        public static string GetAction { get { return OperationContext.Current.IncomingMessageHeaders.Action; } }

        // OperationContext.Current.IncomingMessageHeaders.To.AbsolutePath
        //"/UserMenuService"
        // OperationContext.Current.IncomingMessageHeaders.To.AbsoluteUri
        //"net.tcp://localhost/UserMenuService"
        // OperationContext.Current.IncomingMessageHeaders.To.Authority
        //"localhost"
        // OperationContext.Current.IncomingMessageHeaders.To.DnsSafeHost
        //"localhost"
        // OperationContext.Current.IncomingMessageHeaders.To.Fragment
        //""
        // OperationContext.Current.IncomingMessageHeaders.To.Host
        //"localhost"
        // OperationContext.Current.IncomingMessageHeaders.To.HostNameType
        //Dns
        // OperationContext.Current.IncomingMessageHeaders.To.IsAbsoluteUri
        //true
        // OperationContext.Current.IncomingMessageHeaders.To.LocalPath
        //"/UserMenuService"
        // OperationContext.Current.IncomingMessageHeaders.To.PathAndQuery
        //"/UserMenuService"
        // OperationContext.Current.IncomingMessageHeaders.To.Query
        //""
        // OperationContext.Current.IncomingMessageHeaders.To.Scheme
        //"net.tcp"
        // OperationContext.Current.IncomingMessageHeaders.To.UserInfo
        //""
    }
}
