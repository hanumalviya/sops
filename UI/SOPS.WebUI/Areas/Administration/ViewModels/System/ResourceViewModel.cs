using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOPS.WebUI.Areas.Administration.ViewModels
{
    public class ResourceViewModel
    {
        public int Id { get; set; }
        public string Course { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool InUse { get; set; }
    }
}
