using System.ComponentModel.DataAnnotations;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string PasswordQuestion { get; set; }

        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string PasswordAnswer { get; set; }
    }
}