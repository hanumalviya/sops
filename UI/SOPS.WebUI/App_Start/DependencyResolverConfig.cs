using System.Net.Mail;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using FluentNHibernate.Cfg.Db;
using NHMembership.Services;
using SOPS.Services.Contracts;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Students;
using SOPS.Services.Companies;
using SOPS.Services.Offers;
using SOPS.Services.Templates;
using SOPS.Services.System;
using SOPS.Services.Documents;
using NHMembership.Logging;
using SOPS.Services.Mail;
using System.Configuration;
using MailingService.Mailsender;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHMembership.Security.Encryption.BCrypt;

namespace SOPS.WebUI
{
    public static class DependencyResolverConfig
    {
        public static void RegisterDependencyResolver()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            RegisterDependencies(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            // authentication
            IAuthenticationService authenticationService = AuthenticationService();
            builder.Register(c => authenticationService).As<IAuthenticationService>().SingleInstance();         
   
            // mail
            MailService mailService = new MailService(ReadFromConfig());
            builder.Register(n => mailService).As<IMailService>().InstancePerHttpRequest();

            // repositories
            var sessionFactory = ConfigureDatabase();
            builder.Register(n => new UnitOfWork(sessionFactory)).As<IUnitOfWork>().InstancePerHttpRequest();

            builder.RegisterType<RepositoriesFactory>().As<IRepositoriesFactory>().InstancePerHttpRequest();
            

            // students
            builder.RegisterType<StudentsProvider>().As<IStudentsProvider>().InstancePerHttpRequest();
            builder.RegisterType<StudentCreator>().As<IStudentCreator>().InstancePerHttpRequest();
            builder.RegisterType<StudentDestructor>().As<IStudentDestructor>().InstancePerHttpRequest();
            builder.RegisterType<StudentUpdater>().As<IStudentUpdater>().InstancePerHttpRequest();
            builder.RegisterType<CsvStudentsImporter>().As<IStudentsImporter>().InstancePerHttpRequest();

            // employees
            builder.RegisterType<EmployeesProvider>().As<IEmployeesProvider>().InstancePerHttpRequest();
            builder.RegisterType<EmployeeCreator>().As<IEmployeeCreator>().InstancePerHttpRequest();
            builder.RegisterType<EmployeeDestructor>().As<IEmployeeDestructor>().InstancePerHttpRequest();
            builder.RegisterType<EmployeeUpdater>().As<IEmployeeUpdater>().InstancePerHttpRequest();

            // offers
            builder.RegisterType<OffersProvider>().As<IOffersProvider>().InstancePerHttpRequest();
            builder.RegisterType<OfferCreator>().As<IOfferCreator>().InstancePerHttpRequest();
            builder.RegisterType<OfferDestructor>().As<IOfferDestructor>().InstancePerHttpRequest();
            builder.RegisterType<OfferUpdater>().As<IOfferUpdater>().InstancePerHttpRequest();
            builder.RegisterType<OfferTypeProvider>().As<IOfferTypeProvider>().InstancePerHttpRequest();

            // companies
            builder.RegisterType<CompaniesProvider>().As<ICompaniesProvider>().InstancePerHttpRequest();
            builder.RegisterType<CompanyCreator>().As<ICompanyCreator>().InstancePerHttpRequest();
            builder.RegisterType<CompanyDestructor>().As<ICompanyDestructor>().InstancePerHttpRequest();
            builder.RegisterType<CompanyUpdater>().As<ICompanyUpdater>().InstancePerHttpRequest();

            // department
            builder.RegisterType<ModesProvider>().As<IModesProvider>().InstancePerHttpRequest();
            builder.RegisterType<DepartmentsProvider>().As<IDepartmentsProvider>().InstancePerHttpRequest();
            builder.RegisterType<DepartmentCreator>().As<IDepartmentCreator>().InstancePerHttpRequest();
            builder.RegisterType<DepartmentDestructor>().As<IDepartmentDestructor>().InstancePerHttpRequest();
            builder.RegisterType<DepartmentUpdater>().As<IDepartmentUpdater>().InstancePerHttpRequest();

            // courses
            builder.RegisterType<CoursesProvider>().As<ICoursesProvider>().InstancePerHttpRequest();
            builder.RegisterType<CourseCreator>().As<ICourseCreator>().InstancePerHttpRequest();
            builder.RegisterType<CourseDestructor>().As<ICourseDestructor>().InstancePerHttpRequest();
            builder.RegisterType<CourseUpdater>().As<ICourseUpdater>().InstancePerHttpRequest();
            builder.RegisterType<UniversityDetailsProvider>().As<IUniversityDetailsProvider>().InstancePerHttpRequest();
            builder.RegisterType<UniversityDetailsUpdater>().As<IUniversityDetailsUpdater>().InstancePerHttpRequest();
            
            // contract
            builder.RegisterType<ContractProvider>().As<IContractProvider>().InstancePerHttpRequest();
            builder.RegisterType<ContractUpdater>().As<IContractUpdater>().InstancePerHttpRequest();
            builder.RegisterType<ContractGenerator>().As<IContractGenerator>().InstancePerHttpRequest();

            // system
            builder.RegisterType<TemplatesProvider>().As<ITemplatesProvider>().InstancePerHttpRequest();
            builder.RegisterType<TemplateCreator>().As<ITemplateCreator>().InstancePerHttpRequest();
            builder.RegisterType<TemplateDestructor>().As<ITemplateDestructor>().InstancePerHttpRequest();
            builder.RegisterType<DocumentsProvider>().As<IDocumentsProvider>().InstancePerHttpRequest();
            builder.RegisterType<DocumentDestructor>().As<IDocumentDestructor>().InstancePerHttpRequest();
            builder.RegisterType<DocumentCreator>().As<IDocumentCreator>().InstancePerHttpRequest();
        }

        private static ISessionFactory ConfigureDatabase()
        {
            IPersistenceConfigurer persistenceConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));
            return Database.CreateConfiguration(persistenceConfigurer, c =>
            {
                var u = new SchemaUpdate(c);
                u.Execute(true, true);
            });
        }

        private static IAuthenticationService AuthenticationService()
        {
            string applicationName = "sops";

            IPersistenceConfigurer persistenceConfigurer =
                MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));

            IAuthenticationService service = new FormsAuthenticationService(applicationName, new BCryptStrategy(), new ConsoleLogger(), persistenceConfigurer, c =>
            {
                var u = new SchemaUpdate(c);
                u.Execute(true, true);
            });

            service.Configure();

            return service;
        }

        public static MailConfiguration ReadFromConfig()
        {
            var conf = ConfigurationManager.AppSettings;

            if (conf.Count > 0)
            {
                var host = conf["host"];
                var port = conf["port"];
                var ssl = conf["ssl"];
                var username = conf["username"];
                var password = conf["password"];
                var emailfrom = conf["emailfrom"];

                return new MailConfiguration
                {
                    Host = host,
                    Port = int.Parse(port),
                    EnableSsl = bool.Parse(ssl),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    UserName = username,
                    Password = password,
                    EmailFrom = emailfrom
                };
            }

            return null;
        }
    }
}