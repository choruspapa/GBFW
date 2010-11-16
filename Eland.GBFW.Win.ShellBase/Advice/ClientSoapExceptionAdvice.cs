using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using System.Web.Services.Protocols;
using Eland.GBFW.Common.Exceptions;

namespace Eland.GBFW.Win.ShellBase.Advice
{
    public class ClientSoapExceptionAdvice : Spring.Aop.IThrowsAdvice
    {
        public void AfterThrowing(SoapException ex)
        {
            Exception exception = null;
            try
            {
                exception = TransferException.DeserializeFromMessage(ex.Message);
            }
            catch
            {
                throw ex;
            }

            if (exception == null) throw ex;
            throw exception;
        } 
    }
}
