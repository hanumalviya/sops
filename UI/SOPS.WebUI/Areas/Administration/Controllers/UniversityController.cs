using System;
using System.Linq;
using System.Web.Mvc;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Templates;
using SOPS.WebUI.Areas.Administration.Utilities;
using SOPS.WebUI.Areas.Administration.ViewModels.University;
using SOPS.Services.Employees;
using Model.Employees;
using System.Collections.Generic;

namespace SOPS.WebUI.Areas.Administration.Controllers
{
    [Authorize(Roles = EmployeesRoles.Administrator)]
    public class UniversityController : Controller
    {
        private readonly IDepartmentsProvider _departmentsProvider;
        private readonly ICourseUpdater _courseUpdater;
        private readonly ICourseDestructor _courseDestructor;
        private readonly ICourseCreator _courseCreator;
        private readonly IDepartmentUpdater _departmentUpdater;
        private readonly IDepartmentCreator _departmentCreatpr;
        private readonly IDepartmentDestructor _departmentDestructor;
        private readonly IEmployeesProvider _employeesProvider;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly ICoursesProvider _coursesProvider;

        public UniversityController(
            
            IDepartmentsProvider departmentsProvider,
            IDepartmentDestructor departmentDestructor,
            IDepartmentCreator departmentCreatpr,
            IDepartmentUpdater departmentUpdater,
            ICoursesProvider coursesProviderd,
            ICourseCreator courseCreator,
            ICourseDestructor courseDestructor,
            ICourseUpdater courseUpdater,
            IEmployeesProvider employeesProvider,
            ITemplatesProvider templatesProvider)
        {
            
            _departmentsProvider = departmentsProvider;
            _departmentDestructor = departmentDestructor;
            _departmentCreatpr = departmentCreatpr;
            _departmentUpdater = departmentUpdater;
            _courseCreator = courseCreator;
            _courseDestructor = courseDestructor;
            _courseUpdater = courseUpdater;
            _employeesProvider = employeesProvider;
            _templatesProvider = templatesProvider;
            _coursesProvider = coursesProviderd;
        }

        public ActionResult Index()
        {
            var curremUser = _employeesProvider.GetEmployee(User.Identity.Name);
            int userDepartmentId = curremUser.Course.Department.Id;

            ViewBag.IsRoot = curremUser.Root;

            IEnumerable<int> ids = null;

            if (curremUser.Root)
                ids = _departmentsProvider.GetAllDepartments().Select(n => n.Id);
            else
                ids = _departmentsProvider.GetAllDepartments().Where(n => n.Id == userDepartmentId).Select(n => n.Id);

            return View(ids);
        }

        [ChildActionOnly]
        public ActionResult Department(int departmentId)
        {
            var department = _departmentsProvider.GetDepartment(departmentId).ToViewModel();
            ViewBag.CanBeDestroyed = _departmentDestructor.CanBeDestroyed(departmentId);

            return PartialView("Partials/_department", department);
        }

        [ChildActionOnly]
        public ActionResult DeleteDepartment()
        {
            return PartialView("Dialogs/remove-department-dialog");
        }

        [HttpPost]
        public ActionResult DeleteDepartment(int id)
        {
            if (_departmentDestructor.CanBeDestroyed(id) == true)
            {
                _departmentDestructor.Destroy(id);                
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult AddDepartment()
        {
            return PartialView("Dialogs/add-department-dialog");
        }

        [HttpPost]
        [Authorize(Roles=EmployeesRoles.Root)]
        public ActionResult AddDepartment(DepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                _departmentCreatpr.Create(department.Name);
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult EditDepartment()
        {
            return PartialView("Dialogs/edit-department-dialog");
        }

        [HttpPost]
        public ActionResult EditDepartment(DepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                var d = _departmentsProvider.GetDepartment(department.Id);
                d.Name = department.Name;

                _departmentUpdater.Update(d);
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult AddCourse()
        {
            var templates = _templatesProvider.GetAllTemplates().Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Template = templates;

            return PartialView("Dialogs/add-course-dialog", new CourseViewModel());
        }

        [HttpPost]
        public ActionResult AddCourse(CourseViewModel course)
        {
            _courseCreator.Create(course.Name, course.DepartmentId);

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult EditCoursePartial(CourseViewModel course)
        {
            var keepers = _employeesProvider.GetEmployees(string.Empty)
                .Where(n => n.Course.Id == course.Id)
                .Select(n => new { Text = string.Format("{0} {1}", n.FirstName, n.LastName), Value = n.Id }).ToList();
            keepers.Insert(0, null);

            ViewBag.Keeper = keepers;

            var templates = _templatesProvider.GetAllTemplates().Select(n => new { Text = n.Name, Value = n.Id });
            
            ViewBag.Template = templates;
            ViewBag.CanBeDestroyed = _courseDestructor.CanBeDestroyed(course.Id);

            return PartialView("Partials/_course", course);
        }

        [HttpPost]
        public ActionResult EditCourse(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                var c = _coursesProvider.GetCourse(course.Id);
                var template = _templatesProvider.GetTemplate(course.Template.Value);

                Employee manager = null;
                if (course.Keeper.HasValue)
                {
                    manager = _employeesProvider.GetEmployee(course.Keeper.Value);
                }

                c.Name = course.Name;
                c.SetTemplate(template);

                _courseUpdater.Update(c);

                if ((c.Manager == null && course.Keeper.HasValue) 
                    || (c.Manager != null && c.Manager.Id != course.Keeper)
                    || (c.Manager != null && c.Manager == null))
                    _courseUpdater.SetManager(c, manager);
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult DeleteCourse()
        {
            return PartialView("Dialogs/remove-course-dialog");
        }

        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            if (_courseDestructor.CanBeDestroyed(id))
            {
                _courseDestructor.Destroy(id);
            }

            return RedirectToAction("Index");
        }
    }
}
