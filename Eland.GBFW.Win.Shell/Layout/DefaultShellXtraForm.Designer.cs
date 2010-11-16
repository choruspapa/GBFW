namespace Eland.GBFW.Win.Shell.Layout
{
    partial class DefaultShellXtraForm
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
            this.contentPart = new Formular.Win.Cab.BlankPart();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.MainToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.MenuToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainStatusBar = new System.Windows.Forms.StatusStrip();
            this.UserInfoToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ApplicationMenuPart = new Formular.Win.Cab.BlankPart();
            this.MainStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPart
            // 
            this.contentPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPart.Location = new System.Drawing.Point(0, 22);
            this.contentPart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.contentPart.Name = "contentPart";
            this.contentPart.Size = new System.Drawing.Size(714, 423);
            this.contentPart.TabIndex = 0;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 445);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(714, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightToolStripPanel.Location = new System.Drawing.Point(714, 22);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 423);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 22);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 423);
            // 
            // MainToolStripPanel
            // 
            this.MainToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainToolStripPanel.Location = new System.Drawing.Point(0, 22);
            this.MainToolStripPanel.Name = "MainToolStripPanel";
            this.MainToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.MainToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.MainToolStripPanel.Size = new System.Drawing.Size(714, 0);
            // 
            // MenuToolStripStatusLabel
            // 
            this.MenuToolStripStatusLabel.Name = "MenuToolStripStatusLabel";
            this.MenuToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainStatusBar
            // 
            this.MainStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuToolStripStatusLabel,
            this.UserInfoToolStripStatusLabel});
            this.MainStatusBar.Location = new System.Drawing.Point(0, 445);
            this.MainStatusBar.Name = "MainStatusBar";
            this.MainStatusBar.Size = new System.Drawing.Size(714, 22);
            this.MainStatusBar.TabIndex = 7;
            this.MainStatusBar.Text = "statusStrip1";
            // 
            // UserInfoToolStripStatusLabel
            // 
            this.UserInfoToolStripStatusLabel.Name = "UserInfoToolStripStatusLabel";
            this.UserInfoToolStripStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UserInfoToolStripStatusLabel.Size = new System.Drawing.Size(699, 17);
            this.UserInfoToolStripStatusLabel.Spring = true;
            this.UserInfoToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ApplicationMenuPart
            // 
            this.ApplicationMenuPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.ApplicationMenuPart.Location = new System.Drawing.Point(0, 0);
            this.ApplicationMenuPart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ApplicationMenuPart.Name = "ApplicationMenuPart";
            this.ApplicationMenuPart.Size = new System.Drawing.Size(714, 22);
            this.ApplicationMenuPart.TabIndex = 5;
            this.ApplicationMenuPart.Visible = false;
            // 
            // DefaultShellXtraForm
            // 
            this.ClientSize = new System.Drawing.Size(714, 467);
            this.Controls.Add(this.contentPart);
            this.Controls.Add(this.BottomToolStripPanel);
            this.Controls.Add(this.RightToolStripPanel);
            this.Controls.Add(this.LeftToolStripPanel);
            this.Controls.Add(this.MainToolStripPanel);
            this.Controls.Add(this.MainStatusBar);
            this.Controls.Add(this.ApplicationMenuPart);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DefaultShellXtraForm";
            this.Text = "DefaultShellXtraForm";
            this.MainStatusBar.ResumeLayout(false);
            this.MainStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Formular.Win.Cab.BlankPart contentPart;
        public System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        public System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        public System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        public System.Windows.Forms.ToolStripPanel MainToolStripPanel;
        public System.Windows.Forms.ToolStripStatusLabel MenuToolStripStatusLabel;
        public System.Windows.Forms.StatusStrip MainStatusBar;
        public System.Windows.Forms.ToolStripStatusLabel UserInfoToolStripStatusLabel;
        public Formular.Win.Cab.BlankPart ApplicationMenuPart;




    }
}