using Model.Employees;
using NHibernateRepository.UnitOfWork;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Offers;
using SOPS.Services.System;
using SOPS.Services.Templates;
using System;
using System.Linq;

namespace InstallTool.Code
{
    public partial class Generator
    {
        public void Generate()
        {
            Console.WriteLine("\nGenerowanie danych\n");
            PrepareData();

            using (var unitOfWork = new UnitOfWork(_sessionFactory))
            {
                GenerateCourses(unitOfWork);
                CreateRoot(unitOfWork);
            }

            Console.WriteLine("\nSukces możesz się teraz zalogować w systemie\n");
        }

        private void CreateRoot(UnitOfWork unitOfWork)
        {
            var employeesCreator = new EmployeeCreator(unitOfWork, _auth, _repositoriesFactory);
            var coursesProvider = new CoursesProvider(unitOfWork, _repositoriesFactory);

            Console.WriteLine("\nTworzenie użytkownika root");
            Console.WriteLine("\nPodaj numer kierunku z listy do której przynależy użytkownik root:");
            int courseId = int.Parse(Console.ReadLine());
            var course = coursesProvider.GetCourse(courseId);

            Console.WriteLine("\nPodaj nazwę użytkownika do logowania:");
            string userName = Console.ReadLine();

            Console.WriteLine("\nPodaj hasło do logowania:");
            string password = Console.ReadLine();

            Console.WriteLine("\nPodaj adres email:");
            string email = Console.ReadLine();

            Console.WriteLine("\nPodaj imię:");
            string firstName = Console.ReadLine();

            Console.WriteLine("\nPodaj nazwisko:");
            string lastName = Console.ReadLine();

            Console.WriteLine("\nPodaj pytanie do przywracania hasła:");
            string question = Console.ReadLine();

            Console.WriteLine("\nPodaj odpowiedź na pytanie:");
            string answer = Console.ReadLine();

            EmployeeCreateStatus status = EmployeeCreateStatus.None;
            employeesCreator.Create(userName, firstName, lastName, password, email, question, answer, true, true, true, course, out status);
        }

        private void GenerateCourses(UnitOfWork unitOfWork)
        {
            var departmentCreator = new DepartmentCreator(unitOfWork, _repositoriesFactory);
            var courseCreator = new CourseCreator(unitOfWork, _repositoriesFactory);
            var departments = _document.Descendants("department").ToList();
            Console.WriteLine("\nLista utworzonych kierunków:\n");
            foreach (var d in departments)
            {
                var dName = d.Attribute("name").Value;
                var deparment = departmentCreator.Create(dName);

                var courses = d.Descendants("course").ToList();
                foreach (var c in courses)
                {
                    var cName = c.Attribute("name").Value;

                    var course = courseCreator.Create(cName, deparment);
                    Console.WriteLine("{0} {1}", course.Id, cName);
                }
            }
        }

        private void PrepareData()
        {
            CreteRoles();
            CreateDefaultTemplate();

            using (var unitOfWork = new UnitOfWork(_sessionFactory))
            {
                var modesCreator = new ModesCreator(unitOfWork, _repositoriesFactory);
                var universityDetailsCreator = new UniversityDetailsCreator(unitOfWork, _repositoriesFactory);
                var offerTypeCreator = new OfferTypeCreator(unitOfWork, _repositoriesFactory);

                modesCreator.Create("Stacjonarne");
                modesCreator.Create("Niestacjonarne");

                universityDetailsCreator.Create("Akademia Techniczno-Humanistyczna w Bielsku-Białej", "43-309 Bielsko-Biała ul. Willowa 2");

                offerTypeCreator.Create("Staż");
                offerTypeCreator.Create("Praktyka");
                offerTypeCreator.Create("Praca");
            }
        }

        private void CreteRoles()
        {
            _auth.RoleService.AddRole(EmployeesRoles.Keeper);
            _auth.RoleService.AddRole(EmployeesRoles.Moderator);
            _auth.RoleService.AddRole(EmployeesRoles.Administrator);
            _auth.RoleService.AddRole(EmployeesRoles.Root);
        }

        private void CreateDefaultTemplate()
        {
            using (var unitOfWork = new UnitOfWork(_sessionFactory))
            {
                string docXContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                var templateCreator = new TemplateCreator(unitOfWork, _repositoriesFactory);
                templateCreator.Create("Domyślny", "UMOWA.docx", docXContentType);
            }
        }
    }
}
