using Model.Employees;
using Model.University;
using NHMembership.Services;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Mail;
using SOPS.WebUI.Areas.Administration.Utilities;
using SOPS.WebUI.Areas.Administration.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.Controllers
{
    [Authorize(Roles = EmployeesRoles.Administrator)]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesProvider _employeesProvider;
        private readonly IDepartmentsProvider _departmetmentsProvider;
        private readonly IEmployeeDestructor _employeeDestructor;
        private readonly IEmployeeUpdater _employeeUpdater;
        private readonly ICoursesProvider _coursesProvider;
        private readonly IEmployeeCreator _employeeCreator;
        private readonly IMailService _mailService;

        public EmployeesController(            
            IEmployeesProvider employeesProvider,
            IEmployeeDestructor employeeDestructor,
            IEmployeeUpdater employeeUpdater,
            IEmployeeCreator employeeCreator,
            IDepartmentsProvider departmetmentsProvider,
            ICoursesProvider coursesProvider,
            IMailService mailService)
        {
            _employeesProvider = employeesProvider;
            _departmetmentsProvider = departmetmentsProvider;
            _employeeDestructor = employeeDestructor;
            _employeeUpdater = employeeUpdater;
            _coursesProvider = coursesProvider;
            _employeeCreator = employeeCreator;
            _mailService = mailService;
        }

        public ActionResult Index()
        {            
            return View();
        }

        public JsonResult DataTable(string search)
        {
            IList<Employee> list = null;
            var currrentEmployee = _employeesProvider.GetEmployee(User.Identity.Name);

            if (currrentEmployee.Root)
            {
                list = _employeesProvider.GetEmployees(search);
            }
            else
            {
                list = _employeesProvider.GetEmployees(currrentEmployee.Course.Department.Id, search);
            }            

            var results = list.Select(n => new ListItemViewModel()
                {
                    Course = n.Course.Name,
                    Id = n.Id,
                    Name = n.FirstName + " " + n.LastName
                }).ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Courses(int department = 0)
        {
            Department d;
            if (department == 0)
                d = _departmetmentsProvider.GetAllDepartments().First();
            else
                d = _departmetmentsProvider.GetDepartment(department);

            var courses = d.Courses.Select(n => new {Text = n.Name, Value = n.Id});

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult DetailsPartial()
        {
            var department = _departmetmentsProvider.GetAllDepartments().Select(n => new { Text = n.Name, Value = n.Id });
            var course = _departmetmentsProvider.GetAllDepartments().First().Courses.Select(n => new { Text = n.Name, Value = n.Id });

            ViewBag.Department = department;
            ViewBag.Course = course;

            return PartialView("Partials/_details", new EmployeeViewModel());
        }

        public JsonResult Details(int id)
        {
            var employee = _employeesProvider.GetEmployee(id).ToViewModel();

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Add()
        {
            var currrentEmployee = _employeesProvider.GetEmployee(User.Identity.Name);
            ViewBag.IsRoot = currrentEmployee.Root;
            LoadDepartments();

            return PartialView("Dialogs/add-dialog", new CreateEmployeeViewModel());
        }

        [HttpPost]
        public ActionResult Add(CreateEmployeeViewModel e)
        {
            if (ModelState.IsValid)
            {
                var course = _coursesProvider.GetCourse(e.Course);
                EmployeeCreateStatus status;
                var employee = _employeeCreator.Create(e.UserName,
                    e.FirstName,
                    e.LastName,
                    e.Password,
                    e.Email,
                    e.Question,
                    e.Answer,
                    e.Moderator,
                    e.SuperAdministrator,
                    e.Administrator,
                    course,
                    out status);

                if (status == EmployeeCreateStatus.Success)
                {
                    string message = string.Format("Twoje konto w systemie SOPS zostało utworzone\n Nazwa użytkownika: {0}\n Hasło: {1}\nPytanie do resetowania hasła: {2}\nOdpowiedź: {3}",
                        e.UserName, e.Password, e.Question, e.Password);

                    _mailService.SendMail("Twoje konto w systemie SOPS zostało utworzone", message, e.Email);
                    return Json(true);
                }

                switch (status)
                {
                    case EmployeeCreateStatus.DuplicateMail:
                        ModelState.AddModelError("DuplicateMail", "Użytkownik o podanym adresie email już istnieje");
                        break;
                    case EmployeeCreateStatus.UserAlreadyExist:
                        ModelState.AddModelError("UserAlreadyExist", "Użytkownik już istnieje");
                        break;
                    case EmployeeCreateStatus.InvalidAnswer:
                        ModelState.AddModelError("InvalidAnswer", "Niepoprawna odpowiedź");
                        break;
                    case EmployeeCreateStatus.UserRejected:
                        ModelState.AddModelError("UserRejected", "Użytkownik odrzucony");
                        break;
                    case EmployeeCreateStatus.ProviderError:
                        ModelState.AddModelError("ProviderError", "Wystąpił nieoczekiwany bląd");
                        break;
                    case EmployeeCreateStatus.InvalidUserName:
                        ModelState.AddModelError("InvalidUserName", "Niepoprawna nazwa użytkownika");
                        break;
                    case EmployeeCreateStatus.InvalidQuestion:
                        ModelState.AddModelError("InvalidQuestion", "Niepoprawne pytanie");
                        break;
                    case EmployeeCreateStatus.InvalidPassword:
                        ModelState.AddModelError("InvalidPassword", "Niepoprawna hasło");
                        break;
                    case EmployeeCreateStatus.InvalidEmail:
                        ModelState.AddModelError("InvalidEmail", "Niepoprawny adres email");
                        break;
                }
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [ChildActionOnly]
        public ActionResult Edit()
        {
            var viewModel = new EmployeeViewModel();
            var currrentEmployee = _employeesProvider.GetEmployee(User.Identity.Name);
            ViewBag.IsRoot = currrentEmployee.Root;
            LoadDepartments();

            return PartialView("Dialogs/edit-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var m = _employeesProvider.GetEmployee(employee.Id);
                var currenCourse = m.Course;
                var course = _coursesProvider.GetCourse(employee.Course);

                if (m.Keeper == true && currenCourse.Id != course.Id)
                {
                    ModelState.AddModelError("CannotChangeCourse", "Nie można zmienić wydziału/kursu pracownika który jest opiekunem kierunku");
                }
                else
                {
                    m.Course = course;
                    m.Administrator = employee.Administrator;
                    m.FirstName = employee.FirstName;
                    m.LastName = employee.LastName;
                    m.Moderator = employee.Moderator;

                    _employeeUpdater.Update(m);

                    return Json(true);
                }
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
                var currentEmployee = _employeesProvider.GetEmployee(User.Identity.Name);

                if (currentEmployee.Id == id)
                {
                    ModelState.AddModelError("CannotRemoveEmployee", "Nie można usunąć samego siebie");
                }
                else
                {
                    if (_employeeDestructor.CanBeDestroyed(id, currentEmployee.Id))
                    {
                        _employeeDestructor.Destroy(id);
                        return Json(true);
                    }
                    else
                    {
                        ModelState.AddModelError("CannotRemoveEmployee", "Nie można usunąć pracownika który jest opiekunem kierunku lub jest super administratorem");
                    }
                }
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }


        private void LoadDepartments()
        {
            var currrentEmployee = _employeesProvider.GetEmployee(User.Identity.Name);
            IEnumerable<Department> departments = null;

            if (currrentEmployee.Root)
                departments = _departmetmentsProvider.GetAllDepartments();
            else
                departments = _departmetmentsProvider.GetAllDepartments().Where(n => n.Id == currrentEmployee.Course.Department.Id);

            var courses = departments.First().Courses;
            ViewBag.Department = departments.Select(n => new { Text = n.Name, Value = n.Id });
            ViewBag.Course = courses.Select(n => new { Text = n.Name, Value = n.Id });
        }
    }
}
