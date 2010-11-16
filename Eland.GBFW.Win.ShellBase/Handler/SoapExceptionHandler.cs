using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Formular.Win.Cab.Controls.Worker;

namespace Eland.GBFW.Win.Shell.Handler
{
    public class SoapExceptionHandler : Formular.Win.Cab.Shell.ChainHandlers.BaseExceptionHandler
    {
        protected override void HandleAfterAsyncProcess(Formular.Win.Cab.Controls.Worker.AsyncCompleteEventArgs e)
        {
            if (e.Exception != null && e.Exception.InnerException is System.Web.Services.Protocols.SoapException)
            {
                Exception ex = null;

                System.Web.Services.Protocols.SoapException se = e.Exception.InnerException as System.Web.Services.Protocols.SoapException;

                System.Xml.XmlAttribute attrType = null;
                System.Xml.XmlAttribute attrAssembly = null;
                System.Xml.XmlAttribute attrMessage = null;
                System.Xml.XmlAttribute attrStackTrace = null;

                System.Xml.XmlAttribute attrErrorCode = null;
                System.Xml.XmlAttribute attrParameters = null;

                Assembly assembly = null;
                Type type = null;

                int exceptionCount = se.Detail.ChildNodes.Count;
                for (int i = exceptionCount - 1; i >= 0; i--)
                {
                    System.Xml.XmlNode node = se.Detail.ChildNodes[i];

                    attrType = node.Attributes.GetNamedItem("Type") as System.Xml.XmlAttribute;
                    attrAssembly = node.Attributes.GetNamedItem("Assembly") as System.Xml.XmlAttribute;
                    attrMessage = node.Attributes.GetNamedItem("Message") as System.Xml.XmlAttribute;
                    attrStackTrace = node.Attributes.GetNamedItem("StackTrace") as System.Xml.XmlAttribute;

                    assembly = Assembly.Load(attrAssembly.Value);
                    if (assembly != null)
                        type = assembly.GetType(attrType.Value);

                    ex = Activator.CreateInstance(type, attrMessage.Value, ex) as Exception;
                    if (ex is Formular.Common.Exceptions.BaseException)
                    {
                        attrErrorCode = node.Attributes.GetNamedItem("ErrorCode") as System.Xml.XmlAttribute;
                        attrParameters = node.Attributes.GetNamedItem("Parameters") as System.Xml.XmlAttribute;

                        (ex as Formular.Common.Exceptions.BaseException).Code = attrErrorCode.Value;
                        if (!string.IsNullOrEmpty(attrParameters.Value))
                        {
                            string[] parameters = attrParameters.Value.Split(';');
                            (ex as Formular.Common.Exceptions.BaseException).Parameters = parameters;
                        }
                    }
                }
                e.Exception = ex;
            }
            else if (e.Exception != null && e.Exception.InnerException != null)
            {
                e.Exception = e.Exception.InnerException;
            }
        }
    }
}
