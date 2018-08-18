using System;
using System.Linq;
using System.Web.Mvc;
using SOPS.WebUI.ViewModels.Shared;
using SOPS.Services.Employees;
using SOPS.WebUI.ViewModels.Managers;

namespace SOPS.WebUI.Controllers
{
    public class ManagersController : Controller
    {
        private readonly IEmployeesProvider _employeesProvider;

        public ManagersController(
            IEmployeesProvider employeesProvider)
        {
            _employeesProvider = employeesProvider;
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

            var list = _employeesProvider.GetEmployees(search);

            int pageSize = 14;
            int itemsToSkip = (page ?? 0)*pageSize;
            var elements = list.Where(n => n.Course != null && n.Keeper).Skip(itemsToSkip).Take(pageSize).Select(n => new ListItemViewModel()
            {
                Id = n.Id,
                Course = n.Course.Name,
                Department = n.Course.Department != null ? n.Course.Department.Name : null,
                Email = n.Email,
                FullName = string.Format("{0} {1}", n.FirstName, n.LastName)
            });

            return PartialView("Partials/_grid", elements);
        }

        [ChildActionOnly]
        public ActionResult Paging(string search, int? page)
        {
            var list = _employeesProvider.GetEmployees(search);
            Paging p = new Paging()
                {
                    CurrentPage = page?? 0,
                    NumberOfElements = list.Count,
                    PageSize = 14
                };
            return PartialView("Partials/_paging", p);
        }
    }
}
