using System;
using System.Collections.Generic;
using System.Text;
using Formular.Support.Extension.HeaderTransfer;
using Eland.GBFW.Win.Common.Interface;
using Eland.GBFW.Win.Shell.Services;
using Eland.GBFW.Common.Util;

namespace Eland.GBFW.Win.Shell.Advice
{
    public class ClientSoapExtension : Formular.Support.Extension.HeaderTransfer.WebService.ClientSoapExtension
    {
        private const string CLIENT_INFO_KEY = "WebServiceClientInfo";
        private static IHeaderService _service = null;

        protected override object MakeStaticObject()
        {
            IRootWorkItemService RootWorkItemService = SpringUtil.GetObject<IRootWorkItemService>("RootWorkItemService");

            _service = RootWorkItemService.RootWorkItem.Services.Get<IHeaderService>();
            if (_service == null)
            {
                _service = new HeaderService();
                RootWorkItemService.RootWorkItem.State[CLIENT_INFO_KEY] = base.MakeStaticObject();
                RootWorkItemService.RootWorkItem.Services.Add<IHeaderService>(_service);
                //_service = RootWorkItemService.RootWorkItem.Services.AddNew<HeaderService, IHeaderService>();
            }

            ClientInfo clientInfo = RootWorkItemService.RootWorkItem.State[CLIENT_INFO_KEY] as ClientInfo;
            if (clientInfo == null)
            {
                RootWorkItemService.RootWorkItem.State[CLIENT_INFO_KEY] = clientInfo = base.MakeStaticObject() as ClientInfo;
            }

            return clientInfo;
        }

        public override void ServerHeaderVersionChanged(Version oldVersion, Version newVersion, System.Xml.XmlDocument header)
        {
            if (_service == null) return;
            _service.ServerHeaderVersionChanged(oldVersion, newVersion, header);
        }

        public override void AppendClientHeader(Formular.Support.Extension.HeaderTransfer.ITransferHeader header)
        {
            if (_service == null) return;
            _service.AppendClientHeader(header);
        }
    }
}
