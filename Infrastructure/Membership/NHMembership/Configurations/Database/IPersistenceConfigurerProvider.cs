using FluentNHibernate.Cfg.Db;

namespace NHMembership.Configurations.Database
{
    public interface IPersistenceConfigurerProvider
    {
        IPersistenceConfigurer CreatePersistenceConfigurer(string connectionString);
    }
}