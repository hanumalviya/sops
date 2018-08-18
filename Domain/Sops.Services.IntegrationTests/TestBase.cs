using NHibernate;
using NHibernateRepository.UnitOfWork;
using NHMembership.Services;
using SOPS.Repositories.Factory;
using SOPS.Services.Companies;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Offers;
using SOPS.Services.Students;
using SOPS.Services.System;
using SOPS.Services.Templates;
using System;
using System.Linq;

namespace Sops.Services.IntegrationTests
{
    public abstract class TestBase
    {
        protected ISessionFactory sessionFactory;
        protected IAuthenticationService authenticationService;
        protected IRepositoriesFactory repositoriesFactory;
        protected UnitOfWork unitOfWork;

        protected StudentCreator studentsCreator;
        protected StudentsProvider studentsProvider;
        protected StudentDestructor studentDestructor;
        protected CsvStudentsImporter studentImporter;

        protected ModesCreator modesCreator;
        protected ModesProvider modesProvider;

        protected CourseCreator courseCreator;
        protected CoursesProvider courseProvider;
        protected CourseUpdater courseUpdater;
        protected CourseDestructor courseDestructor;

        protected DepartmentCreator departmentCreator;
        protected DepartmentsProvider departmentProvider;
        protected DepartmentUpdater departmentUpdater;
        protected DepartmentDestructor departmentDestructor;

        protected EmployeeDestructor employeesDestructor;
        protected EmployeesProvider employeesProvider;        
        protected EmployeeCreator employeeCreator;
        protected EmployeeUpdater employeesUpdater;

        protected TemplateCreator templateCreator;
        protected TemplateDestructor templateDestructor;
        protected TemplatesProvider templateProvider;

        protected CompanyCreator companyCreator;
        protected CompanyUpdater companyUpdater;
        protected CompanyDestructor companyDestructor;
        protected CompaniesProvider companiesProvider;
        
        protected OfferCreator offerCreator;
        protected OfferUpdater offerUpdater;
        protected OfferDestructor offerDestructor;
        protected OffersProvider offerProvider;

        protected OfferTypeCreator offerTypeCreator;
        protected OfferTypeProvider offerTypeProvider;
        protected UniversityDetailsCreator universityDetailsCreator;
        protected UniversityDetailsUpdater universityUpdater;
        protected UniversityDetailsProvider universityDetailsProvider;
        


        public TestBase()
        {
            InitDb(); 
        }

        public void InitDb()
        {            
            this.authenticationService = Config.AuthenticationService();
            this.sessionFactory = Config.ConfigureDatabase();
            this.repositoriesFactory = new RepositoriesFactory();
        }

        public virtual void SetUp()
        {
            unitOfWork = new UnitOfWork(sessionFactory);
            studentsCreator = new StudentCreator(unitOfWork, repositoriesFactory);
            studentsProvider = new StudentsProvider(unitOfWork, repositoriesFactory);
            studentDestructor = new StudentDestructor(unitOfWork, repositoriesFactory);
            studentImporter = new CsvStudentsImporter(studentsCreator);

            modesCreator = new ModesCreator(unitOfWork, repositoriesFactory);
            modesProvider = new ModesProvider(unitOfWork, repositoriesFactory);

            companyCreator = new CompanyCreator(unitOfWork, repositoriesFactory);
            companyUpdater = new CompanyUpdater(unitOfWork, repositoriesFactory);
            companyDestructor = new CompanyDestructor(unitOfWork, repositoriesFactory);
            companiesProvider = new CompaniesProvider(unitOfWork, repositoriesFactory);

            offerCreator = new OfferCreator(unitOfWork, repositoriesFactory);
            offerUpdater = new OfferUpdater(unitOfWork, repositoriesFactory);
            offerDestructor = new OfferDestructor(unitOfWork, repositoriesFactory);
            offerProvider = new OffersProvider(unitOfWork, repositoriesFactory);

            courseCreator = new CourseCreator(unitOfWork, repositoriesFactory);
            courseProvider = new CoursesProvider(unitOfWork, repositoriesFactory);
            courseUpdater = new CourseUpdater(unitOfWork, repositoriesFactory, authenticationService);
            courseDestructor = new CourseDestructor(unitOfWork, repositoriesFactory);

            departmentCreator = new DepartmentCreator(unitOfWork, repositoriesFactory);
            departmentProvider = new DepartmentsProvider(unitOfWork, repositoriesFactory);
            departmentDestructor = new DepartmentDestructor(unitOfWork, repositoriesFactory);
            departmentUpdater = new DepartmentUpdater(unitOfWork, repositoriesFactory);

            employeesDestructor = new EmployeeDestructor(unitOfWork, this.repositoriesFactory, this.authenticationService);
            employeesProvider = new EmployeesProvider(unitOfWork, repositoriesFactory);
            employeeCreator = new EmployeeCreator(unitOfWork, this.authenticationService, this.repositoriesFactory);
            employeesUpdater = new EmployeeUpdater(unitOfWork, this.repositoriesFactory, this.authenticationService);

            templateCreator = new TemplateCreator(unitOfWork, repositoriesFactory);
            templateDestructor = new TemplateDestructor(unitOfWork, repositoriesFactory);
            templateProvider = new TemplatesProvider(unitOfWork, repositoriesFactory);

            offerTypeCreator = new OfferTypeCreator(unitOfWork, repositoriesFactory);
            offerTypeProvider = new OfferTypeProvider(unitOfWork, repositoriesFactory);

            universityDetailsCreator = new UniversityDetailsCreator(unitOfWork, repositoriesFactory);
            universityUpdater = new UniversityDetailsUpdater(unitOfWork, repositoriesFactory);
            universityDetailsProvider = new UniversityDetailsProvider(unitOfWork, repositoriesFactory);
        }

        public virtual void TearDown()
        {
            unitOfWork.Dispose();
        }
    }
}
