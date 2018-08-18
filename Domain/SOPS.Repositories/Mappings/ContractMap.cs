using FluentNHibernate.Mapping;
using Model.Students;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class ContractMap : ClassMap<Contract>
    {
        public ContractMap()
        {
            Id(n => n.Id);
            References(n => n.Company);
            Map(n => n.CompanyRepresentative);
            Map(n => n.UniversityRepresentative);
            Map(n => n.StartDate);
            Map(n => n.EndDate);
        }
    }
}
