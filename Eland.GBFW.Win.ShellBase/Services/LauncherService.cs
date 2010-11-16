using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eland.GBFW.Win.Common.Interface;
using Eland.GBFW.EnvironmentMng.Service;
using Eland.GBFW.Common.Util;
using Formular.Win.Cab;
using Microsoft.Practices.CompositeUI;
using Formular.Win.Cab.Services;
using Eland.GBFW.ProgramMng.Domain;
using Formular.Win.Cab.Constants;

namespace Eland.GBFW.Win.ShellBase.Services
{
    public class LauncherService : ILauncherService
    {

        private IRootWorkItemService rootWorkItemService;
        private ICoordinator coordinator;
        private WorkItem workItem;

        public LauncherService()
        {
            rootWorkItemService = SpringUtil.GetObject<IRootWorkItemService>("RootWorkItemService");
            workItem = rootWorkItemService.RootWorkItem;

            coordinator = workItem.Services.Get<ICoordinator>();
        }

        #region ILauncherService 멤버

        public void Run(IDictionary<string, string> args)
        {
            string loginUserId = "";
            if (workItem.State["LoginId"] != null)
                loginUserId = (string)workItem.State["LoginId"];

            MenuLoad(loginUserId);


            if (args != null)
            {
                if (args.ContainsKey("programid"))
                {
                    int programId = Convert.ToInt32(args["programid"]);
                    string viewName = args.ContainsKey("viewname") ? args["viewname"] : "";
                    string commandName = args.ContainsKey("commandname") ? args["commandname"] : "";

                    ViewLoader(programId, viewName, loginUserId, commandName, new object[] { args });
                }
            }
        }

        private void ViewLoader(int programId, string viewName, string loginId, string commandName, object[] parameters)
        {
            // 프로그램 호출
            Program startprogram = workItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>()
               .GetProgramById(programId) as Program;
            // 프로그램 표시
            coordinator.LoadView(viewName, startprogram.Id, startprogram.ClassName, startprogram.AssemblyName,
                          null, commandName, parameters);
        }

        private void MenuLoad(string loginId)
        {
            IEnvironmentMngService environmentMngService = SpringUtil.GetObject<IEnvironmentMngService>("EnvironmentMngService");
            ICoordinator coordinator = workItem.Services.Get<ICoordinator>();

            environmentMngService.EnvironmentinfoCall();
            string menuProgramString = environmentMngService.GetAppSettingInfoByKey("MenuProgramId");

            int menuProgramId = -1;
            if (string.IsNullOrEmpty(menuProgramString) || !int.TryParse(menuProgramString, out menuProgramId))
            {
                MessageArgs messageArgs = new MessageArgs();
                messageArgs.Message = "There is no information about menu program. Check the server configuration file";
                workItem.Services.Get<IMessageService>().ShowMessage(messageArgs);
                return;
            }

            Program program = workItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>()
                .GetProgramById(menuProgramId) as Program;

            try
            {
                coordinator.LoadView(program.Name, program.Id, program.ClassName, program.AssemblyName,
                              null, "CreateMenu", new object[] { loginId },
                              coordinator.GetSmartPartInfo((SmartPartTypes)Enum.Parse(typeof(SmartPartTypes),
                                                              Enum.GetName(typeof(WorkspaceType),
                                                              program.WorkspaceType))));
            }
            catch (Exception ex)
            {
                MessageArgs messageArgs = new MessageArgs();
                messageArgs.MessageLevel = MessageLevel.Error;
                messageArgs.MessageType = MessageType.Alert;
                messageArgs.Message = ex.Message;
                messageArgs.Details = ex.StackTrace;
                if (ex.InnerException != null) messageArgs.Details += "\n\rInnerMessage : " + ex.InnerException.Message;
                workItem.Services.Get<IMessageService>().ShowMessage(messageArgs);
            }
        }
        #endregion
    }
}
