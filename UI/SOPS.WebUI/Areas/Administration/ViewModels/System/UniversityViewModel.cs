using System.ComponentModel.DataAnnotations;

namespace SOPS.WebUI.Areas.Administration.ViewModels.System
{
    public class UniversityViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}