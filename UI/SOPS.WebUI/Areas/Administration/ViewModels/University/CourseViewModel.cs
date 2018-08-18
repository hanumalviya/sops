using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SOPS.WebUI.Areas.Administration.ViewModels.University
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Keeper { get; set; }
        public int? Template { get; set; }
    }
}