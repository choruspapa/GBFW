using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

namespace Eland.GBFW.Win.Shell
{
    public class MenuItemElement : ConfigurationElement
    {
        [ConfigurationProperty("commandname", IsRequired = false)]
        public string CommandName
        {
            get
            {
                return (string)this["commandname"];
            }
            set
            {
                this["commandname"] = value;
            }
        }

        [ConfigurationProperty("key", IsRequired = false)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("id", IsRequired = false, IsKey = true)]
        public int ID
        {
            get
            {
                return (int)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("label", IsRequired = true)]
        public string Label
        {
            get
            {
                return (string)this["label"];
            }
            set
            {
                this["label"] = value;
            }
        }

        [ConfigurationProperty("site", IsRequired = true)]
        public string Site
        {
            get
            {
                return (string)this["site"];
            }
            set
            {
                this["site"] = value;
            }
        }

        [ConfigurationProperty("iscontainer", IsRequired = false)]
        public bool IsContainer
        {
            get
            {
                return (bool)this["iscontainer"];
            }
            set
            {
                this["iscontainer"] = value;
            }
        }


        [ConfigurationProperty("icon", IsRequired = false)]
        public string Icon
        {
            get
            {
                return (string)this["icon"];
            }
            set
            {
                this["icon"] = value;
            }
        }

        public ToolStrip ToToolStrip()
        {
            ToolStrip toolStrip = new ToolStrip();

            return toolStrip;
        }

        public ToolStripButton ToToolStripItem(string buttonName)
        {
            ToolStripButton toolStripButton = new ToolStripButton(buttonName);

            return toolStripButton;
        }


        public ToolStripButton ToToolStripItem()
        {
            ToolStripButton toolStripButton = new ToolStripButton();

            return toolStripButton;
        }
    }
}
