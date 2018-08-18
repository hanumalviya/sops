using System.Collections.Generic;

namespace SOPS.WebUI.Areas.Administration.ViewModels.System
{
    public class ResourcesListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ResourceViewModel> Resources { get; set; }
    }
}