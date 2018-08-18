using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHMembership.Logging;
using NHMembership.Security.Encryption.BCrypt;
using NHMembership.Services;
using SOPS.Repositories;
using System.Linq;

namespace Sops.Services.IntegrationTests
{
    public static class Config
    {
        static bool configured = false;
        static IAuthenticationService service = null;
        public static IAuthenticationService AuthenticationService()
        {
            if (configured == false)
            {
                string applicationName = "sops";

                IPersistenceConfigurer persistenceConfigurer =
                    MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));

                service = new FormsAuthenticationService(applicationName,
                    new BCryptStrategy(),
                    new ConsoleLogger(),
                    persistenceConfigurer,
                    configuration =>
                    {
                        var se = new SchemaExport(configuration);
                        se.Drop(true, true);
                        se.Execute(true, true, true);

                        var su = new SchemaUpdate(configuration);
                        su.Execute(true, true);
                    });
                service.Configure();
                configured = true;
            }

            return service;
        }

        public static ISessionFactory ConfigureDatabase()
        {
            IPersistenceConfigurer persistenceConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));
            return Database.CreateConfiguration(persistenceConfigurer,
                configuration =>
                {
                    var se = new SchemaExport(configuration);
                    se.Drop(true, true);
                    se.Execute(true, true, true);

                    var su = new SchemaUpdate(configuration);
                    su.Execute(true, true);
                });
        }
    }
}