using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Formular.Win.Cab.Shell.UIElements;
using System.Xml;
using System.IO;
using Microsoft.Practices.ObjectBuilder;
using Eland.GBFW.ProgramMng.Domain;
using Eland.GBFW.Win.Common.Interface;
using Microsoft.Practices.CompositeUI;
using Formular.Win.Cab.Workspaces;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Drawing.Drawing2D;
using Formular.Win.Cab.Controls.Docking;
using Eland.GBFW.Win.Shell.Properties;
using Eland.GBFW.Win.Shell;
using Formular.Win.Cab.Services;
using System.Resources;
using Eland.GBFW.Common.Util.Domain;
using Eland.GBFW.Win.Common;
using DevExpress.LookAndFeel;
using System.Configuration;

namespace Eland.GBFW.Win.Shell.Layout
{
    public partial class ShellForm : DefaultShellForm
    {
        public List<Operation> operation = new List<Operation>();

        private ICommandButtonService _commandButtonService;
        private IResourceService _resourceService;
        DevExpress.Skins.Skin currentSkin;

        public ShellForm()
        {
            InitializeComponent();
            this.EnabledToolStripPanel = false;
            this.Shown += new EventHandler(ShellForm_Shown);
        }

        private ShellFormPresenter presenter;
        /// <summary>
        /// StartShellFOrmPresenter Injection.
        /// </summary>
        [Dependency]
        public ShellFormPresenter Presenter
        {
            set
            {
                presenter = value;
            }
        }

        public void Initialize()
        {
            _resourceService = presenter.WorkItem.Services.Get<IResourceService>();
            _commandButtonService = presenter.WorkItem.Services.Get<ICommandButtonService>();

            this.MinimumSize = new System.Drawing.Size(1024, 768);
            //this.Width = 1024;
            //this.Height = 768;
            //this.Text = "E.LAND ERP SYSTEM";
            //PictureBox wallpaper = new PictureBox();
            //wallpaper.Dock = DockStyle.Fill;
            //this.ContentPart.Controls.Add(wallpaper);

            //IResourceService resourceService = presenter.WorkItem.Services.Get<IResourceService>();
            this.FavoriteToolStripButton.Text = _resourceService.GetResourceString("AddFavorite");
            this.FavoriteToolStripButton.Image = Properties.Resources.imgAddFavorite;
        }

        public override void OnLoadView(object sender, Formular.Win.Cab.Entities.ViewLoadEventArgs e)
        {
            base.OnLoadView(sender, e);

        }
        private void CreateCommonButton()
        {
            //XmlTextReader textReader = new XmlTextReader(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("Eland.GBFW.Win.Startup.Config.MainMenuCatalog.xml"));

            //string strXML = "";

            //XmlDocument doc = new XmlDocument();
            //doc.Load(textReader);
            //if (doc != null)
            //{
            //    StringWriter writer = new StringWriter();
            //    doc.Save(writer);
            //    strXML = writer.ToString();
            //}
            //UIElementBuilder.LoadFromConfig(this.presenter.WorkItem, this.MainToolStripPanel, doc);

            presenter.SearchStartUpToolStrip();
            ProcessConfigItem(this.presenter.WorkItem, this.MainToolStripPanel, operation);

            //UIElementBuilder.ProcessConfigItem(this.presenter.WorkItem, this.MainToolStripPanel, operation);

            IResourceService resourceService = presenter.WorkItem.Services.Get<IResourceService>();
            foreach (Control control in this.MainToolStripPanel.Controls)
            {
                ToolStrip toolstrip = control as ToolStrip;
                if (toolstrip == null) continue;

                for (int i = 0; i < toolstrip.Items.Count; i++)
                {
                    ToolStripItem item = toolstrip.Items[i] as ToolStripItem;
                    if (item != null && item.Tag != null)
                    {
                        if (item is ToolStripSplitButton)
                        {
                            // ToolStripSplitButton 인경우 : {0} 행추가 (동적으로 행추가)
                            string[] splitButtonName = ((string)item.Tag).Split('#');
                            if (splitButtonName.Length > 0)
                                item.Text = string.Format(resourceService.GetResourceString(splitButtonName[0]), 1);
                            AddMultiRowButton(item, resourceService);
                        }
                        else
                            item.Text = resourceService.GetResourceString((string)item.Tag);
                    }
                }
            }
        }

        public void ChangeUserInfoStatusLabel(string message)
        {
            UserInfoToolStripStatusLabel.Text = message;
        }

        public void ChangeMenuStatusLabel(string message)
        {
            MenuToolStripStatusLabel.Text = message;
        }

        public void DisplayDownloadingFileStatus(bool isDownloadingFile)
        {
            lblDownloading.BringToFront();
            lblDownloading.Visible = isDownloadingFile;
            this.Enabled = !isDownloadingFile;
        }

        public void SetContentWorkspaceEventHandler(IWorkspace workspace)
        {
            DockPanelWorkspace dockPanelWorkspace = workspace as DockPanelWorkspace;

            workspace.SmartPartActivated += new EventHandler<WorkspaceEventArgs>(workspace_SmartPartActivated);

            dockPanelWorkspace.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.White;
            dockPanelWorkspace.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.DarkGray;//Color.SteelBlue;
            dockPanelWorkspace.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.LinearGradientMode = LinearGradientMode.Vertical;
        }

        void workspace_SmartPartActivated(object sender, WorkspaceEventArgs e)
        {
            Color? BeforeColor = null;
            Color? AfterColor = null;

            DockPanelWorkspace dockPanelWorkspace = (DockPanelWorkspace)sender;

            if (dockPanelWorkspace == null) return;
            if (dockPanelWorkspace.ActivePane == null) return;

            dockPanelWorkspace.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.LinearGradientMode = LinearGradientMode.Vertical;

            if (dockPanelWorkspace.ActivePane.DockState == DockState.DockLeft)
            {
                if (dockPanelWorkspace.ActiveDocumentPane == null) return;
                BeforeColor = dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor;
                dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = Color.White;
                dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = Color.White;
                dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.White;
                dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.White;
            }
            else
            {
                BeforeColor = dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor;

                DevExpress.Skins.SkinElement skinElement;
                string elementName;

                //currentSkin = DevExpress.Skins.EditorsSkins.GetSkin(UserLookAndFeel.Default);
                //elementName = DevExpress.Skins.EditorsSkins.SkinCheckBox;
                currentSkin = DevExpress.Skins.CommonSkins.GetSkin(UserLookAndFeel.Default);
                elementName = DevExpress.Skins.CommonSkins.SkinForm;
                skinElement = currentSkin[elementName];

                Color skinFormBackColor = skinElement.Color.BackColor;

                if (dockPanelWorkspace.ActiveSmartPart is Eland.GBFW.Win.Common.View.GBFWBaseView)
                    ((Eland.GBFW.Win.Common.View.GBFWBaseView)(dockPanelWorkspace.ActiveSmartPart)).BackColor = skinFormBackColor;

                Color skinDockStripGradientStart = currentSkin.Properties.GetColor("DockStripGradientStart");
                Color skinDockStripGradientEnd = currentSkin.Properties.GetColor("DockStripGradientEnd");
                Color skinActiveTabGradientStart = currentSkin.Properties.GetColor("ActiveTabGradientStart");
                Color skinActiveTabGradientEnd = currentSkin.Properties.GetColor("ActiveTabGradientEnd");
                Color skinInactiveTabGradientStart = currentSkin.Properties.GetColor("InactiveTabGradientStart");
                Color skinInactiveTabGradientEnd = currentSkin.Properties.GetColor("InactiveTabGradientEnd");

                if (skinDockStripGradientStart.IsEmpty)
                {
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = Color.PowderBlue;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = Color.PowderBlue;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.FromArgb(250, 230, 130);
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.FromArgb(248, 244, 229);
                }
                else
                {
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = skinDockStripGradientStart;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = skinDockStripGradientEnd;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = skinActiveTabGradientStart;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = skinActiveTabGradientEnd;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = skinInactiveTabGradientStart;
                    dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = skinInactiveTabGradientEnd;
                }
                //if (dockPanelWorkspace.ActiveSmartPart is Eland.GBFW.Win.Common.View.GBFWBaseView)
                //    ((Eland.GBFW.Win.Common.View.GBFWBaseView)(dockPanelWorkspace.ActiveSmartPart)).BackColor = userControlBackColor;
            }

            // 스킨 적용시 WinForm BackColor 설정
            //UserLookAndFeel lookAndFeel = UserLookAndFeel.Default;
            //Skin currentSkin = CommonSkins.GetSkin(lookAndFeel);
            //Color userControlBackColor = currentSkin.Properties.GetColor("userControlBackColor");
            //dockPanelWorkspace.ActivePane.DockPanel.BackColor = Color.Red;



            AfterColor = dockPanelWorkspace.ActivePane.DockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor;

            if (BeforeColor == AfterColor) return;

            foreach (DockPane pane in dockPanelWorkspace.Panes)
            {
                if (pane.DockState != DockState.DockLeft)
                    pane.Invalidate(true);
            }
        }

        public void ProcessConfigItem(WorkItem workItem, ToolStripPanel toolStripPanel, List<Operation> operation)
        {
            IResourceService _resourceService = workItem.Services.Get<IResourceService>();
            if (operation == null) return;
            MenuItemElement menuItem = new MenuItemElement();

            Dictionary<string, string> locationList = new Dictionary<string, string>();
            foreach (object enumEntry in Enum.GetValues(typeof(LocationType)))
            {
                locationList.Add(DomainHelper.GetCode<LocationType>((LocationType)enumEntry), enumEntry.ToString());
            }

            var locationListSort = from dic in locationList
                                   orderby dic.Key descending
                                   select dic;

            Dictionary<string, string> enumList = locationListSort.ToDictionary(lt => lt.Key, lt => lt.Value);

            //this.MainToolStripPanel.BackgroundImage = ResourceManager. GetIcon("ICON_bar");
            //this.toolStrip1.BackgroundImage = GetIcon("ICON_bar");

            // 고정도구모음, 가변도구모음 ToolStrip 생성
            foreach (KeyValuePair<string, string> enumEntry in enumList)
            {
                string locationType = enumEntry.Value;
                if (locationType == LocationType.Screen.ToString()) continue;

                ToolStrip toolStrip = menuItem.ToToolStrip();
                //ToolStrip toolStrip = new ToolStrip();

                //toolStrip.Size = new System.Drawing.Size(442, 25);
                toolStrip.Text = locationType;
                toolStrip.Name = locationType;
                toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
                toolStrip.MinimumSize = new Size(0, 40);
                //toolStrip.BackgroundImage = GetIcon("ICON_bar");
                toolStripPanel.Join(toolStrip);

                workItem.UIExtensionSites.RegisterSite(locationType, toolStrip);
                workItem.RootWorkItem.Items.Add(toolStrip, locationType);
            }

            _commandButtonService.InitShellCommandButtonLayout(toolStripPanel, operation, new Size(32, 32), true,
               ContentAlignment.MiddleLeft, TextImageRelation.ImageBeforeText, new Font("Tahoma", 8.25F, FontStyle.Regular), _resourceService);
                      
        }



        private void FavoriteToolStripButton_Click(object sender, EventArgs e)
        {
            presenter.AddFavourite();
        }

        /// <summary>
        /// ActionWatcher Set.
        /// </summary>
        public void UIActionWatcherSetting()
        {
            double warningMinutes = -1;
            double maxMinutesIdle = -1;

            //double.TryParse(presenter.WorkItem.State["WarningMinutes"], out warningMinutes);
            //double.TryParse(presenter.WorkItem.State["MaxMinutesIdle"], out maxMinutesIdle);

            //aw.WarningMinutes = warningMinutes;
            //aw.MaxMinutesIdle = maxMinutesIdle;
            //aw.Idle += new EventHandler(ActionWatcher_Idle);

            //actionWatcherBindingSource.DataSource = aw;

        }

        private void cboSkinList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeof(DefaultShellXtraForm).IsAssignableFrom(this.GetType()) == true)
            {
                this.ApplicationMenuPart.Visible = false;
            }
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = cboSkinList.SelectedItem as string;
            this.ContentPart.BackColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["Control"];
            this.ContentPart.BorderStyle = BorderStyle.FixedSingle;
            this.MainToolStripPanel.BackColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["Control"];
            this.MainToolStripPanel.ForeColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["ControlText"];
            //TreeView treeView = this.ContentPart.Controls[0].Controls[3].Controls[1].Controls[2].Controls[0].Controls[0].Controls[1].Controls[2].Controls[0].Controls[0].Controls[0] as TreeView;
            //if (treeView != null)
            //{
            //    treeView.BackColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["Control"];
            //    treeView.ForeColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["ControlText"];
            //}
            foreach (Control control in MainToolStripPanel.Controls)
            {
                control.BackColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["Control"];
                control.ForeColor = DevExpress.Skins.SkinManager.Default.Skins[DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName].CommonSkin.Colors["ControlText"];
            }

            currentSkin = DevExpress.Skins.CommonSkins.GetSkin(UserLookAndFeel.Default);
            Color mainStatusBarBackColor = currentSkin.Properties.GetColor("MainStatusBarBackColor");

            if (!mainStatusBarBackColor.IsEmpty)
                this.MainStatusBar.BackColor = mainStatusBarBackColor;

            // ToolStrip 배경이미지 적용
            Image toolStripBackgroundImage = null;

            //if (currentSkin.Name == "GBFWSkinGreen")
            //    toolStripBackgroundImage = Properties.Resources.ICON_bar1;
            //else if (currentSkin.Name == "GBFWSkinBlue")
            //    toolStripBackgroundImage = Properties.Resources.ICON_bar2;

            toolStripBackgroundImage = Properties.Resources.ICON_bar;

            ToolStrip toolStripFix = (ToolStrip)this.Controls.Find("Fix", true)[0];
            ToolStrip toolStripChange = (ToolStrip)this.Controls.Find("Change", true)[0];
            
            this.MainToolStripPanel.BackgroundImage = toolStripBackgroundImage;
            this.Extension.BackgroundImage = toolStripBackgroundImage;
            toolStripFix.BackgroundImage = toolStripBackgroundImage;
            toolStripChange.BackgroundImage = toolStripBackgroundImage;
            
        }

        private void ShellForm_Load(object sender, EventArgs e)
        {
            CreateCommonButton();

            if (ConfigurationSettings.AppSettings["MenuLocation"].ToUpper().IndexOf("TOP") < 0)
            {
                this.ApplicationMenuPart.Visible = false;
            }

            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.GBFWSkin).Assembly); //Register!

            List<DevExpress.Skins.SkinContainer> skinList = new List<DevExpress.Skins.SkinContainer>();
            foreach (DevExpress.Skins.SkinContainer skinContainer in DevExpress.Skins.SkinManager.Default.Skins)
            {
                //if (!skinContainer.IsEmbedded)
                    cboSkinList.Items.Add(skinContainer.SkinName);
            }
            //cboSkinList.SelectedText = DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName;
            cboSkinList.SelectedIndex = 0;

            currentSkin = DevExpress.Skins.CommonSkins.GetSkin(UserLookAndFeel.Default);
            Color mainStatusBarBackColor = currentSkin.Properties.GetColor("MainStatusBarBackColor");

            if (!mainStatusBarBackColor.IsEmpty)
                this.MainStatusBar.BackColor = mainStatusBarBackColor;

            ChangeUserInfoStatusLabel(this.presenter.WorkItem.RootWorkItem.State["LoginId"].ToString());

        }

        void ShellForm_Shown(object sender, EventArgs e)
        {
            WorkItem workItem = presenter.WorkItem;
            workItem.State["ShellShown"] = true;

            if (ConfigurationSettings.AppSettings["MenuLocation"].ToUpper().IndexOf("LEFT") >= 0)
            {
                IDictionary<string, string> paramDic = null;
                try
                {
                    paramDic = workItem.State["QueryStringParams"] as IDictionary<string, string>;
                }
                catch { }
                workItem.Services.Get<ILauncherService>().Run(paramDic);
            }

            this.EnabledToolStripPanel = true;
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                Timer t = new Timer();
                t.Interval = 10000;
                t.Tick += new EventHandler(t_Tick);
                t.Start();
                System.Deployment.Application.ApplicationDeployment appDeployment = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                appDeployment.CheckForUpdateCompleted += new System.Deployment.Application.CheckForUpdateCompletedEventHandler(appDeployment_CheckForUpdateCompleted);
                appDeployment.UpdateCompleted += new AsyncCompletedEventHandler(appDeployment_UpdateCompleted);
                btnYes.ButtonClick += new EventHandler(btnYes_ButtonClick);
                btnNo.ButtonClick += new EventHandler(btnNo_ButtonClick);
                btnUpdateBarClose.ButtonClick += new EventHandler(btnUpdateBarClose_ButtonClick);
                //appDeployment.CheckForUpdateAsync();
            }

            // 2010.09.26 kdy 로그인후 왼쪽 하단에 조그만 네모 상자 생겨서 주석처리 하여 실행하지 못하게 함
            //Eland.GBFW.Win.Common.Utility.XtraGridHelper.InitDeleteRowFont(this);
        }

        void appDeployment_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            isUpdate = false;
            BaseModuleInit.FlushMemory();
        }

        void btnNo_ButtonClick(object sender, EventArgs e)
        {
            btnYes.Visible = btnNo.Visible = false;
            btnUpdateBarClose.Visible = false;
            this.ExtensionStatusBar.Visible = false;
            this.ExtensionStatusBar.Update();
        }

        void btnYes_ButtonClick(object sender, EventArgs e)
        {
            btnYes.Visible = btnNo.Visible = false;
            btnUpdateBarClose.Visible = false;
            lblUpdate.Text = "New version updating...";
            prgUpdate.Style = ProgressBarStyle.Marquee;
            prgUpdate.Visible = true;
            this.ExtensionStatusBar.Visible = true;
            this.ExtensionStatusBar.Update();
            System.Deployment.Application.ApplicationDeployment appDeployment = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
            appDeployment.UpdateAsync();
            isUpdate = true;
        }

        void btnUpdateBarClose_ButtonClick(object sender, EventArgs e)
        {
            isUpdate = null;
            this.ExtensionStatusBar.Visible = false;
            this.Update();
        }

        bool? isUpdate = null;
        Version lastTriedVersion = null;
        void t_Tick(object sender, EventArgs e)
        {
            if (isUpdate == null)
            {
                System.Deployment.Application.ApplicationDeployment appDeployment = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                System.Deployment.Application.UpdateCheckInfo checkInfo = appDeployment.CheckForDetailedUpdate();
                if (checkInfo != null && checkInfo.UpdateAvailable && (appDeployment.UpdatedVersion < checkInfo.AvailableVersion))
                {
                    lblUpdate.Text = "New version is ready. Do you want to download it? ";
                    prgUpdate.Visible = false;
                    btnYes.Visible = btnNo.Visible = true;
                    btnUpdateBarClose.Visible = false;
                    this.ExtensionStatusBar.Visible = true;
                    this.ExtensionStatusBar.Update();
                }
            }
            else if (isUpdate != null)
            {
                if (isUpdate == true)
                {
                    ((Timer)sender).Interval = 500;
                }
                else
                {
                    prgUpdate.Style = ProgressBarStyle.Continuous;
                    lblUpdate.Text = "Update is completed. Please restart this application.";
                    btnYes.Visible = btnNo.Visible = false;
                    prgUpdate.Visible = btnUpdateBarClose.Visible = true;
                    this.ExtensionStatusBar.Visible = true;
                    this.ExtensionStatusBar.Update();
                    ((Timer)sender).Interval = 10000;
                }
            }
        }

        private void appDeployment_CheckForUpdateCompleted(object sender, System.Deployment.Application.CheckForUpdateCompletedEventArgs e)
        {
            try
            {
                System.Deployment.Application.ApplicationDeployment appDeployment = sender as System.Deployment.Application.ApplicationDeployment;
                if (appDeployment == null) appDeployment = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;

                if (e.UpdateAvailable)
                {
                    isUpdate = true;
                    appDeployment.Update();
                    isUpdate = false;
                }
                else
                {
                    System.Threading.Thread.Sleep(10000);
                    appDeployment.CheckForUpdateAsync();
                }
            }
            catch { }
            finally { BaseModuleInit.FlushMemory(); }
        }

        private void AddMultiRowButton(ToolStripItem item, IResourceService resourceService)
        {
            if (item is ToolStripSplitButton)
            {
                ToolStripSplitButton toolStripDropDownButton = item as ToolStripSplitButton;
                ToolStripDropDown toolStripDropDown = new ToolStripDropDown();
                string[] splitButtonName = ((string)item.Tag).Split('#');
                string[] splitButtonDetail = null;
                if (splitButtonName.Length > 1 && !string.IsNullOrEmpty(splitButtonName[1]))
                {
                    splitButtonDetail = splitButtonName[1].Split(',');
                    if (splitButtonDetail.Length > 0 )
                    {
                        int buttonNum = splitButtonDetail.Length;
                        ToolStripItem[] numToolStripButtonItems = new ToolStripItem[buttonNum];
                        for (int i = 0; i < buttonNum; i++)
                        {
                            numToolStripButtonItems[i] = GetNumToolStripButton(i + 2, new Size(item.Width, 20),
                                resourceService, splitButtonDetail[i].Trim());
                        }
                        toolStripDropDown.Items.AddRange(numToolStripButtonItems);
                        toolStripDropDownButton.DropDown = toolStripDropDown;
                        toolStripDropDownButton.DropDownDirection = ToolStripDropDownDirection.Default;
                    }
                }
            }
        }

        private static ToolStripButton GetNumToolStripButton(int num, Size size, IResourceService resourceService, string splitButtonName)
        {
            string caption = String.Format(resourceService.GetResourceString(splitButtonName), num);
            ToolStripButton numToolStripButton = new ToolStripButton(caption);
            numToolStripButton.Tag = num;
            numToolStripButton.Size = size;
            numToolStripButton.AutoSize = false;
            //
            return numToolStripButton;
        }

        private void ExeMenuToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.presenter.ShotKeyMenuExecute(this.ExeMenuToolStripTextBox.Text);
            }
        }

    }
}
