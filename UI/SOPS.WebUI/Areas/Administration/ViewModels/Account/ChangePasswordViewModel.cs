using System.ComponentModel.DataAnnotations;


namespace SOPS.WebUI.Areas.Administration.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Compare("PasswordConfirm")]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

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