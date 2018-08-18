using SOPS.WebUI.ViewModels.Companies;
using System;
using System.Linq;
using System.Web.Mvc;
using SOPS.WebUI.ViewModels.Shared;
using SOPS.Services.Companies;

namespace SOPS.WebUI.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompaniesProvider _companiesProvider;

        public CompanyController(
            ICompaniesProvider companiesProvider)
        {
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
            var list = _companiesProvider.GetCompanies(search);
            int pageSize = 20;
            int itemsToSkip = (page ?? 0)*pageSize;
            var elements = list.Skip(itemsToSkip).Take(pageSize).Select(n => new ListItemViewModel()
            {
                Id = n.Id,
                Company = n.Name
            });

            return PartialView("Partials/_grid", elements);
        }

        [ChildActionOnly]
        public ActionResult Paging(string search, int? page)
        {
            var list = _companiesProvider.GetCompanies(search);
            var p = new Paging()
                {
                    CurrentPage = page?? 0,
                    NumberOfElements = list.Count,
                    PageSize = 20
                };

            return PartialView("Partials/_paging", p);
        }

        public ActionResult Details(int id)
        {
            var c = _companiesProvider.GetCompany(id);

            var companyDetails = new CompanyViewModel()
                {
                    Company = c.Name,
                    Address = string.Format("{0}, {1}, {2}", c.City, c.PostalCode, c.Street),
                    Email = c.Email,
                    Description = c.Description,
                    Id = c.Id,
                    Phone = c.Phone,
                    Site = c.Url
                };

            return View(companyDetails);
        }
    }
}
