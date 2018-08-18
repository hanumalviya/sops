using Model.Companies;
using SOPS.WebUI.Areas.Administration.ViewModels.Companies;
using System;
using System.Linq;

namespace SOPS.WebUI.Areas.Administration.Utilities
{
    public static class CompaniesUtilities
    {
        public static CompanyViewModel ToViewModel(this Company c)
        {
            return c == null ? null : new CompanyViewModel()
            {
                Street = c.Street,
                City = c.City,
                PostalCode = c.PostalCode,
                Name = c.Name,
                Description = c.Description, 
                Email = c.Email,
                Id = c.Id,
                Phone = c.Phone,
                Url = c.Url
            };
        }
    }
}