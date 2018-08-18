using System.ComponentModel.DataAnnotations;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Account
{
    public class UpdateProfileViewModel
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
        public bool IsLockedOut { get; set; }
    }
}