using System;
using SOPS.WebUI.ViewModels.Practices;
using System.Web.Mvc;
using SOPS.WebUI.ViewModels.Shared;
using System.Linq;
using SOPS.Services.Offers;
using SOPS.Services.Companies;

namespace SOPS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOffersProvider _offersProvider;
        private readonly ICompaniesProvider _companiesProvider;

        public HomeController(
            IOffersProvider offersProvider,
            ICompaniesProvider companiesProvider)
        {
            _offersProvider = offersProvider;
            _companiesProvider = companiesProvider;
        }

        public ActionResult Index(string search, int? page)
        {
            ViewBag.Search = search;
            ViewBag.PageIndex = page;

            return View();
        }

        public ActionResult Grid(string search, int? page)
        {
            ViewBag.Search = search;
            ViewBag.Page = page;

            var list = _offersProvider.GetOffers(search);
            int pageSize = 14;
            int itemsToSkip = (page ?? 0)*pageSize;
            var elements = list.Skip(itemsToSkip).Take(pageSize).Select(n => new ListItemViewModel()
            {
                Company = n.Company.Name,
                Id = n.Id,
                Title = n.Title,
                Type = n.Type.Name
            });

            return PartialView("Partials/_grid", elements);
        }

        [ChildActionOnly]
        public ActionResult Paging(string search, int? page)
        {
            var list = _offersProvider.GetOffers(search);
            Paging p = new Paging()
                {
                    CurrentPage = page?? 0,
                    NumberOfElements = list.Count,
                    PageSize = 14
                };
            return PartialView("Partials/_paging", p);
        }

        public ActionResult Details(int id)
        {
            var offer = _offersProvider.GetOffer(id);

            var offerDetails = new OfferViewModel()
                {
                    CompanyName = offer.Company.Name,
                    CompanyId = offer.Company.Id,
                    Date = offer.Date.ToShortDateString(),
                    Title = offer.Title,
                    Description = offer.Description,
                    Id = id,
                    Type = offer.Type.Name
                };

            return View(offerDetails);
        }
    }
}
