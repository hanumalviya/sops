using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Employees
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public int Course { get; set; }

        public int Department { get; set; }

        public bool SuperAdministrator { get; set; }

        public bool Administrator { get; set; }

        public bool Moderator { get; set; }
    }
}