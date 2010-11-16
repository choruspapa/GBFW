using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Eland.GBFW.Win.Shell.Layout
{
    public partial class LoadingForm : Form
    {

        public LoadingForm()
        {
            InitializeComponent();

            int loginFormWidth = 300;
            int loginFormHeight = 200;
            try
            {
                loginFormWidth = int.Parse(ConfigurationSettings.AppSettings["LoginFormWidth"]);
            }
            catch { }
            try
            {
                loginFormHeight = int.Parse(ConfigurationSettings.AppSettings["LoginFormHeight"]);
            }
            catch { }

            this.Size = new Size(loginFormWidth, loginFormHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Specialized.NameValueCollection settings = System.Configuration.ConfigurationManager.GetSection("SpringAppSettings") as System.Collections.Specialized.NameValueCollection;
                string imgUrl = settings["url"] + "/images/BackgroundImage.jpg";
                Image img = Image.FromStream(new System.IO.MemoryStream(new System.Net.WebClient().DownloadData(imgUrl)));
                this.BackgroundImage = img;
                this.lblLoadingMsg.BackColor = System.Drawing.Color.Transparent;
                this.prgbLoading.BackColor = System.Drawing.Color.Transparent;
            }
            catch { }
            this.lblLoadingMsg.Text = "Loading ...";
            this.prgbLoading.MarqueeAnimationSpeed = 1;
            this.prgbLoading.Style = ProgressBarStyle.Marquee;
            this.BringToFront();
            this.Activate();
        }
    }
}
