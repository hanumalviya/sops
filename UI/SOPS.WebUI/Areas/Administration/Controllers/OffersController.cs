using Model.Employees;
using NHibernateRepository.UnitOfWork;
using SOPS.Services.Companies;
using SOPS.Services.Offers;
using SOPS.WebUI.Areas.Administration.Utilities;
using SOPS.WebUI.Areas.Administration.ViewModels.Offers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.Controllers
{
    [Authorize(Roles = EmployeesRoles.Moderator)]
    public class OffersController : Controller
    {
        private readonly IOfferTypeProvider _offerTypeProvider;
        private readonly IOffersProvider _offersProvider;
        private readonly IOfferCreator _offerCreator;
        private readonly IOfferDestructor _offerDestructor;
        private readonly IOfferUpdater _offerUpdater;
        private readonly ICompaniesProvider _companiesProvider;
        

        public OffersController(            
            IOfferTypeProvider offerTypeProvider,
            IOffersProvider offersProvider,
            IOfferCreator offerCreator,
            IOfferDestructor offerDestructor,
            IOfferUpdater offerUpdater,
            ICompaniesProvider companiesProvider)
        {
            _offerTypeProvider = offerTypeProvider;
            _offersProvider = offersProvider;
            _offerCreator = offerCreator;
            _offerDestructor = offerDestructor;
            _offerUpdater = offerUpdater;
            _companiesProvider = companiesProvider;            
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DataTable(string search)
        {
            var list = _offersProvider.GetOffers(search)
                .Select(offer => new ListItemViewModel()
                {
                    Title = offer.Title,
                    Id = offer.Id,
                    Company = offer.Company.Name
                }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult DetailsPartial()
        {
            var type = _offerTypeProvider.GetAllOfferTypes().Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Type = type;

            var company = _companiesProvider.GetCompanies(string.Empty).Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Company = company;

            return PartialView("Partials/_details", new OfferViewModel());
        }

        public JsonResult Details(int id)
        {
            var details = _offersProvider.GetOffer(id).ToViewModel();

            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Add()
        {
            var viewModel = new OfferViewModel();

            var type = _offerTypeProvider.GetAllOfferTypes().Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Type = type;

            var company = _companiesProvider.GetCompanies(string.Empty).Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Company = company;

            return PartialView("Dialogs/add-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Add(OfferViewModel offer)
        {
            if (ModelState.IsValid)
            {
                var company = _companiesProvider.GetCompany(offer.Company);
                var type = _offerTypeProvider.GetOfferType(offer.Type);
                _offerCreator.Create(offer.Title, offer.Description, offer.Trade, company, type);
                
                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [ChildActionOnly]
        public ActionResult Edit()
        {
            var viewModel = new OfferViewModel();

            var type = _offerTypeProvider.GetAllOfferTypes().Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Type = type;

            var company = _companiesProvider.GetCompanies(string.Empty).Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Company = company;

            return PartialView("Dialogs/edit-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(OfferViewModel offer)
        {
            if (ModelState.IsValid)
            {
                var o = _offersProvider.GetOffer(offer.Id);
                o.Approved = offer.Approved;
                o.Company = _companiesProvider.GetCompany(offer.Company);
                o.Date = DateTime.Parse(offer.Date);
                o.Description = offer.Description;
                o.Title = offer.Title;
                o.Trade = offer.Trade;
                o.Type = _offerTypeProvider.GetOfferType(offer.Type);

                _offerUpdater.Update(o);
                
                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [ChildActionOnly]
        public ActionResult Delete()
        {
            return PartialView("Dialogs/remove-dialog");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                _offerDestructor.Destroy(id);
                
                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }
    }
}
