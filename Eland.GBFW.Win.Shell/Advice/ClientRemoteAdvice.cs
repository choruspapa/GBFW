using System;
using System.Collections.Generic;
using System.Text;
using Eland.GBFW.Win.Common.Interface;
using Eland.GBFW.Win.Shell.Services;
using Formular.Support.Extension.HeaderTransfer;
using Eland.GBFW.Common.Util;

namespace Eland.GBFW.Win.Shell.Advice
{
    public class ClientRemoteAdvice : Formular.Support.Extension.HeaderTransfer.Remote.ClientRemoteAdvice
    {
        private IHeaderService _service = null;

        public override void ServerHeaderVersionChanged(Version oldVersion, Version newVersion, System.Xml.XmlDocument header)
        {
            CheckHeaderService();

            if (_service == null) return;
            _service.ServerHeaderVersionChanged(oldVersion, newVersion, header);

        }

        public override void AppendClientHeader(Formular.Support.Extension.HeaderTransfer.ITransferHeader header)
        {
            CheckHeaderService();

            if (_service == null) return;
            _service.AppendClientHeader(header);
        }

        private bool CheckHeaderService()
        {
            if (_service != null) return true;

            IRootWorkItemService RootWorkItemService = SpringUtil.GetObject<IRootWorkItemService>("RootWorkItemService");
            IHeaderService service = RootWorkItemService.RootWorkItem.Services.Get<IHeaderService>();
            if (service == null)
            {
                _service = new HeaderService();
                RootWorkItemService.RootWorkItem.Services.Add<IHeaderService>(_service);
                //_service = RootWorkItemService.RootWorkItem.Services.AddNew<HeaderService, IHeaderService>();
            }

            return true;
        }
    }
}
