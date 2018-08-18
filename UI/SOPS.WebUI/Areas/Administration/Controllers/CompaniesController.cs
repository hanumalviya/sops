using System.Linq;
using System.Web.Mvc;
using SOPS.Services.Companies;
using SOPS.WebUI.Areas.Administration.Utilities;
using SOPS.WebUI.Areas.Administration.ViewModels.Companies;
using Model.Employees;

namespace SOPS.WebUI.Areas.Administration.Controllers
{
    [Authorize(Roles = EmployeesRoles.Moderator)]
    public class CompaniesController : Controller
    {
        private readonly ICompaniesProvider _companiesProvider;
        private readonly ICompanyUpdater _companyUpdater;
        private readonly ICompanyDestructor _companyDestructor;
        private readonly ICompanyCreator _companyCreator;
        

        public CompaniesController(            
            ICompanyCreator companyCreator,
            ICompanyDestructor companyDestructor,
            ICompanyUpdater companyUpdater,
            ICompaniesProvider companiesProvider)
        {
            _companyCreator = companyCreator;
            _companyDestructor = companyDestructor;
            _companyUpdater = companyUpdater;
            _companiesProvider = companiesProvider;            
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DataTable(string search)
        {
            var list = _companiesProvider.GetCompanies(search)
                .Select(company => new ListItemViewModel()
                {
                    Name = company.Name,
                    Id = company.Id
                }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult DetailsPartial()
        {
            return PartialView("Partials/_details", new CompanyViewModel());
        }

        public JsonResult Details(int id)
        {
            var details = _companiesProvider.GetCompany(id).ToViewModel();

            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Add()
        {
            var viewModel = new CompanyViewModel();

            return PartialView("Dialogs/add-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Add(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                _companyCreator.Create(company.Name, company.Street, company.City, company.PostalCode, company.Email, company.Phone, company.Url, company.Description);

                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [ChildActionOnly]
        public ActionResult Edit()
        {
            var viewModel = new CompanyViewModel();

            return PartialView("Dialogs/edit-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                var c = _companiesProvider.GetCompany(company.Id);
                c.Name = company.Name;
                c.Street = company.Street;
                c.City = company.City;
                c.PostalCode = company.PostalCode;
                c.Description = company.Description;
                c.Email = company.Email;
                c.Phone = company.Phone;
                c.Url = company.Url;

                _companyUpdater.Update(c);
                
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
            if (_companyDestructor.CanBeDestroy(id))
            {
                _companyDestructor.Destroy(id);
                return Json(true);
            }

            ModelState.AddModelError("CannotDestroy", "Nie można usunąć firmy powiązanej z umowami studentów");
            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

    }
}
