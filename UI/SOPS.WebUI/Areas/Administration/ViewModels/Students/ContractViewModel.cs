using System;
using System.ComponentModel.DataAnnotations;

namespace SOPS.WebUI.Areas.Administration.ViewModels.Students
{
    public class ContractViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int? CompanyId { get; set; }

        [Required]
        public string CompanyRepresentative { get; set; }

        [Required]
        public string UniversityRepresentative { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}