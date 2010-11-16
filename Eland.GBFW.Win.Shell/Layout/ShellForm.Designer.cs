namespace Eland.GBFW.Win.Shell.Layout
{
    partial class ShellForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShellForm));
            this.Extension = new System.Windows.Forms.ToolStrip();
            this.FavoriteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cboSkinList = new System.Windows.Forms.ToolStripComboBox();
            this.ExeMenuToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.lblDownloading = new Formular.Win.UI.Controls.FSharpLabel();
            this.ExtensionStatusBar = new System.Windows.Forms.StatusStrip();
            this.lblUpdate = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgUpdate = new System.Windows.Forms.ToolStripProgressBar();
            this.btnYes = new System.Windows.Forms.ToolStripSplitButton();
            this.btnNo = new System.Windows.Forms.ToolStripSplitButton();
            this.btnUpdateBarClose = new System.Windows.Forms.ToolStripSplitButton();
            this.MainToolStripPanel.SuspendLayout();
            this.Extension.SuspendLayout();
            this.ExtensionStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(714, 49);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 396);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 49);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 396);
            // 
            // MainToolStripPanel
            // 
            this.MainToolStripPanel.Controls.Add(this.Extension);
            this.MainToolStripPanel.Location = new System.Drawing.Point(0, 24);
            this.MainToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
            this.MainToolStripPanel.Size = new System.Drawing.Size(714, 25);
            // 
            // ApplicationMenuPart
            // 
            this.ApplicationMenuPart.Margin = new System.Windows.Forms.Padding(0);
            this.ApplicationMenuPart.Size = new System.Drawing.Size(714, 24);
            // 
            // _contentPart
            // 
            this._contentPart.Location = new System.Drawing.Point(0, 49);
            this._contentPart.Margin = new System.Windows.Forms.Padding(0);
            this._contentPart.Size = new System.Drawing.Size(714, 396);
            // 
            // Extension
            // 
            this.Extension.BackColor = System.Drawing.Color.Transparent;
            this.Extension.Dock = System.Windows.Forms.DockStyle.None;
            this.Extension.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Extension.GripMargin = new System.Windows.Forms.Padding(0);
            this.Extension.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FavoriteToolStripButton,
            this.cboSkinList,
            this.ExeMenuToolStripTextBox});
            this.Extension.Location = new System.Drawing.Point(3, 0);
            this.Extension.Name = "Extension";
            this.Extension.Padding = new System.Windows.Forms.Padding(0);
            this.Extension.Size = new System.Drawing.Size(316, 25);
            this.Extension.TabIndex = 8;
            this.Extension.Text = "toolStrip1";
            // 
            // FavoriteToolStripButton
            // 
            this.FavoriteToolStripButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FavoriteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FavoriteToolStripButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FavoriteToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FavoriteToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.FavoriteToolStripButton.Margin = new System.Windows.Forms.Padding(2);
            this.FavoriteToolStripButton.Name = "FavoriteToolStripButton";
            this.FavoriteToolStripButton.Size = new System.Drawing.Size(23, 21);
            this.FavoriteToolStripButton.Text = "즐겨찾기추가";
            this.FavoriteToolStripButton.Click += new System.EventHandler(this.FavoriteToolStripButton_Click);
            // 
            // cboSkinList
            // 
            this.cboSkinList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cboSkinList.Margin = new System.Windows.Forms.Padding(0);
            this.cboSkinList.Name = "cboSkinList";
            this.cboSkinList.Size = new System.Drawing.Size(121, 25);
            this.cboSkinList.SelectedIndexChanged += new System.EventHandler(this.cboSkinList_SelectedIndexChanged);
            // 
            // ExeMenuToolStripTextBox
            // 
            this.ExeMenuToolStripTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ExeMenuToolStripTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.ExeMenuToolStripTextBox.Name = "ExeMenuToolStripTextBox";
            this.ExeMenuToolStripTextBox.Size = new System.Drawing.Size(130, 25);
            this.ExeMenuToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExeMenuToolStripTextBox_KeyDown);
            // 
            // lblDownloading
            // 
            this.lblDownloading.AutoSize = true;
            this.lblDownloading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDownloading.Image = null;
            this.lblDownloading.Location = new System.Drawing.Point(266, 210);
            this.lblDownloading.Name = "lblDownloading";
            this.lblDownloading.Size = new System.Drawing.Size(324, 25);
            this.lblDownloading.TabIndex = 8;
            this.lblDownloading.Text = "Downloading DevExpress Dll Files...";
            // 
            // ExtensionStatusBar
            // 
            this.ExtensionStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUpdate,
            this.prgUpdate,
            this.btnYes,
            this.btnNo,
            this.btnUpdateBarClose});
            this.ExtensionStatusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ExtensionStatusBar.Location = new System.Drawing.Point(0, 421);
            this.ExtensionStatusBar.Name = "ExtensionStatusBar";
            this.ExtensionStatusBar.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.ExtensionStatusBar.Size = new System.Drawing.Size(714, 24);
            this.ExtensionStatusBar.SizingGrip = false;
            this.ExtensionStatusBar.TabIndex = 9;
            this.ExtensionStatusBar.Visible = false;
            // 
            // lblUpdate
            // 
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(132, 19);
            this.lblUpdate.Text = "New version updating...";
            // 
            // prgUpdate
            // 
            this.prgUpdate.Name = "prgUpdate";
            this.prgUpdate.Size = new System.Drawing.Size(86, 18);
            this.prgUpdate.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgUpdate.Value = 100;
            // 
            // btnYes
            // 
            this.btnYes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnYes.DropDownButtonWidth = 0;
            this.btnYes.Image = ((System.Drawing.Image)(resources.GetObject("btnYes.Image")));
            this.btnYes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(30, 22);
            this.btnYes.Text = "Yes";
            // 
            // btnNo
            // 
            this.btnNo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNo.DropDownButtonWidth = 0;
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(28, 22);
            this.btnNo.Text = "No";
            // 
            // btnUpdateBarClose
            // 
            this.btnUpdateBarClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUpdateBarClose.DropDownButtonWidth = 0;
            this.btnUpdateBarClose.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateBarClose.Image")));
            this.btnUpdateBarClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdateBarClose.Name = "btnUpdateBarClose";
            this.btnUpdateBarClose.Size = new System.Drawing.Size(56, 22);
            this.btnUpdateBarClose.Text = "Confirm";
            // 
            // ShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 467);
            this.Controls.Add(this.ExtensionStatusBar);
            this.Controls.Add(this.lblDownloading);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "ShellForm";
            this.Text = "ShellForm";
            this.Load += new System.EventHandler(this.ShellForm_Load);
            this.Controls.SetChildIndex(this.lblDownloading, 0);
            this.Controls.SetChildIndex(this.ExtensionStatusBar, 0);
            this.Controls.SetChildIndex(this.ApplicationMenuPart, 0);
            this.Controls.SetChildIndex(this.MainToolStripPanel, 0);
            this.Controls.SetChildIndex(this.LeftToolStripPanel, 0);
            this.Controls.SetChildIndex(this.RightToolStripPanel, 0);
            this.Controls.SetChildIndex(this.BottomToolStripPanel, 0);
            this.Controls.SetChildIndex(this._contentPart, 0);
            this.MainToolStripPanel.ResumeLayout(false);
            this.MainToolStripPanel.PerformLayout();
            this.Extension.ResumeLayout(false);
            this.Extension.PerformLayout();
            this.ExtensionStatusBar.ResumeLayout(false);
            this.ExtensionStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip Extension;
        private Formular.Win.UI.Controls.FSharpLabel lblDownloading;
        private System.Windows.Forms.ToolStripButton FavoriteToolStripButton;
        private System.Windows.Forms.ToolStripComboBox cboSkinList;
        private System.Windows.Forms.ToolStripTextBox ExeMenuToolStripTextBox;
        private System.Windows.Forms.ToolStripStatusLabel lblUpdate;
        private System.Windows.Forms.ToolStripProgressBar prgUpdate;
        public System.Windows.Forms.StatusStrip ExtensionStatusBar;
        private System.Windows.Forms.ToolStripSplitButton btnYes;
        private System.Windows.Forms.ToolStripSplitButton btnNo;
        private System.Windows.Forms.ToolStripSplitButton btnUpdateBarClose;
    }
}