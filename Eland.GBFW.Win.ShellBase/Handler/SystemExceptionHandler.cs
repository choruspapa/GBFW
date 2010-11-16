using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Formular.Win.Cab.Services;

namespace Eland.GBFW.Win.Shell.Handler
{
    public class SystemExceptionHandler : Formular.Win.Cab.Shell.ChainHandlers.BaseExceptionHandler
    {
        protected override void HandleAfterAsyncProcess(Formular.Win.Cab.Controls.Worker.AsyncCompleteEventArgs e)
        {
            if (e.Exception != null && !(e.Exception is Formular.Common.Exceptions.BaseException))
            {
                Microsoft.Practices.CompositeUI.WorkItem workItem = Eland.GBFW.Common.Util.SpringUtil.GetObject<Eland.GBFW.Win.Common.Interface.IRootWorkItemService>("RootWorkItemService").RootWorkItem;

                Exception ex = e.Exception.InnerException;
                StringBuilder sb = new StringBuilder();
                while (ex != null)
                {
                    sb.Append("Message: " + ex.Message + Environment.NewLine);
                    sb.Append("StackTrace: " + ex.StackTrace + Environment.NewLine);
                    sb.Append("=================================" + Environment.NewLine);
                }

                workItem.Services.Get<IMessageService>().ShowMessage("시스템 오류입니다. 관리자에게 문의하세요", sb.ToString());

            }
        }
    }
}
