using Model.Employees;
using Model.Offers;
using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Services.Companies;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Offers;
using SOPS.Services.Students;
using SOPS.Services.System;
using SOPS.Services.Templates;
using System;
using System.IO;
using System.Linq;

namespace SampleDataGenerator
{
    public partial class Generator
    {
        public void Generate()
        {
            PrepareData();

            using (var unitOfWork = new UnitOfWork(_sessionFactory))
            {
                var departmentCreator = new DepartmentCreator(unitOfWork, _repositoriesFactory);
                var courseCreator = new CourseCreator(unitOfWork, _repositoriesFactory);
                var courseUpdater = new CourseUpdater(unitOfWork, _repositoriesFactory, _auth);
                var employeesCreator = new EmployeeCreator(unitOfWork, _auth, _repositoriesFactory);
                var studentCreator = new StudentCreator(unitOfWork, _repositoriesFactory);
                var studentsImporter = new CsvStudentsImporter(studentCreator);
                var modesProvider = new ModesProvider(unitOfWork, _repositoriesFactory);

                var mode = modesProvider.GetModes().First();

                EmployeeCreateStatus status;
                Course rootCourse = null;

                var departments = _document.Descendants("department").ToList();

                foreach (var d in departments)
                {
                    var dName = d.Attribute("name").Value;                    
                    var deparment = departmentCreator.Create(dName);

                    var courses = d.Descendants("course").ToList();
                    foreach (var c in courses)
                    {
                        var cName = c.Attribute("name").Value;
                        Console.WriteLine(cName);
                        var course = courseCreator.Create(cName, deparment);

                        if (rootCourse == null)
                        {
                            rootCourse = course;
                        }

                        var employees = c.Descendants("employee").ToList();
                        foreach (var e in employees)
                        {
                            var userName = e.Attribute("name").Value;
                            var manager = bool.Parse(e.Attribute("manager").Value);
                            var admin = bool.Parse(e.Attribute("administrator").Value);
                            var moderator = bool.Parse(e.Attribute("moderator").Value);

                            var firstName = userName.Split('.')[0];
                            var lastName = userName.Split('.')[1];
                            var mail = string.Format("{0}@mailinator.com", userName);
                            var employee = employeesCreator.Create(userName, firstName, lastName, "qwerty", mail, "qwerty", "qwerty", moderator, false, admin, course, out status);

                            if (manager)
                            {
                                courseUpdater.SetManager(course, employee);
                            }
                        }

                        using(var streamReader = new StreamReader("Data/studenci.txt"))
	                    {
                            studentsImporter.Import(streamReader.BaseStream, course, mode);
	                    }
                    }
                }
                
                employeesCreator.Create("root", "Jan", "Kowalski", "qwerty", "root@mailinator.com", "qwerty", "qwerty", true, true, true, rootCourse, out status);

                Console.WriteLine("\n\nOferty\n\n");
                var companyCreator = new CompanyCreator(unitOfWork, _repositoriesFactory);
                var offerTypesProvider = new OfferTypeProvider(unitOfWork, _repositoriesFactory);
                var offerCreator = new OfferCreator(unitOfWork, _repositoriesFactory);

                var companies = _document.Descendants("company").ToList();
                int counter = 0;
                foreach (var c in companies)
                {
                    counter++;

                    var companyName = c.Attribute("name").Value;
                    var city = c.Attribute("city").Value;
                    var url = string.Format("http://www.{0}.pl", companyName);
                    string postalCode = string.Format("43-3{0}",(counter + 5) * 11);
                    var street = string.Format("ul. Przykładowa {0}",  (counter + 3) * 14);
                    var description = string.Format("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.");
                    var email = string.Format("{0}@mailinator.com", companyName);
                    var phone = string.Format("{0}264{1}24{2}{3}", counter + 1, counter, counter, counter + 1);

                    Console.WriteLine(companyName);

                    var company = companyCreator.Create(companyName, street, city, postalCode, email, phone, url, description);

                    var offers = c.Descendants("offer").ToList();
                    foreach (var o in offers)
                    {
                        var offerName = o.Attribute("name").Value;
                        var trade = o.Attribute("trade").Value;
                        OfferType type = null;

                        if (o.Attribute("type").Value == "0")
                            type = offerTypesProvider.GetAllOfferTypes().Single(n => n.Name.ToLower() == "praktyka");
                        else
                            type = offerTypesProvider.GetAllOfferTypes().Single(n => n.Name.ToLower() == "staż");

                        offerCreator.Create(offerName, description, trade, company, type);
                    }
                }
            } 
        }

        public void PrepareData()
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

        public void CreteRoles()
        {
            _auth.RoleService.AddRole(EmployeesRoles.Keeper);
            _auth.RoleService.AddRole(EmployeesRoles.Moderator);
            _auth.RoleService.AddRole(EmployeesRoles.Administrator);
            _auth.RoleService.AddRole(EmployeesRoles.Root);
        }

        public void CreateDefaultTemplate()
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
