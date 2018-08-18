using System;
using System.Linq;

namespace SOPS.WebUI.ViewModels.Companies
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}