using System.ComponentModel.DataAnnotations;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Compare("PasswordConfirm")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        public string PasswordQuestion { get; set; }

        [Required]
        [MinLength(5)]
        [Compare("PasswordAnswerConfirm")]
        [DataType(DataType.Password)]
        public string PasswordAnswer { get; set; }

        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string PasswordAnswerConfirm { get; set; }
    }
}