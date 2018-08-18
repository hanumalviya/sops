using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Companies
{
    public class CompanyViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MaxLength(500, ErrorMessage="Maksymalna długość 500 znaków")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}