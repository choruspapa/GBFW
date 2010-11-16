using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using System.Windows.Forms;
using System.Xml;
using Eland.GBFW.ProgramMng.Domain;
using Eland.GBFW.Win.Common.Interface;
using System.Drawing;

namespace Eland.GBFW.Win.Shell.Layout
{
    public static class UIElementBuilder
    {
        public static void LoadFromConfig(WorkItem workItem, ToolStripPanel toolStripPanel, XmlDocument doc)
        {
            XmlConfig xcfg = new XmlConfig(doc);

            ConfigSetting newplugin = xcfg.Settings["toolstrips"];

            foreach (ConfigSetting setting in newplugin.Children())
            {
                if (setting.Name.Contains("comment"))
                    continue;
                MenuItemElement menuItem = new MenuItemElement();

                menuItem.CommandName = setting["commandname"].Value;
                menuItem.Icon = setting["icon"].Value;
                menuItem.Label = setting["label"].Value;
                menuItem.ID = setting["id"].Value == "" ? 0 : Convert.ToInt32(setting["id"].Value);
                menuItem.Key = setting["key"].Value;
                menuItem.IsContainer = setting["iscontainer"].Value == "" ? false : true;
                menuItem.Site = setting["site"].Value;

                ProcessConfigItem(workItem, toolStripPanel, menuItem);
            }
        }

        public static void ProcessConfigItem(WorkItem workItem, ToolStripPanel toolStripPanel, MenuItemElement menuItem)
        {

            if (menuItem.IsContainer)
            {

                ToolStrip toolStrip = menuItem.ToToolStrip();

                toolStrip.Size = new System.Drawing.Size(442, 25);
                toolStrip.Text = menuItem.Site;
                toolStrip.Name = menuItem.Site;
                toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
                toolStripPanel.Join(toolStrip);

                workItem.UIExtensionSites.RegisterSite(menuItem.Site, toolStrip);

                workItem.RootWorkItem.Items.Add(toolStrip, menuItem.Site);
            }
            else
            {

                foreach (Control control in toolStripPanel.Controls)
                {

                    ToolStrip toolStrip = control as ToolStrip;
                    toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);

                    if (toolStrip != null)
                    {

                        if (toolStrip.Name == menuItem.Site)
                        {

                            ToolStripButton toolStripButton = menuItem.ToToolStripItem();
                            toolStripButton.ToolTipText = menuItem.CommandName;
                            toolStripButton.Name = menuItem.CommandName;
                            toolStripButton.Tag = menuItem.Icon;
                            toolStripButton.Image = menuItem.GetIcon(menuItem.Icon);
                            if (workItem.UIExtensionSites.Contains(menuItem.Site))
                            {

                                workItem.UIExtensionSites[menuItem.Site].Add<ToolStripButton>(toolStripButton);
                            }

                            if (!String.IsNullOrEmpty(menuItem.CommandName))
                                workItem.Commands[menuItem.CommandName].AddInvoker(toolStripButton, "Click");

                        }
                    }
                }
            }
        }

        public static void ProcessConfigItem(WorkItem workItem, ToolStripPanel toolStripPanel, List<Operation> operation)
        {
            IResourceService _resourceService = workItem.Services.Get<IResourceService>();
            if (operation == null) return;
            MenuItemElement menuItem = new MenuItemElement();

            foreach (Operation op in operation)
            {
                if (op.ContainerYN)
                {

                    ToolStrip toolStrip = menuItem.ToToolStrip();
                    //ToolStrip toolStrip = new ToolStrip();

                    toolStrip.Size = new System.Drawing.Size(442, 25);
                    toolStrip.Text = op.LocationType.ToString();
                    toolStrip.Name = op.LocationType.ToString();
                    toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
                    toolStripPanel.Join(toolStrip);

                    workItem.UIExtensionSites.RegisterSite(op.LocationType.ToString(), toolStrip);

                    workItem.RootWorkItem.Items.Add(toolStrip, op.LocationType.ToString());
                }
                else
                {

                    foreach (Control control in toolStripPanel.Controls)
                    {

                        ToolStrip toolStrip = control as ToolStrip;
                        toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);

                        if (toolStrip != null)
                        {

                            if (toolStrip.Name == op.LocationType.ToString())
                            {

                                //ToolStripButton toolStripButton = op.ToToolStripItem();
                                ToolStripButton toolStripButton = new ToolStripButton();
                                toolStripButton.ToolTipText = op.Name;
                                toolStripButton.Name = op.Name;
                                toolStripButton.Tag = op.NormalResId;
                                toolStripButton.Image = menuItem.GetIcon(op.NormalResId);
                                if (workItem.UIExtensionSites.Contains(op.LocationType.ToString()))
                                {

                                    workItem.UIExtensionSites[op.LocationType.ToString()].Add<ToolStripButton>(toolStripButton);
                                }

                                if (!String.IsNullOrEmpty(op.Name))
                                    workItem.Commands[op.Name].AddInvoker(toolStripButton, "Click");

                            }
                        }
                    }
                }
            }
        }
    }
}
