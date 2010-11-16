using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Eland.GBFW.Win.Shell;
using System.Deployment.Application;
using System.Web;
using System.Net;

namespace Eland.GBFW.Win.Startup
{
    static class Program
    {
        static LoadingForm loadingForm = null;
        static bool? isLoading = null;

        static string currentPath = Environment.CurrentDirectory;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            Application.EnableVisualStyles();

            // Setup //
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "unzip.exe");
            psi.Arguments = "-n DevExpress_Setup.zip";
            psi.CreateNoWindow = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process unzipProcess = System.Diagnostics.Process.Start(psi);
            unzipProcess.WaitForExit();
            ///////////

            try
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("dfsvc");
                foreach (System.Diagnostics.Process proc in processes)
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch { }
                }
                //processes = System.Diagnostics.Process.GetProcesses();
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                //{
                //    foreach (System.Diagnostics.Process proc in processes)
                //    {
                //        try
                //        {
                //            SetProcessWorkingSetSize(proc.Handle, -1, -1);
                //        }
                //        catch { }
                //    }
                //}
                //FlushMemory();
            }
            catch { }

            try
            {
                ApplicationDeployment appDeployment = null;

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    loadingForm = new LoadingForm();
                    isLoading = true;
                    OpenLoadingShell();
                }

                if (loadingForm != null)
                {
                    while (isLoading != false)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);
                        //FlushMemory();
                    }
                }

                new ShellApplication().Start(arguments);
                //if (appDeployment != null && appDeployment.CheckForUpdate())
                //{
                //    appDeployment.Update();
                //}
            }
            catch (Exception ex)
            {
                //if (appDeployment != null && appDeployment.CheckForUpdate())
                //{
                //    appDeployment.Update();
                //}

                //DialogResult result = MessageBox.Show("예상하지 못한 오류가 발생하였습니다. 프로그램 재설치시 문제가 해결될 수 있습니다. 프로그램을 삭제하시겠습니까?", "프로그램 오류 발생", MessageBoxButtons.YesNo);
                //if (result == DialogResult.Yes)
                //{
                //    string appsPath = null;
                //    appsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                //    if (String.Compare(appsPath.Substring(appsPath.LastIndexOf('\\') + 1), "Local", true) == 0) appsPath += @"\Apps";
                //    else appsPath = System.IO.Directory.GetParent(appsPath).FullName + @"\Apps";

                //    System.IO.File.Copy(currentPath + @"\DeleteApps.bat", appsPath + @"\DeleteApps.bat", true);

                //    System.Diagnostics.ProcessStartInfo delpsi = new System.Diagnostics.ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "unzip.exe");
                //    delpsi.WorkingDirectory = appsPath;
                //    delpsi.FileName = @"DeleteApps.bat";
                //    delpsi.CreateNoWindow = true;
                //    delpsi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //    System.Diagnostics.Process.Start(delpsi);
                //    return;
                //}

                throw ex;
            }
            finally
            {
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minWorkingSetSize, int maxWorkingSetSize);
        private static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        static int _percent = 0;
        static DownloadGroups[] GROUPS = new DownloadGroups[]{
            new DownloadGroups("Setup", 50),
            new DownloadGroups("Setup_GBFW", 25),
            new DownloadGroups("Interface", 23)
        };

        static void OpenLoadingShell()
        {
            //System.Threading.Thread thd = new System.Threading.Thread(new System.Threading.ThreadStart(StartLoading));
            //thd.Start();
            System.Threading.ThreadPool.QueueUserWorkItem((x) => { StartLoading(); });
        }

        static void StartLoading()
        {
            ApplicationDeployment appDeployment = ApplicationDeployment.CurrentDeployment;
            loadingForm.Show();
            loadingForm.Update();
            int formCount = Application.OpenForms.Count;
            loadingForm.PerformStep(0, "Downloading setup files...");
            appDeployment.DownloadFileGroupProgressChanged += new DeploymentProgressChangedEventHandler(appDeployment_DownloadFileGroupProgressChanged);
            int totalPercent = 0;
            for (int i = 0; i < GROUPS.Length; i++)
            {
                if (appDeployment.IsFileGroupDownloaded(GROUPS[i].Group) == false)
                {
                    _percent = 0;
                    appDeployment.DownloadFileGroupAsync(GROUPS[i].Group);
                    while (_percent < 100)
                    {
                        System.Threading.Thread.Sleep(100);
                        loadingForm.PerformStep(totalPercent + (((GROUPS[i].Percent) / 100 * _percent)), String.Format("Downloading setup files (GROUP : {0})...", GROUPS[i].Group));
                    }
                    FlushMemory();
                }
                totalPercent += GROUPS[i].Percent;
                loadingForm.PerformStep(totalPercent, "Downloading setup files...");
            }

            loadingForm.PerformStep(totalPercent, "Now prepare to start application.");
            isLoading = false;

            while (isLoading != null)
            {
                Application.DoEvents();
                if (formCount != Application.OpenForms.Count) break;
                System.Threading.Thread.Sleep(200);
                loadingForm.Update();
            }

            loadingForm.Close();
            loadingForm.Dispose();
            loadingForm = null;

            FlushMemory();
        }

        static void appDeployment_DownloadFileGroupProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            _percent = e.ProgressPercentage;
        }

        public struct DownloadGroups
        {
            public string Group;
            public int Percent;

            public DownloadGroups(string group, int percent)
            {
                this.Group = group;
                this.Percent = percent;
            }
        }

        public class LoadingForm : Form
        {
            public LoadingForm()
            {
                InitializeComponent();
                this.Size = new System.Drawing.Size(300, 300);
                this.ShowInTaskbar = true;
                this.Text = "Loading...";
                this.StartPosition = FormStartPosition.CenterScreen;
                try
                {
                    System.Collections.Specialized.NameValueCollection settings = System.Configuration.ConfigurationManager.GetSection("SpringAppSettings") as System.Collections.Specialized.NameValueCollection;
                    string imgUrl = settings["url"] + "/images/BackgroundImage.jpg";
                    System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(new System.Net.WebClient().DownloadData(imgUrl)));
                    this.Size = img.Size;
                    this.BackgroundImage = img;
                    this.lblLoadingMsg.BackColor = System.Drawing.Color.Transparent;
                    this.prgbLoading.BackColor = System.Drawing.Color.Transparent;
                }
                catch { }
            }

            private void LoadingForm_Load(object sender, EventArgs e)
            {
                this.lblLoadingMsg.Text = "Loading ...";
                this.prgbLoading.Style = ProgressBarStyle.Continuous;
                System.Collections.Specialized.NameValueCollection settings = System.Configuration.ConfigurationManager.GetSection("SpringAppSettings") as System.Collections.Specialized.NameValueCollection;
                string imgUrl = settings["url"] + "/images/BackgroundImage.jpg";
                System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(new System.Net.WebClient().DownloadData(imgUrl)));
                this.picBack.BackColor = System.Drawing.Color.FromArgb(95, System.Drawing.Color.White);
            }

            public void PerformStep(int percent, string message)
            {
                this.lblLoadingMsg.Text = message;
                this.prgbLoading.Value = percent;
                this.Update();
            }

            #region Designer Generate Code(s)

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.picBack = new System.Windows.Forms.PictureBox();
                this.prgbLoading = new System.Windows.Forms.ProgressBar();
                this.lblLoadingMsg = new System.Windows.Forms.Label();
                this.SuspendLayout();
                // 
                // picBack
                // 
                this.picBack.Dock = DockStyle.Fill;
                this.picBack.Name = "picBack";
                this.picBack.TabIndex = 0;
                this.picBack.UseWaitCursor = true;
                // 
                // prgbLoading
                // 
                this.prgbLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.prgbLoading.Location = new System.Drawing.Point(46, 194);
                this.prgbLoading.Name = "prgbLoading";
                this.prgbLoading.Size = new System.Drawing.Size(508, 23);
                this.prgbLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                this.prgbLoading.TabIndex = 2;
                this.prgbLoading.UseWaitCursor = true;
                // 
                // lblLoadingMsg
                // 
                this.lblLoadingMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.lblLoadingMsg.Location = new System.Drawing.Point(48, 161);
                this.lblLoadingMsg.Name = "lblLoadingMsg";
                this.lblLoadingMsg.Size = new System.Drawing.Size(504, 29);
                this.lblLoadingMsg.TabIndex = 1;
                this.lblLoadingMsg.Text = "label1";
                this.lblLoadingMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.lblLoadingMsg.UseWaitCursor = true;
                // 
                // LoadingForm
                // 
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
                this.BackColor = System.Drawing.Color.Lavender;
                this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.ClientSize = new System.Drawing.Size(600, 430);
                this.Controls.Add(this.lblLoadingMsg);
                this.Controls.Add(this.prgbLoading);
                this.Controls.Add(this.picBack);
                this.DoubleBuffered = true;
                this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Name = "LoadingForm";
                this.ShowInTaskbar = false;
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Text = "LoadingForm";
                this.UseWaitCursor = true;
                this.Load += new System.EventHandler(this.LoadingForm_Load);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.PictureBox picBack;
            private System.Windows.Forms.ProgressBar prgbLoading;
            private System.Windows.Forms.Label lblLoadingMsg;

            #endregion
        }
    }
}
