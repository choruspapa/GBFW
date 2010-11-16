using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eland.GBFW.Win.Common.Interface;
using Eland.GBFW.Common.Util;
using Formular.Win.Cab;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.Commands;
using Eland.GBFW.ProgramMng.Domain;
using System.Resources;
using System.Drawing;
using Microsoft.Practices.CompositeUI;
using Formular.Win.Cab.Services;
using Eland.GBFW.Win.Common;
using Formular.Win.Cab.Constants;
using Formular.Win.Cab.Shell.Services;
using Formular.Win.Cab.Workspaces;

namespace Eland.GBFW.Win.ShellBase.Services
{
    public class CommandButtonService : ICommandButtonService
    {
        protected static Type RESOURCE_TYPE = Type.GetType("Eland.GBFW.Win.Shell.Properties.Resources, Eland.GBFW.Win.Shell");

        [ServiceDependency]
        public WorkItem WorkItem { get; set; }

        [ServiceDependency]
        public ICoordinator Coordinator { get; set; }

        [ServiceDependency]
        public ICurrentWorkingService CurrentWorkingService { get; set; }

        Eland.GBFW.Win.Common.Interface.IRoleShellService _roleShellService = null;
        public Eland.GBFW.Win.Common.Interface.IRoleShellService RoleShellService
        {
            get
            {
                if (_roleShellService == null) _roleShellService = WorkItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>();
                return _roleShellService;
            }
        }

        /// <summary>
        /// 공통 버튼 상태 변경
        /// </summary>
        /// <param name="programCode"></param>
        /// <param name="isDoWorkBusy"></param>
        public void ChangeCommandButtonListState(int programId, bool isBusy)
        {
            IDictionary<string, CommandStatus> commandButtonList = this.RoleShellService.GetCommandButtonList(programId);
            this.CurrentWorkingService.ActiveView.AvailButtonList = commandButtonList;

            ChangeCommandButtonListState(commandButtonList, isBusy);
        }

        /// <summary>
        /// 공통 버튼 상태 변경
        /// </summary>
        /// <param name="programControls"></param>
        /// <param name="isDoWorkBusy"></param>
        private void ChangeCommandButtonListState(IDictionary<string, CommandStatus> commandButtonList, bool isBusy)
        {
            if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.ContentPart)
            {
                this.Coordinator.ShellImplementor.BaseShell.EnabledToolStripPanel = !isBusy;
                SetToolStripVisible(false);
            }
            IEnumerator<KeyValuePair<string, Command>> enumerator = this.WorkItem.RootWorkItem.Commands.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!commandButtonList.ContainsKey(enumerator.Current.Key))
                    this.ChangeCommandButtonState(enumerator.Current.Key, CommandStatus.Unavailable);
                else if (isBusy)
                    this.ChangeCommandButtonState(enumerator.Current.Key, CommandStatus.Disabled);
                else
                    this.ChangeCommandButtonState(enumerator.Current.Key, commandButtonList[enumerator.Current.Key]);
            }
            if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.ContentPart)
            {
                SetToolStripVisible(true);
            }
        }

        private void SetToolStripVisible(bool visible)
        {
            Control stripControl = null;
            try
            {
                stripControl = ((Control)this.Coordinator.ShellImplementor.BaseShell).Controls["MainToolStripPanel"].Controls["Extension"];
                stripControl.Visible = visible;
            }
            catch { }
            try
            {
                stripControl = ((Control)this.Coordinator.ShellImplementor.BaseShell).Controls["MainToolStripPanel"].Controls["Change"];
                stripControl.Visible = visible;
            }
            catch { }
        }

        /// <summary>
        /// 공통 버튼 상태 변경
        /// </summary>
        /// <param name="buttonName">버튼 이름</param>
        /// <param name="status">상태</param>
        public void ChangeCommandButtonState(string commandName, CommandStatus status)
        {
            if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.ContentPart)
                WorkItem.RootWorkItem.Commands[commandName].Status = status;
            else if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.PopupPart)
            {
                if (CurrentWorkingService.ActiveView is AbstractPopupHostView)
                    ((AbstractPopupHostView)(CurrentWorkingService.ActiveView)).ChangeCommandButtonState(commandName, status);
            }
        }

        public IDictionary<string, CommandStatus> GetCommandButtonList(int programId)
        {
            return this.RoleShellService.GetCommandButtonList(programId);
        }


        #region ICommandButtonService Members
        /*
        public virtual IDictionary<string, CommandStatus> GetCommandButtonList(int programId)
        {
            if (this.WorkItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>() == null)
            {
                return null;
            }
            return this.WorkItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>().GetCommandButtonList(programId);
        }

        public virtual void ChangeCommandButtonState(string commandName, CommandStatus status)
        {
            coordinator.ChangeCommandButtonState(commandName, status);
        }

        */
        #endregion

        public virtual void InitPopupCommandButtonLayout(
            ToolStripPanel toolStripPanel,
            IView hostedView,
            Size btnSize,
            bool isTextShown,
            ContentAlignment txtAlign,
            TextImageRelation txtImgRelation,
            Font font,
            IResourceService resourceService,
            EventHandler buttonClickHandler)
        {
            if (hostedView == null)
                return;

            foreach (Control control in toolStripPanel.Controls)
            {
                ToolStrip toolStrip = control as ToolStrip;
                if (toolStrip != null)
                {
                    toolStrip.ImageScalingSize = btnSize;

                    IDictionary<string, CommandStatus> commandButtonList = GetCommandButtonList(hostedView.ProgramID);
                    IEnumerator<KeyValuePair<string, CommandStatus>> enumerator = commandButtonList.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ToolStripButton toolStripButton = new ToolStripButton();
                        string commandKey = enumerator.Current.Key;
                        toolStripButton.Image = GetIcon("img" + commandKey, RESOURCE_TYPE);
                        toolStripButton.Name = commandKey;
                        toolStripButton.ToolTipText = commandKey;
                        toolStripButton.Click += buttonClickHandler;
                        toolStrip.Items.Add(toolStripButton);
                    }

                    toolStripPanel.Controls.Add(toolStrip);
                    hostedView.AvailButtonList = commandButtonList;
                }
            }
        }

        public virtual void InitShellCommandButtonLayout(
            System.Windows.Forms.ToolStripPanel toolStripPanel,
            object operationList,
            System.Drawing.Size btnSize,
            bool isTextShown,
            System.Drawing.ContentAlignment txtAlign,
            System.Windows.Forms.TextImageRelation txtImgRelation,
            System.Drawing.Font font,
            IResourceService resourceService)
        {
            IList<Operation> opList = operationList as IList<Operation>;
            if (opList == null) return;
            foreach (Operation op in opList)
            {
                foreach (Control control in toolStripPanel.Controls)
                {
                    ToolStrip toolStrip = control as ToolStrip;

                    if (toolStrip != null)
                    {
                        toolStrip.ImageScalingSize = btnSize;
                        if (toolStrip.Name == op.LocationType.ToString())
                        {
                            ToolStripItem toolStripItem = null;

                            switch (op.OperationType)
                            {
                                case OperationType.Button:
                                    toolStripItem = SetToolStripItem(new ToolStripButton(), op, resourceService, 0, isTextShown, txtAlign, txtImgRelation, font);
                                    break;
                                case OperationType.Separator:
                                    toolStripItem = SetToolStripItem(new ToolStripSeparator(), op, resourceService, 0, isTextShown, txtAlign, txtImgRelation, font);
                                    break;
                                case OperationType.SplitButton:
                                    toolStripItem = SetToolStripItem(new ToolStripSplitButton(), op, resourceService, 1, isTextShown, txtAlign, txtImgRelation, font);
                                    break;
                                default:
                                    break;
                            }

                            if (this.WorkItem.UIExtensionSites.Contains(op.LocationType.ToString()))
                            {
                                this.WorkItem.UIExtensionSites[op.LocationType.ToString()].Add<ToolStripItem>(toolStripItem);
                            }

                            switch (op.OperationType)
                            {
                                case OperationType.Button:
                                    this.WorkItem.Commands[op.Name].AddInvoker(toolStripItem, "Click");
                                    break;
                                case OperationType.SplitButton:
                                    this.WorkItem.Commands[op.Name].AddInvoker(toolStripItem, "ButtonClick");
                                    ToolStripSplitButtonDropDownItemClickedAdapter adapter = new ToolStripSplitButtonDropDownItemClickedAdapter(toolStripItem as ToolStripSplitButton);
                                    adapter.Tag = 1;
                                    this.WorkItem.Commands[op.Name].AddCommandAdapter(adapter);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private ToolStripItem SetToolStripItem(ToolStripItem toolStripItem, Operation operation,
            IResourceService resourceService, int rowCount, bool isTextShown, ContentAlignment txtAlign, TextImageRelation txtImgRelation, Font font)
        {
            string toolStripResourceName = string.Empty;

            if (toolStripItem is ToolStripSplitButton)
            {
                toolStripResourceName = string.Format(resourceService.GetResourceString(operation.Name), rowCount);
                toolStripItem.Tag = operation.Name + '#' + operation.Detail;
            }
            else
            {
                toolStripResourceName = resourceService.GetResourceString(operation.Name);
                toolStripItem.Tag = operation.Name;
            }

            toolStripItem.ToolTipText = toolStripResourceName;
            toolStripItem.Name = operation.Name;
            toolStripItem.Image = GetIcon(operation.NormalResId, RESOURCE_TYPE); //
            if (isTextShown)
            {
                toolStripItem.Text = toolStripResourceName;
                toolStripItem.TextAlign = txtAlign;
                toolStripItem.TextImageRelation = txtImgRelation;
                toolStripItem.Font = font;
            }
            return toolStripItem;
        }

        private static System.Drawing.Image GetIcon(string iconName, Type resourceType)
        {
            ResourceManager resourceMan = new ResourceManager(resourceType);
            object obj = resourceMan.GetObject(iconName);
            if (obj is Icon)
                return ((Icon)obj).ToBitmap();
            return (Bitmap)obj;
        }
    }
}


#region Save CommandButton Status Logic
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eland.GBFW.Win.Common.Interface;
using Eland.GBFW.Common.Util;
using Formular.Win.Cab;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.Commands;
using Eland.GBFW.ProgramMng.Domain;
using System.Resources;
using System.Drawing;
using Microsoft.Practices.CompositeUI;
using Formular.Win.Cab.Services;
using Eland.GBFW.Win.Common;
using Formular.Win.Cab.Constants;
using Formular.Win.Cab.Shell.Services;
using Formular.Win.Cab.Workspaces;

namespace Eland.GBFW.Win.ShellBase.Services
{
    public class CommandButtonService : ICommandButtonService
    {
        protected static Type RESOURCE_TYPE = Type.GetType("Eland.GBFW.Win.Shell.Properties.Resources, Eland.GBFW.Win.Shell");
        protected IDictionary<int, IDictionary<string, CommandStatus>> _commandStatusListDic = new Dictionary<int, IDictionary<string, CommandStatus>>();
        [ServiceDependency]
        public WorkItem WorkItem { get; set; }

        [ServiceDependency]
        public ICoordinator Coordinator { get; set; }

        [ServiceDependency]
        public ICurrentWorkingService CurrentWorkingService { get; set; }

        Eland.GBFW.Win.Common.Interface.IRoleShellService _roleShellService = null;
        public Eland.GBFW.Win.Common.Interface.IRoleShellService RoleShellService
        {
            get
            {
                if (_roleShellService == null) _roleShellService = WorkItem.Services.Get<Eland.GBFW.Win.Common.Interface.IRoleShellService>();
                return _roleShellService;
            }
        }

        /// <summary>
        /// 공통 버튼 상태 변경
        /// </summary>
        /// <param name="programCode"></param>
        /// <param name="isDoWorkBusy"></param>
        public void ChangeCommandButtonListState(int programId, bool isBusy)
        {
            IDictionary<string, CommandStatus> commandButtonList = null;
            if (_commandStatusListDic.ContainsKey(programId)) commandButtonList = _commandStatusListDic[programId];
            else
            {
                commandButtonList = this.RoleShellService.GetCommandButtonList(programId);
                _commandStatusListDic.Add(programId, commandButtonList);
            }
            this.CurrentWorkingService.ActiveView.AvailButtonList = commandButtonList;

            ChangeCommandButtonListState(commandButtonList, isBusy);
        }

        /// <summary>
        /// 공통 버튼 상태 변경
        /// </summary>
        /// <param name="programControls"></param>
        /// <param name="isDoWorkBusy"></param>
        private void ChangeCommandButtonListState(IDictionary<string, CommandStatus> commandButtonList, bool isBusy)
        {
            if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.ContentPart)
            {
                this.Coordinator.ShellImplementor.BaseShell.EnabledToolStripPanel = !isBusy;
                SetToolStripVisible(false);
            }
            IEnumerator<KeyValuePair<string, Command>> enumerator = this.WorkItem.RootWorkItem.Commands.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!commandButtonList.ContainsKey(enumerator.Current.Key))
                    this.ChangeCommandButtonStateWithoutSave(enumerator.Current.Key, CommandStatus.Unavailable);
                else if (isBusy)
                    this.ChangeCommandButtonStateWithoutSave(enumerator.Current.Key, CommandStatus.Disabled);
                else
                    this.ChangeCommandButtonStateWithoutSave(enumerator.Current.Key, commandButtonList[enumerator.Current.Key]);
            }
            if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.ContentPart)
            {
                SetToolStripVisible(true);
            }
        }

        private void SetToolStripVisible(bool visible)
        {
            Control stripControl = null;
            try
            {
                stripControl = ((Control)this.Coordinator.ShellImplementor.BaseShell).Controls["MainToolStripPanel"].Controls["Extension"];
                stripControl.Visible = visible;
            }
            catch { }
            try
            {
                stripControl = ((Control)this.Coordinator.ShellImplementor.BaseShell).Controls["MainToolStripPanel"].Controls["Change"];
                stripControl.Visible = visible;
            }
            catch { }
        }

        public void ChangeCommandButtonStateWithoutSave(string commandName, CommandStatus status)
        {
            if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.ContentPart)
                WorkItem.RootWorkItem.Commands[commandName].Status = status;
            else if (CurrentWorkingService.CurrentWorkspaceName == WorkspaceNames.PopupPart)
            {
                if (CurrentWorkingService.ActiveView is AbstractPopupHostView)
                    ((AbstractPopupHostView)(CurrentWorkingService.ActiveView)).ChangeCommandButtonState(commandName, status);
            }
        }

        /// <summary>
        /// 공통 버튼 상태 변경
        /// </summary>
        /// <param name="buttonName">버튼 이름</param>
        /// <param name="status">상태</param>
        public void ChangeCommandButtonState(string commandName, CommandStatus status)
        {
            this.ChangeCommandButtonStateWithoutSave(commandName, status);
            int programId = this.CurrentWorkingService.ActiveView.ProgramID;
            if (_commandStatusListDic.ContainsKey(programId))
            {
                try
                {
                    if (_commandStatusListDic[programId].ContainsKey(commandName))
                    {
                        _commandStatusListDic[programId][commandName] = status;
                    }
                    else
                    {
                        _commandStatusListDic[programId].Add(commandName, status);
                    }
                }
                catch { }
            }
        }

        public IDictionary<string, CommandStatus> GetCommandButtonList(int programId)
        {
            return this.RoleShellService.GetCommandButtonList(programId);
        }


        public virtual void InitPopupCommandButtonLayout(
            ToolStripPanel toolStripPanel,
            IView hostedView,
            Size btnSize,
            bool isTextShown,
            ContentAlignment txtAlign,
            TextImageRelation txtImgRelation,
            Font font,
            IResourceService resourceService,
            EventHandler buttonClickHandler)
        {
            if (hostedView == null)
                return;

            foreach (Control control in toolStripPanel.Controls)
            {
                ToolStrip toolStrip = control as ToolStrip;
                if (toolStrip != null)
                {
                    toolStrip.ImageScalingSize = btnSize;

                    IDictionary<string, CommandStatus> commandButtonList = GetCommandButtonList(hostedView.ProgramID);
                    IEnumerator<KeyValuePair<string, CommandStatus>> enumerator = commandButtonList.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ToolStripButton toolStripButton = new ToolStripButton();
                        string commandKey = enumerator.Current.Key;
                        toolStripButton.Image = GetIcon("img" + commandKey, RESOURCE_TYPE);
                        toolStripButton.Name = commandKey;
                        toolStripButton.ToolTipText = commandKey;
                        toolStripButton.Click += buttonClickHandler;
                        toolStrip.Items.Add(toolStripButton);
                    }

                    toolStripPanel.Controls.Add(toolStrip);
                    hostedView.AvailButtonList = commandButtonList;
                }
            }
        }

        public virtual void InitShellCommandButtonLayout(
            System.Windows.Forms.ToolStripPanel toolStripPanel,
            object operationList,
            System.Drawing.Size btnSize,
            bool isTextShown,
            System.Drawing.ContentAlignment txtAlign,
            System.Windows.Forms.TextImageRelation txtImgRelation,
            System.Drawing.Font font,
            IResourceService resourceService)
        {
            IList<Operation> opList = operationList as IList<Operation>;
            if (opList == null) return;
            foreach (Operation op in opList)
            {
                foreach (Control control in toolStripPanel.Controls)
                {
                    ToolStrip toolStrip = control as ToolStrip;

                    if (toolStrip != null)
                    {
                        toolStrip.ImageScalingSize = btnSize;
                        if (toolStrip.Name == op.LocationType.ToString())
                        {
                            ToolStripItem toolStripItem = null;

                            switch (op.OperationType)
                            {
                                case OperationType.Button:
                                    toolStripItem = SetToolStripItem(new ToolStripButton(), op, resourceService, 0, isTextShown, txtAlign, txtImgRelation, font);
                                    break;
                                case OperationType.Separator:
                                    toolStripItem = SetToolStripItem(new ToolStripSeparator(), op, resourceService, 0, isTextShown, txtAlign, txtImgRelation, font);
                                    break;
                                case OperationType.SplitButton:
                                    toolStripItem = SetToolStripItem(new ToolStripSplitButton(), op, resourceService, 1, isTextShown, txtAlign, txtImgRelation, font);
                                    break;
                                default:
                                    break;
                            }

                            if (this.WorkItem.UIExtensionSites.Contains(op.LocationType.ToString()))
                            {
                                this.WorkItem.UIExtensionSites[op.LocationType.ToString()].Add<ToolStripItem>(toolStripItem);
                            }

                            switch (op.OperationType)
                            {
                                case OperationType.Button:
                                    this.WorkItem.Commands[op.Name].AddInvoker(toolStripItem, "Click");
                                    break;
                                case OperationType.SplitButton:
                                    this.WorkItem.Commands[op.Name].AddInvoker(toolStripItem, "ButtonClick");
                                    ToolStripSplitButtonDropDownItemClickedAdapter adapter = new ToolStripSplitButtonDropDownItemClickedAdapter(toolStripItem as ToolStripSplitButton);
                                    adapter.Tag = 1;
                                    this.WorkItem.Commands[op.Name].AddCommandAdapter(adapter);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private ToolStripItem SetToolStripItem(ToolStripItem toolStripItem, Operation operation,
            IResourceService resourceService, int rowCount, bool isTextShown, ContentAlignment txtAlign, TextImageRelation txtImgRelation, Font font)
        {
            string toolStripResourceName = string.Empty;

            if (toolStripItem is ToolStripSplitButton)
            {
                toolStripResourceName = string.Format(resourceService.GetResourceString(operation.Name), rowCount);
                toolStripItem.Tag = operation.Name + '#' + operation.Detail;
            }
            else
            {
                toolStripResourceName = resourceService.GetResourceString(operation.Name);
                toolStripItem.Tag = operation.Name;
            }

            toolStripItem.ToolTipText = toolStripResourceName;
            toolStripItem.Name = operation.Name;
            toolStripItem.Image = GetIcon(operation.NormalResId, RESOURCE_TYPE); //
            if (isTextShown)
            {
                toolStripItem.Text = toolStripResourceName;
                toolStripItem.TextAlign = txtAlign;
                toolStripItem.TextImageRelation = txtImgRelation;
                toolStripItem.Font = font;
            }
            return toolStripItem;
        }

        private static System.Drawing.Image GetIcon(string iconName, Type resourceType)
        {
            ResourceManager resourceMan = new ResourceManager(resourceType);
            object obj = resourceMan.GetObject(iconName);
            if (obj is Icon)
                return ((Icon)obj).ToBitmap();
            return (Bitmap)obj;
        }

    }
}
*/
#endregion
