using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using SOPS.Repositories.Implementation;
using System;
using System.Linq;

namespace SOPS.Repositories
{
    public static class Database
    {
        public static ISessionFactory CreateConfiguration(IPersistenceConfigurer persistenceConfigurer,
            Action<Configuration> exposeConfiguration)
        {
            ISessionFactory factory = Fluently.Configure()
                .Database(persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CompanyRepository>())
                .ExposeConfiguration(exposeConfiguration)
                .BuildSessionFactory();

            return factory;
        }
    }
}
