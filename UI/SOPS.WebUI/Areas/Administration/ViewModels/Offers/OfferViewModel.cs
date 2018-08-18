using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Offers
{
    public class OfferViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Approved { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "yyyy-MM-dd")]
        public string Date { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Company { get; set; }

        [Required]
        public string Trade { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(500)]
        [Required]
        public string Description { get; set; }
    }
}