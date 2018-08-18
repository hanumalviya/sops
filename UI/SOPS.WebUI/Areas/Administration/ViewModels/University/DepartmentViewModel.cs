using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SOPS.WebUI.Areas.Administration.ViewModels.University
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}