using FluentNHibernate.Mapping;
using Model.System;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class UniversityMap : ClassMap<University>
    {
        public UniversityMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
            Map(n => n.Address);
        }
    }
}
