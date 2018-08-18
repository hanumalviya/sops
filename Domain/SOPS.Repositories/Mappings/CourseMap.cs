using FluentNHibernate.Mapping;
using Model.University;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class CourseMap : ClassMap<Course>
    {
        public CourseMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
            References(n => n.Department);
            References(n => n.Manager).Column("ManagerId");
            HasMany(n => n.Students).Inverse().Cascade.All();
            References(n => n.Template);
        }
    }
}
