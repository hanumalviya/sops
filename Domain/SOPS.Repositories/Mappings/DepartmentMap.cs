using FluentNHibernate.Mapping;
using Model.University;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
            HasMany(n => n.Courses).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
