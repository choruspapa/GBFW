using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Eland.GBFW.Win.Common.Interface;

namespace Eland.GBFW.Win.Shell.Services
{
    public class RootWorkItemService :IRootWorkItemService
    {
        public WorkItem RootWorkItem { get; set; }
        public RootWorkItemService() { }
    }
}
