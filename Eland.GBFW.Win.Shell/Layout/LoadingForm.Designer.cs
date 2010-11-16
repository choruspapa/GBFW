namespace Eland.GBFW.Win.Shell.Layout
{
    partial class LoadingForm
    {
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
            this.prgbLoading = new System.Windows.Forms.ProgressBar();
            this.lblLoadingMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // prgbLoading
            // 
            this.prgbLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prgbLoading.Location = new System.Drawing.Point(46, 194);
            this.prgbLoading.Name = "prgbLoading";
            this.prgbLoading.Size = new System.Drawing.Size(508, 23);
            this.prgbLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgbLoading.TabIndex = 0;
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
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LoadingForm";
            this.TopMost = true;
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.LoadingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar prgbLoading;
        private System.Windows.Forms.Label lblLoadingMsg;
    }
}