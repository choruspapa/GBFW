using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Formular.Win.Cab;
using System.Reflection;

namespace Eland.GBFW.Win.Shell.Layout
{
    public partial class DefaultShellXtraForm : DevExpress.XtraEditors.XtraForm, IView, Microsoft.Practices.ObjectBuilder.IBuilderAware, IBaseShell
    {
        public BlankPart ContentPart
        {
            get { return contentPart; }
        }

        public bool EnabledToolStripPanel
        {
            get { return this.MainMenuStrip.Enabled; }
            set { this.MainMenuStrip.Enabled = this.LeftToolStripPanel.Enabled = this.RightToolStripPanel.Enabled = this.BottomToolStripPanel.Enabled = value; }
        }

        private ContentForm _contentForm = new ContentForm();
        public DefaultShellXtraForm()
        {
            InitializeComponent();
            /*
            this.SuspendLayout();
            // 
            // contentPart
            // 
            this.contentPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPart.Location = new System.Drawing.Point(0, 22);
            this.contentPart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.contentPart.Size = new System.Drawing.Size(714, 423);
            this.contentPart.Name = "contentPart";
            this.contentPart.TabIndex = 0;
            // 
            // BaseShell
            // 
            _contentForm.Dock = DockStyle.Fill;
            _contentForm.Controls.Add(contentPart);
            this.BottomToolStripPanel = _contentForm.BottomToolStripPanel;
            this.RightToolStripPanel = _contentForm.RightToolStripPanel;
            this.LeftToolStripPanel = _contentForm.LeftToolStripPanel;
            this.MainToolStripPanel = _contentForm.MainToolStripPanel;
            this.MenuToolStripStatusLabel = _contentForm.MenuToolStripStatusLabel;
            this.MainStatusBar = _contentForm.MainStatusBar;
            this.UserInfoToolStripStatusLabel = _contentForm.UserInfoToolStripStatusLabel;
            this.ApplicationMenuPart = _contentForm.ApplicationMenuPart;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(_contentForm);
            this.ResumeLayout(false);
            */
            
            /*
            this.SuspendLayout();
            // 
            // contentPart
            // 
            this.contentPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPart.Location = new System.Drawing.Point(0, 22);
            this.contentPart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.contentPart.Size = new System.Drawing.Size(714, 423);
            this.contentPart.Name = "contentPart";
            this.contentPart.TabIndex = 0;
            // 
            // BaseShell
            // 
            this.Controls.Add(contentPart);
            this.ResumeLayout(false);




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
            this.UserInfoToolStripStatusLabel.Size = new System.Drawing.Size(668, 17);
            this.UserInfoToolStripStatusLabel.Spring = true;
            this.UserInfoToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TopMenuBarWorkspace
            // 
            this.ApplicationMenuPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.ApplicationMenuPart.Location = new System.Drawing.Point(0, 0);
            this.ApplicationMenuPart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ApplicationMenuPart.Name = "ApplicationMenuPart";
            this.ApplicationMenuPart.Size = new System.Drawing.Size(714, 22);
            this.ApplicationMenuPart.TabIndex = 5;
            // 
            // DefaultShellForm
            // 
            this.ClientSize = new System.Drawing.Size(714, 467);
            this.Controls.Add(this.BottomToolStripPanel);
            this.Controls.Add(this.RightToolStripPanel);
            this.Controls.Add(this.LeftToolStripPanel);
            this.Controls.Add(this.MainToolStripPanel);
            this.Controls.Add(this.MainStatusBar);
            this.Controls.Add(this.ApplicationMenuPart);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DefaultShellForm";
            this.Text = "DefaultShellForm";
            this.Controls.SetChildIndex(this.ApplicationMenuPart, 0);
            this.Controls.SetChildIndex(this.MainStatusBar, 0);
            this.Controls.SetChildIndex(this.MainToolStripPanel, 0);
            this.Controls.SetChildIndex(this.LeftToolStripPanel, 0);
            this.Controls.SetChildIndex(this.RightToolStripPanel, 0);
            this.Controls.SetChildIndex(this.BottomToolStripPanel, 0);
            this.MainStatusBar.ResumeLayout(false);
            this.MainStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
             * */
        }

        #region IBuilderAware 멤버

        public void OnBuiltUp(string id)
        {
            FieldInfo[] fieldInfos = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField | BindingFlags.DeclaredOnly);
            FieldInfo presenterFieldInfo = null;
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (typeof(AbstractPresenterBase).IsAssignableFrom(fieldInfo.FieldType))
                {
                    presenterFieldInfo = fieldInfo;
                    break;
                }
            }

            if (presenterFieldInfo != null)
                (presenterFieldInfo.GetValue(this) as AbstractPresenterBase).OnViewBuiltUp(this);
        }

        public void OnTearingDown()
        {
        }

        #endregion

        public virtual void InitializeView() { }

        #region IView Members

        public virtual bool CanCloseView() { return true; }
        public virtual string NestedWorkspaceID { get; set; }
        public virtual void OnClosing() { }
        public virtual void OnActive() { }
        public virtual int ProgramID { get; set; }
        public virtual int MenuID { get; set; }
        public IDictionary<string, Microsoft.Practices.CompositeUI.Commands.CommandStatus> AvailButtonList { get; set; }
        public virtual void ChangeCommandButtonState(string commandName, Microsoft.Practices.CompositeUI.Commands.CommandStatus status) { }
        public virtual object ExtraData { get; set; }
        public virtual void OnLoadView(object sender, Formular.Win.Cab.Entities.ViewLoadEventArgs e) { }

        #endregion

    }

    public class ContentForm : XtraPanel
    {
        public ContentForm()
        {
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
            this.UserInfoToolStripStatusLabel.Size = new System.Drawing.Size(668, 17);
            this.UserInfoToolStripStatusLabel.Spring = true;
            this.UserInfoToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TopMenuBarWorkspace
            // 
            this.ApplicationMenuPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.ApplicationMenuPart.Location = new System.Drawing.Point(0, 0);
            this.ApplicationMenuPart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ApplicationMenuPart.Name = "ApplicationMenuPart";
            this.ApplicationMenuPart.Size = new System.Drawing.Size(714, 22);
            this.ApplicationMenuPart.TabIndex = 5;
            // 
            // DefaultShellForm
            // 
            this.ClientSize = new System.Drawing.Size(714, 467);
            this.Controls.Add(this.BottomToolStripPanel);
            this.Controls.Add(this.RightToolStripPanel);
            this.Controls.Add(this.LeftToolStripPanel);
            this.Controls.Add(this.MainToolStripPanel);
            this.Controls.Add(this.MainStatusBar);
            this.Controls.Add(this.ApplicationMenuPart);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DefaultShellForm";
            this.Text = "DefaultShellForm";
            this.Controls.SetChildIndex(this.ApplicationMenuPart, 0);
            this.Controls.SetChildIndex(this.MainStatusBar, 0);
            this.Controls.SetChildIndex(this.MainToolStripPanel, 0);
            this.Controls.SetChildIndex(this.LeftToolStripPanel, 0);
            this.Controls.SetChildIndex(this.RightToolStripPanel, 0);
            this.Controls.SetChildIndex(this.BottomToolStripPanel, 0);
            this.MainStatusBar.ResumeLayout(false);
            this.MainStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

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