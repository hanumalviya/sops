using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHMembership.Logging;
using NHMembership.Security.Encryption.BCrypt;
using NHMembership.Services;
using SOPS.Repositories;
using SOPS.Repositories.Factory;
using System;
using System.Linq;
using System.Xml.Linq;

namespace InstallTool.Code
{
    public partial class Generator
    {
        private XDocument _document;
        private IAuthenticationService _auth;
        private ISessionFactory _sessionFactory;
        private RepositoriesFactory _repositoriesFactory;

        public Generator()
        {
            _document = XDocument.Load("Data/data.xml");
            Configure();
        }

        private void Configure()
        {
            _auth = AuthenticationService();
            _sessionFactory = ConfigureDatabase();
            _repositoriesFactory = new RepositoriesFactory();
        }

        private IAuthenticationService AuthenticationService()
        {
            string applicationName = "sops";

            IPersistenceConfigurer persistenceConfigurer =
                MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));

            IAuthenticationService service = new FormsAuthenticationService(applicationName, new BCryptStrategy(), new ConsoleLogger(), persistenceConfigurer, c =>
            {
                var se = new SchemaExport(c);
                se.Drop(false, true);
                se.Execute(false, true, true);

                var su = new SchemaUpdate(c);
                su.Execute(false, true);
            });
            service.Configure();

            return service;
        }

        private ISessionFactory ConfigureDatabase()
        {
            IPersistenceConfigurer persistenceConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));
            return Database.CreateConfiguration(persistenceConfigurer, c =>
            {
                var se = new SchemaExport(c);
                se.Drop(false, true);
                se.Execute(false, true, true);

                var su = new SchemaUpdate(c);
                su.Execute(false, true);
            });
        }
    }
}
