using FluentNHibernate.Mapping;
using Model.Companies;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(n => n.Id);
            HasMany(n => n.Offers).Inverse().Cascade.All();
            HasMany(n => n.Contracts).Inverse();
            Map(n => n.Name);
            Map(n => n.Url);
            Map(n => n.Street);
            Map(n => n.Email);
            Map(n => n.Phone);
            Map(n => n.City);
            Map(n => n.PostalCode);
            Map(n => n.Description).CustomSqlType("nvarchar(500)");
        }
    }
}
