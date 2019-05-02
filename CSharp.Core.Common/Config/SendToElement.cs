using System;
using System.Configuration;

namespace CSharp.Core.Common
{
    /// <summary>
    /// CSharp.Config的Mail寄件清單區段
    /// </summary>
    [ConfigurationCollection(typeof(SendToElement), AddItemName = "SendTo", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SendList : GenericElementCollection<SendToElement>
    {  
        /// <summary>
        /// 取得SendToElement
        /// </summary>
        /// <param name="Name">名字</param>
        /// <returns>SendTo Element</returns>
        public SendToElement GetSendList(String Name) 
        { 
            return BaseGet(Name) as SendToElement; 
        }
    }
    /// <summary>
    /// CSharp.Config的Mail收件者清單區段
    /// </summary>
    public class SendToElement : GenericElement
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public SendToElement()
        {
        }
        /// <summary>
        /// SendToElement
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="address">mail address</param>
        public SendToElement(string name, string address)
        {
            this.Name = name;
            this.Address = address;
        }
        /// <summary>
        /// 當作識別的KEY
        /// </summary>
        [ConfigurationProperty("Name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }
        /// <summary>
        /// Address
        /// </summary>
        [ConfigurationProperty("Address", DefaultValue = "", IsRequired = true)]
        public string Address
        {
            get { return (string)base["Address"]; }
            set { this["Address"] = value; }
        }
        /// <summary>
        /// 注意,這裡要給 Key值,取得資料用
        /// </summary>
        public override object ElementKey
        {
            get { return this.Name; }
        }
    }
    
}
