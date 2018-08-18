using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOPS.Services.Contracts;
using SOPS.Services.Courses;
using SOPS.Services.Employees;
using SOPS.Services.Students;
using SOPS.WebUI.Areas.Administration.Utilities;
using SOPS.WebUI.Areas.Administration.ViewModels.Students;
using Model.Students;
using SOPS.Services.System;
using Model.Employees;
using System.IO;
using SOPS.Services.Companies;
using Model.Companies;


namespace SOPS.WebUI.Areas.Administration.Controllers
{
    [Authorize(Roles = EmployeesRoles.Keeper)]
    public class StudentsController : Controller
    {
        private readonly IStudentsProvider _studentsProvider;
        private readonly IStudentCreator _studentCreator;
        private readonly IStudentDestructor _studentDestructor;
        private readonly IStudentUpdater _studentUpdater;
        private readonly IEmployeesProvider _employeesProvider;
        private readonly IModesProvider _modesProvider;
        private readonly IStudentsImporter _studentsImporter;
        private readonly IContractProvider _contractProvider;
        private readonly IContractUpdater _contractUpdater;
        private readonly IContractGenerator _contractGenerator;
        private readonly IUniversityDetailsProvider _universityDetailsProvider;
        private readonly ICoursesProvider _coursesProvider;
        private readonly ICompaniesProvider _companiesProvider;
        private readonly ICompanyCreator _companyCreator;

        public StudentsController(
            IStudentsProvider studentsProvider,
            IStudentCreator studentCreator,
            IStudentDestructor studentDestructor,
            IStudentUpdater studentUpdater,
            IEmployeesProvider employeesProvider,
            IModesProvider modesProvider,
            IStudentsImporter studentsImporter,
            IContractProvider contractProvider,
            IContractUpdater contractUpdater,
            IContractGenerator contractGenerator,
            IUniversityDetailsProvider universityDetailsProvider,
            ICompaniesProvider companiesProvider,
            ICoursesProvider coursesProvider,
            ICompanyCreator companyCreator)
        { 
            _studentsProvider = studentsProvider;
            _studentCreator = studentCreator;
            _studentDestructor = studentDestructor;
            _studentUpdater = studentUpdater;
            _employeesProvider = employeesProvider;
            _modesProvider = modesProvider;
            _studentsImporter = studentsImporter;
            _contractProvider = contractProvider;
            _contractUpdater = contractUpdater;
            _contractGenerator = contractGenerator;
            _universityDetailsProvider = universityDetailsProvider;
            _coursesProvider = coursesProvider;
            _companiesProvider = companiesProvider;
            _companyCreator = companyCreator;
        }

        public ActionResult Index()
        {
            ViewBag.Mode = _modesProvider.GetModes().Select(n => new { Text = n.Name, Value = n.Id }); 
            return View();
        }

        public JsonResult DataTable(string search)
        {
            int currentEmployeeCourseId = _employeesProvider.GetEmployee(User.Identity.Name).Course.Id;

            var list = _studentsProvider.GetStudents(currentEmployeeCourseId, search)
                .Select(student => new ListItemViewModel()
                {
                    Album = student.Album,
                    Id = student.Id,
                    Name = string.Format("{0} {1}", student.FirstName, student.LastName)
                }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult DetailsPartial()
        {
            ViewBag.Mode = _modesProvider.GetModes().Select(n => new { Text = n.Name, Value = n.Id }); 

            return PartialView("Partials/_details", new StudentViewModel());
        }

        public JsonResult Details(int id)
        {
            Student student = _studentsProvider.GetStudent(id);
            StudentViewModel viewModel = student.ToViewModel();

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Add()
        {
            var currentEmployee = _employeesProvider.GetEmployee(User.Identity.Name);
            var department = currentEmployee.Course.Department;
            var course =  currentEmployee.Course;

            var viewModel = new StudentViewModel
                {
                    Department = department.Name,
                    Course = course.Name
                };

            ViewBag.Mode = _modesProvider.GetModes().Select(n => new { Text = n.Name, Value = n.Id }); 

            return PartialView("Dialogs/add-student-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Add(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                var course = _coursesProvider.GetCourse(student.Course);
                var mode = _modesProvider.GetModes().Single(n => n.Id == student.Mode);

                _studentCreator.Create(student.Name,
                    student.LastName,
                    student.Album,
                    course,
                    mode,
                    student.Email,
                    student.Phone,
                    student.City,
                    student.Street,
                    student.PostalCode);
                                
                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [ChildActionOnly]
        public ActionResult Edit()
        {
            var viewModel = new StudentViewModel();
            ViewBag.Mode = _modesProvider.GetModes().Select(n => new { Text = n.Name, Value = n.Id }); 

            return PartialView("Dialogs/edit-student-dialog", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                var mode = _modesProvider.GetModes().Single(n => n.Id == student.Mode);

                var s = _studentsProvider.GetStudent(student.Id);
                s.Address = student.Street;
                s.Album = student.Album;
                s.City = student.City;
                s.Email = student.Email;
                s.FirstName = student.Name;
                s.LastName = student.LastName;
                s.Mode = mode;
                s.Phone = student.Phone;
                s.PostalCode = student.PostalCode;

                _studentUpdater.Update(s);
                
                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                _studentDestructor.Destroy(id);
                
                return Json(true);
            }

            return PartialView("Partials/_FormSubmitResult", ModelState);
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file, int mode)
        {
            if (file != null && file.ContentLength > 0)
            {
                var m = _modesProvider.GetModes().Single(n => n.Id == mode);
                var c = _employeesProvider.GetEmployee(User.Identity.Name).Course;
                _studentsImporter.Import(file.InputStream, c, m);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Contract(int id)
        {
            var student = _studentsProvider.GetStudent(id);
            var contract = student.Contract;

            var companies = _companiesProvider.GetCompanies(string.Empty).Select(n => new { Text = n.Name, Value = n.Id }).ToList();
            companies.Insert(0, null);

            ViewBag.Company = companies;
            ViewBag.ContractId = id;

            var viewModel = new Tuple<StudentViewModel, ContractViewModel>(student.ToViewModel(), contract.ToViewModel());



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ContractUpdate(ContractViewModel contract)
        {
            if (ModelState.IsValid)
            {
                Company company = null;

                if (contract.CompanyId.HasValue)
                    company = _companiesProvider.GetCompany(contract.CompanyId.Value);                

                var c = _contractProvider.GetContract(contract.Id);
                c.StartDate = contract.StartDate?? DateTime.Now;
                c.UniversityRepresentative = contract.UniversityRepresentative;
                c.Company = company;
                c.CompanyRepresentative = contract.CompanyRepresentative;
                c.EndDate = contract.EndDate ?? DateTime.Now;

                _contractUpdater.Update(c);                
            }

            return RedirectToAction("Contract", new { id = contract.Id});
        }

        [HttpPost]
        public FileResult Download(int id)
        {
            var student = _studentsProvider.GetStudent(id);
            var course = student.Course;
            var university = _universityDetailsProvider.GetUniversity();
            var employee = _employeesProvider.GetEmployee(User.Identity.Name);
            var template = course.Template;
            string contentType = course.Template.ContentType;

            var path = Path.Combine(Server.MapPath("~/App_Data/templates"), template.FilePath);
            string fileName = string.Format("{0} {1}.{2}.docx", student.FirstName, student.LastName, course.Name);
            var contractPath = Path.Combine(Server.MapPath("~/App_Data/contracts"), fileName);

            _contractGenerator.Generate(path, contractPath, student, university, employee);

            return File(contractPath, contentType, fileName);
        }

        [HttpPost]
        public ActionResult AddCompany(int contractId, SOPS.WebUI.Areas.Administration.ViewModels.Companies.CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                _companyCreator.Create(company.Name, company.Street, company.City, company.PostalCode, company.Email, company.Phone, company.Url, company.Description);
            }

            return RedirectToAction("Contract", new { id = contractId });
        }
    }
}
