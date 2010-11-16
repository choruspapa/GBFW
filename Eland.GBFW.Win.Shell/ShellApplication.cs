using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Formular.Win.Cab;
using Formular.Win.Cab.Shell;
using Eland.GBFW.Win.Shell.Layout;
using Formular.Win.Cab.Services;
using System.Deployment.Application;
using System.EnterpriseServices.Internal;
using System.Reflection;
using System.IO;
using Eland.GBFW.Win.Common.Interface;
using Eland.GBFW.ProgramMng.Domain;
using Eland.GBFW.UserMng.Domain;
using Formular.Win.Cab.Constants;
using Eland.GBFW.Common.Util;
using System.Configuration;
using Eland.GBFW.EnvironmentMng.Service;
using System.Collections;
using Eland.GBFW.Win.ShellBase.Services;
using System.Web;
using Formular.Win.Cab.Workspaces;
using System.ComponentModel;


namespace Eland.GBFW.Win.Shell
{
    public class ShellApplication : SmartClientApplication<WorkItem, ShellForm>
    {
        private bool isDownloadingFile;
        bool isLoadingFormClose = false;

        private void ShowExcpetionInfo(string location, Exception e)
        {
            Exception ex = null;
            if (e is System.Web.Services.Protocols.SoapException)
            {
                System.Web.Services.Protocols.SoapException se = e as System.Web.Services.Protocols.SoapException;

                System.Xml.XmlAttribute attrType = null;
                System.Xml.XmlAttribute attrAssembly = null;
                System.Xml.XmlAttribute attrMessage = null;
                System.Xml.XmlAttribute attrStackTrace = null;

                System.Xml.XmlAttribute attrErrorCode = null;
                System.Xml.XmlAttribute attrParameters = null;

                Assembly assembly = null;
                Type type = null;

                int exceptionCount = se.Detail.ChildNodes.Count;
                for (int i = exceptionCount-1; i >= 0; i--)
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
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("StackTrace: " + e.StackTrace + Environment.NewLine);
            if (e.InnerException != null)
            {
                sb.Append(e.InnerException.Message);
                if (e.InnerException.InnerException != null)
                {
                    sb.Append(e.InnerException.InnerException.Message);
                    if (e.InnerException.InnerException.InnerException != null)
                        sb.Append(e.InnerException.InnerException.InnerException.Message);
                }

            }
            
            if (RootWorkItem != null && RootWorkItem.Services != null && RootWorkItem.Services.Get<IMessageService>() != null)
            {
                RootWorkItem.Services.Get<IMessageService>().ShowMessage(string.Format("StackTrace:{0}::{1}", location, e.Message),sb.ToString());
            }
        }

        [STAThread]
        public void Start(string[] arguments)
        {
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment appDeployment = ApplicationDeployment.CurrentDeployment;

                    //Register events for async work
                    //_appDeployment.DownloadFileGroupProgressChanged += new DeploymentProgressChangedEventHandler(OnDownloadFileGroupProgressChanged);
                    appDeployment.DownloadFileGroupCompleted += new DownloadFileGroupCompletedEventHandler(OnDownloadFileGroupCompleted);

                    isDownloadingFile = false;
                    if (appDeployment.IsFileGroupDownloaded("DevExpress") == false) { isDownloadingFile = true; appDeployment.DownloadFileGroupAsync("DevExpress", "DevExpress"); } else DevExpress.Skins.SkinManager.EnableFormSkins();
                    if (appDeployment.IsFileGroupDownloaded("Resources") == false) { isDownloadingFile = true; appDeployment.DownloadFileGroupAsync("Resources", "Resources"); }
                    if (appDeployment.IsFileGroupDownloaded("NHibernate") == false) { isDownloadingFile = true; appDeployment.DownloadFileGroupAsync("NHibernate", "NHibernate"); }
                    if (appDeployment.IsFileGroupDownloaded("Etc") == false) { isDownloadingFile = true; appDeployment.DownloadFileGroupAsync("Etc", "Etc"); }
                    if (appDeployment.IsFileGroupDownloaded("UserMng") == false) { isDownloadingFile = true; appDeployment.DownloadFileGroupAsync("UserMng", "UserMng"); }
                    if (appDeployment.IsFileGroupDownloaded("ProgramMng") == false) { isDownloadingFile = true; appDeployment.DownloadFileGroupAsync("ProgramMng", "ProgramMng"); }

                    if (isDownloadingFile == false) AfterDownloadingFile();
                }

                //base.SplashRun(Eland.GBFW.Win.Shell.Properties.Resources.eland_log);
                base.Run();
            }
            catch (Exception ex)
            {
                ShowExcpetionInfo("StartUp()", ex);
            }
            
        }

        

        #region AsyncEventHandlers

        // DownloadFileGroupCompleted, so update the progress dialog and the main form
        private void OnDownloadFileGroupCompleted(object sender, DownloadFileGroupCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ApplicationDeployment appDeployment = ApplicationDeployment.CurrentDeployment;
                if (e.Group == "DevExpress")
                {
                    DevExpress.Skins.SkinManager.EnableFormSkins();
                }
                if (appDeployment.IsFileGroupDownloaded("DevExpress") &&
                    appDeployment.IsFileGroupDownloaded("Resources") &&
                    appDeployment.IsFileGroupDownloaded("NHibernate") &&
                    appDeployment.IsFileGroupDownloaded("Etc") &&
                    appDeployment.IsFileGroupDownloaded("UserMng") &&
                    appDeployment.IsFileGroupDownloaded("ProgramMng")
                    )
                {
                    if (isDownloadingFile) AfterDownloadingFile();
                    isDownloadingFile = false;
                }

                if (Shell != null) Shell.DisplayDownloadingFileStatus(isDownloadingFile);

                //DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BonusSkins).Assembly); //Register!
                //DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.OfficeSkins).Assembly); //Register!
                //DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinTest1).Assembly); //Register!
            }
            else
            {
                ShowExcpetionInfo("OnDownloadFileGroupCompleted()", e.Error);
            }
        }

        private void AfterDownloadingFile()
        {
            ApplicationDeployment appDeployment = ApplicationDeployment.CurrentDeployment;
            if (appDeployment != null)
            {
                Uri uri = appDeployment.ActivationUri;
                if (uri != null && uri.Query.Length > 0)
                {
                    base.RootWorkItem.State["QueryStringParams"] = ParseQueryString(uri.Query);
                }
            }

            this.StartPrepareSpringServices();
        }

        #endregion

        protected override void AfterShellCreated()
        {
            try
            {
                StartBackgroundFlushMemory();
                base.AfterShellCreated();
                AddHandler();
                AddWorkspaces();
                SetShellLayout();
            }
            catch (Exception ex)
            {
                ShowExcpetionInfo("AfterShellCreated()", ex);
            }
        }

        protected override void Start()
        {
            try
            {
                // Run Login Program
                if (ValidateUser())
                {
                    Eland.GBFW.Win.Common.BaseModuleInit.FlushMemory();
                    // 프로그램 실행중... 화면 Open
                    OpenLoadingShell();

                    CheckAsyncDownLoad();

                    //// menu load & parameters
                    //if (ApplicationDeployment.IsNetworkDeployed)
                    //{
                    //Dictionary<string,string> args = GetQueryStringParameters();
                    //RootWorkItem.Services.Get<ILauncherService>().Run(args);
                    //}
                    //else
                    //{
                    //RootWorkItem.Services.Get<ILauncherService>().Run(null);
                    //}

                    // 프로그램 실행중... 화면 Close
                    CloseLoadingShell();

                    Eland.GBFW.Win.Common.BaseModuleInit.FlushMemory();
                    base.Start();
                }
            }
            catch (Exception ex)
            {
                ShowExcpetionInfo("Start()", ex);
            }
        }

        private bool ValidateUser()
        {
            try
            {
                // debug Login
                string isLoginValue = ConfigurationSettings.AppSettings["IsLogin"];
                bool isLogin = true;
                if (!string.IsNullOrEmpty(isLoginValue))
                    bool.TryParse(isLoginValue, out isLogin);

                if (isLogin == false)
                {
                    this.RootWorkItem.State["LoginId"] = ConfigurationSettings.AppSettings["LoginId"];
                    int userId = 0;
                    int.TryParse(ConfigurationSettings.AppSettings["UserId"], out userId);
                    this.RootWorkItem.State["UserId"] = userId;
                    this.RootWorkItem.State["MenuProgramId"] = ConfigurationSettings.AppSettings["MenuProgramId"];
                    return true;
                }
                // real Run Login
                IUserShellService userShellService = RootWorkItem.Services.Get<IUserShellService>();

                Eland.GBFW.Win.Common.BaseModuleInit.FlushMemory();
                if (userShellService != null)
                {
                    return userShellService.ValidateUser(); // Login Form 
                }
                return false;
            }
            catch (Exception e)
            {
                ShowExcpetionInfo("ValidateUser()", e);
                return false;
            }
        }

        protected override void AddServices()
        {
            try
            {
                base.AddServices();
                IRootWorkItemService rootWorkItemService = SpringUtil.GetObject<IRootWorkItemService>("RootWorkItemService");
                rootWorkItemService.RootWorkItem = RootWorkItem;
                RootWorkItem.Services.Add<IRootWorkItemService>(rootWorkItemService);

                Eland.GBFW.Win.Shell.Services.CultureService cultureService = new Eland.GBFW.Win.Shell.Services.CultureService();
                cultureService.CurrentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;
                cultureService.CurrentCultureName = System.Globalization.CultureInfo.CurrentCulture.Name;

                ICommandButtonService commandButtonService = new CommandButtonService();

                RootWorkItem.Services.Remove<ICultureService>();
                RootWorkItem.Services.Add<ICultureService>(cultureService);
                RootWorkItem.Services.Add<IResourceService>(cultureService);
                RootWorkItem.Services.Add<ICommandButtonService>(commandButtonService);

                RootWorkItem.Services.Add<ILauncherService>(new LauncherService());

            }
            catch (Exception e)
            {
                ShowExcpetionInfo("AddServices()", e);
            }
        }

        private void AddWorkspaces()
        {
            try
            {
                ShellImplementor shellImplementor = SpringUtil.GetObject<ShellImplementor>("ShellImplementor");

                shellImplementor.WorkItem = this.RootWorkItem;
                shellImplementor.ShellForm = Shell;

                ICoordinator uiCoordinatorService = RootWorkItem.Services.Get<ICoordinator>();
                uiCoordinatorService.ShellImplementor = shellImplementor;

                //Content Workspace Create. 
                IWorkspace contentWorkspace = uiCoordinatorService.GetWorkspace(SmartPartTypes.ContentPart);
                ((Control)contentWorkspace).Name = "ContentWorkspace";
                if (contentWorkspace is Control) Shell.ContentPart.Controls.Add((Control)contentWorkspace);
                RootWorkItem.Workspaces.Add(contentWorkspace, WorkspaceNames.ContentPart);

                //Business Menu Workspace Create. 
                IWorkspace businessMenuPart = uiCoordinatorService.GetWorkspace(SmartPartTypes.BusinessMenuPart);
                RootWorkItem.Workspaces.Add(businessMenuPart, WorkspaceNames.BusinessMenuPart);

                //ApplicationMenu Workspace Create.             
                IWorkspace applicationMenuPart = uiCoordinatorService.GetWorkspace(SmartPartTypes.ApplicationMenuPart);
                if (applicationMenuPart is Control) Shell.ApplicationMenuPart.Controls.Add((Control)applicationMenuPart);
                RootWorkItem.Workspaces.Add(applicationMenuPart, WorkspaceNames.ApplicationMenuPart);
                
                //Popup Workspace Create.             
                IWorkspace workspace = uiCoordinatorService.GetWorkspace(SmartPartTypes.PopupPart);
                RootWorkItem.Workspaces.Add(workspace, WorkspaceNames.PopupPart);
            }
            catch (Exception e)
            {
                ShowExcpetionInfo("AddWorkspaces()", e);
            }
        }

        private void SetShellLayout()
        {
            try
            {
                Shell.Initialize();
                Shell.DisplayDownloadingFileStatus(isDownloadingFile);
                SetContentWorkspaceEventHandler();
            }
            catch (Exception e)
            {
                ShowExcpetionInfo("AddWorkspaces()", e);
            }
        }

        private void AddHandler()
        {
            IAfterAsyncHandlerService afterAsyncHandlerService = RootWorkItem.Services.Get<IAfterAsyncHandlerService>();
            IDictionary exceptionHandlerList = Spring.Context.Support.ContextRegistry.GetContext().GetObjectsOfType(typeof(BaseChainHandler));
            IDictionaryEnumerator enumerator = exceptionHandlerList.GetEnumerator();
            while (enumerator.MoveNext())
                if (enumerator.Value is BaseChainHandler)
                    afterAsyncHandlerService.AddHandler(enumerator.Key as string, enumerator.Value as BaseChainHandler);
        }


        /// <summary>
        /// Set Content Workspace Event Handler.
        /// </summary>
        private void SetContentWorkspaceEventHandler()
        {
            //ICurrentContentWorkspaceService currentContentWorkspaceService =  RootWorkItem.Services.Get<ICurrentContentWorkspaceService>();

            //Shell.SetContentWorkspaceEventHandler(currentContentWorkspaceService.CurrentContentWorksapce);
            //Shell.SetContentWorkspaceEventHandler(this.RootWorkItem.Workspaces[WorkspaceNames.ContentPart]);
            Shell.SetContentWorkspaceEventHandler(this.RootWorkItem.Workspaces[WorkspaceNames.ContentPart]);
        }


        //public static Dictionary<string, string> GetQueryStringParameters()
        //{
        //  Dictionary<string, string> queryStringparams = new Dictionary<string, string>();
        //  Uri uri = ApplicationDeployment.CurrentDeployment.ActivationUri;
        //  if (uri != null && uri.Query.Length > 0)
        //  {
        //    queryStringparams = ParseQueryString(uri.Query);
        //  }
        //  return queryStringparams;
        //}

        private void CheckAsyncDownLoad()
        {
            while (isDownloadingFile)
                System.Threading.Thread.Sleep(1000);
        }

        private static Dictionary<string, string> ParseQueryString(string queryString)
        {
          Dictionary<string, string> queryStringParams = new Dictionary<string, string>();
          if (string.IsNullOrEmpty(queryString))
            return queryStringParams;
          queryString = HttpUtility.UrlDecode(queryString);

          if (queryString[0] == '?')
          {
            queryString = queryString.Substring(1);
          }

          string[] parts = queryString.Split('&');
          foreach (string part in parts)
          {
            string trimmedPart = part.Trim();
            if (!string.IsNullOrEmpty(trimmedPart))
            {
              string[] paramSet = trimmedPart.Split('=');
              if (paramSet.Length == 1)
              {
                queryStringParams.Add(paramSet[0], null);
              }
              if (paramSet.Length == 2)
              {
                queryStringParams.Add(paramSet[0], paramSet[1]);
              }
            }
          }
          return queryStringParams;
        }

        private void StartPrepareSpringServices()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(PrepareSpringServices)).Start();
        }

        private void PrepareSpringServices()
        {
            Spring.Context.IApplicationContext context = Spring.Context.Support.ContextRegistry.GetContext();
            string[] names = context.GetObjectDefinitionNames();
            foreach (string name in names)
            {
                try
                {
                    context.GetObject(name);
                }
                catch { }
            }
        }

        private void StartBackgroundFlushMemory()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(BackgroundFlushMemory)).Start();
        }

        private void BackgroundFlushMemory()
        {
            while (Shell.IsDisposed == false)
            {
                try
                {
                    Eland.GBFW.Win.Common.BaseModuleInit.FlushMemory();
                }
                catch { }
                System.Threading.Thread.Sleep(5000);
            }
        }

        private void OpenLoadingShell()
        {
            System.Threading.ThreadPool.QueueUserWorkItem((x) =>
            {
                using (LoadingForm loadingForm = new LoadingForm())
                {
                    loadingForm.Show();
                    while (!isLoadingFormClose)
                        Application.DoEvents();
                    loadingForm.Close();
                }
            });
        }

        private void CloseLoadingShell()
        {
            isLoadingFormClose = true;
        }
    }
}
