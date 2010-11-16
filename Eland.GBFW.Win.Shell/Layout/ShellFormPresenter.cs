using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Formular.Win.Cab;
using Microsoft.Practices.CompositeUI.EventBroker;
using Formular.Win.Cab.Entities;
using Eland.GBFW.ProgramMng.Service;
using Eland.GBFW.ProgramMng.Domain;
using Eland.GBFW.Common.Util;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Eland.GBFW.MenuMng.Domain;

namespace Eland.GBFW.Win.Shell.Layout
{
    public interface IShellForm
    {
        void SearchStartUpToolStrip();
    }

    public class ShellFormPresenter : AbstractPresenter<ShellForm>, IShellForm
    {
        private IList<string> flowingInformationList = null;
        private StringBuilder flowingInformationStrBuilder = null;
        private int informationInterval = 10;
        private int strBuilderCapacity = 500;
        private int strBuilderIndicatingNum = 0;
        private Timer flowingInformationTimer = new Timer();

        public ShellFormPresenter()
        {
            flowingInformationTimer.Tick += new EventHandler(timer_Tick);
        }
        
        [EventSubscription(Eland.GBFW.Win.Common.Constants.EventTopicNames.ChangeShellUserInfoStatusLabel)]
        public void ChangeUserInfoStatusLabel(object sender, EventArgs<string> e)
        {
            if (e.Data != null)
            {
                View.ChangeUserInfoStatusLabel(e.Data);   
            }
        }

        [EventSubscription(Eland.GBFW.Win.Common.Constants.EventTopicNames.ChangeShellMenuStatusLabel)]
        public void ChangeMenuStatusLabel(object sender, EventArgs<string> e)
        {
            if (e.Data != null)
            {
                View.ChangeMenuStatusLabel(e.Data);
            }
        }

        [EventSubscription(Eland.GBFW.Win.Common.Constants.EventTopicNames.FlowInformationToShellUserInfoStatusLabel)]
        public void FlowMessageToUserInfoStatusLabel(object sender, Formular.Win.Cab.Entities.EventArgs<object[]> e)
        {
            if (e.Data != null && e.Data[0] is IList<string> && e.Data[1] is int && e.Data[2] is int && e.Data[3] is int)
            {
                flowingInformationList = e.Data[0] as IList<string>;
                int timeInterval = int.Parse(e.Data[1].ToString());
                informationInterval = int.Parse(e.Data[2].ToString());
                strBuilderCapacity = int.Parse(e.Data[3].ToString());   
               
                flowingInformationStrBuilder = GetFlowingInformationStrBuilder(strBuilderCapacity, flowingInformationList, informationInterval);
                InitializeFlowingInformation();
                flowingInformationTimer.Interval = timeInterval;
                flowingInformationTimer.Start();
            }
        }

        [EventSubscription(Eland.GBFW.Win.Common.Constants.EventTopicNames.ClearFlowingInformationFromShellUserInfoStatusLabel)]
        public void ClearFlowingInformationFromUserInfoStatusLabel(object sender, Formular.Win.Cab.Entities.EventArgs<object> e)
        {
            InitializeFlowingInformation();
        }

        private void InitializeFlowingInformation()
        {            
            flowingInformationTimer.Stop();
            strBuilderIndicatingNum = 0;
            View.ChangeUserInfoStatusLabel("");
        }

        private StringBuilder GetFlowingInformationStrBuilder(int maxCapacity, IList<string> flowingInformationList, int informationInterval)
        {
            StringBuilder strBuilder = new StringBuilder();
            while (strBuilder.Length < maxCapacity)
            {
                foreach (string information in flowingInformationList)
                {
                    if (strBuilder.Length + information.Length >= maxCapacity) return strBuilder;                    
                    strBuilder.Append(information);
                    for (int i = 0; i < informationInterval; i++)
                    {
                        if (strBuilder.Length >= maxCapacity) return strBuilder;
                        strBuilder.Append(" ");
                    }                    
                }
            }
            return strBuilder;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (strBuilderIndicatingNum >= flowingInformationStrBuilder.Length)
            {
                string lotatingChar = flowingInformationStrBuilder.ToString().Substring(0, 1);
                flowingInformationStrBuilder.Remove(0, 1);
                flowingInformationStrBuilder.Append(lotatingChar);
            }
            else strBuilderIndicatingNum++;
            if (flowingInformationStrBuilder.Length >= strBuilderIndicatingNum)
            {
                string showingInfomation = flowingInformationStrBuilder.ToString().Substring(0, strBuilderIndicatingNum);
                View.ChangeUserInfoStatusLabel(showingInfomation);
            }
        }

        public void SearchStartUpToolStrip()
        {
            List<Operation> operationTemp = new List<Operation>();
            List<Operation> operations = new List<Operation>();
            IOperationMngService operationMngService = SpringUtil.GetObject<IOperationMngService>("OperationMngService");
            
            operations = operationMngService.SearchStartUpToolStrip();

            View.operation = operations;
        }

        public override void OnInitialize(IView sender, EventArgs e) { }

        public void ShotKeyMenuExecute(string menuId)
        {
            Coordinator.FireEvent<string>(Eland.GBFW.Win.Common.Constants.EventTopicNames.ShotKeyMenuExecute, menuId);
        }

        /// <summary>
        /// 즐겨찾기 추가
        /// </summary>
        public void AddFavourite()
        {
            IWorkspace contentWorkspace = (IWorkspace)this.WorkItem.RootWorkItem.Workspaces[Formular.Win.Cab.Constants.WorkspaceNames.ContentPart];
            IView activeSmartPart = (IView)contentWorkspace.ActiveSmartPart;

            MenuDto menus = WorkItem.RootWorkItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>().GetProgramInfo(activeSmartPart.ProgramID) as MenuDto;

            if (menus == null) return;
            string programId = menus.ProgramId.ToString().Trim();
            //string programName = menus.Name.Trim();
            string programName = ((object[])activeSmartPart.ExtraData)[0].ToString();
            string program = programId + "|#" + programName;
            Coordinator.FireEvent<string>(Eland.GBFW.Win.Common.Constants.EventTopicNames.AddFavourite, program);
        }

    }
}
