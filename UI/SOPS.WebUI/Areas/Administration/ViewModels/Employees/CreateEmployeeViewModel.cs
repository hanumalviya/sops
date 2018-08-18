using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Employees
{
    public class CreateEmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Course { get; set; }

        public int Department { get; set; }

        public bool SuperAdministrator { get; set; }

        public bool Administrator { get; set; }

        public bool Moderator { get; set; }

        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        [Compare("PasswordConfirm")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("AnswerConfirm")]
        [MinLength(6)]
        public string Answer { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string AnswerConfirm { get; set; }
    }
}