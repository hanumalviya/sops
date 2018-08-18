using Model.Offers;
using SOPS.Services.Companies;
using SOPS.Services.Offers;
using SOPS.WebUI.Areas.Administration.ViewModels.Offers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.Utilities
{
    public static class OffersUtilities
    {
        public static OfferViewModel ToViewModel(this Offer o)
        {
            return o == null ? null : new OfferViewModel()
            {
                Approved = o.Approved,
                Company = o.Company.Id,
                Date = o.Date.ToShortDateString(),
                Description = o.Description,
                Id = o.Id,
                Title = o.Title,
                Trade = o.Trade,
                Type = o.Type.Id
            };
        }

        public static Offer ToModel(this OfferViewModel o)
        {
            var companiesProvider = DependencyResolver.Current.GetService<ICompaniesProvider>();
            var typesProvider = DependencyResolver.Current.GetService<IOfferTypeProvider>();

            return o == null ? null : new Offer()
            {
                Approved = o.Approved,
                Company = companiesProvider.GetCompany(o.Company),
                Date = DateTime.Parse(o.Date),
                Description = o.Description,
                Id = o.Id,
                Title = o.Title,
                Trade = o.Trade,
                Type = typesProvider.GetOfferType(o.Type)
            };
        }
    }
}