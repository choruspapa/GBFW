using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Eland.GBFW.Win.Common.Interface;
using Microsoft.Practices.ObjectBuilder;
using Eland.GBFW.EnvironmentMng.Service;
using System.Xml;
using Formular.Support.Extension.HeaderTransfer;
using Eland.GBFW.Common.Util;

namespace Eland.GBFW.Win.Shell.Services
{
    public class HeaderService : IHeaderService, IBuilderAware
    {
        [Dependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }

        private IEnvironmentMngService environmentMngService = null;

        #region IClientProxy Members

        public virtual void AppendClientHeader(Formular.Support.Extension.HeaderTransfer.ITransferHeader header)
        {
            object userId = this.WorkItem.State["UserId"];
            if( userId != null ) header.Add("UserId", this.WorkItem.State["UserId"].ToString());
        }

        public virtual void ServerHeaderVersionChanged(Version oldVersion, Version newVersion, System.Xml.XmlDocument headerDocument)
        {
            if( environmentMngService == null ) environmentMngService = SpringUtil.GetObject<IEnvironmentMngService>("EnvironmentMngService");
            string appSettingInfoXml = environmentMngService.GetAppSettingInfo();
            string key = string.Empty;
            string value = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(string.Format("<root>{0}</root>", appSettingInfoXml));
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment) continue;
                key = node.SelectSingleNode("key").InnerText;
                value = node.SelectSingleNode("value").InnerText;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    WorkItem.State[key] = value;
            }
        }

        public virtual void ConnectionChanged(string oldIpAddress, string oldHostName, string newIpAddress, string newHostName)
        {
            this.WorkItem.State["ServerIpAddress"] = newIpAddress;
            this.WorkItem.State["ServerHostName"] = newHostName;
        }

        #endregion

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            //environmentMngService = SpringUtil.GetContextObject<IEnvironmentMngService>("EnvironmentMngService");
        }

        public void OnTearingDown() { }

        #endregion

    }
}
