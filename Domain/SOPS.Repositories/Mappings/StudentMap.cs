using FluentNHibernate.Mapping;
using Model.Students;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Id(n => n.Id);
            Map(n => n.FirstName);
            Map(n => n.LastName);
            Map(n => n.Album);
            Map(n => n.City);
            Map(n => n.Address);
            Map(n => n.PostalCode);
            Map(n => n.Email);
            Map(n => n.Phone);
            References(n => n.Course);
            References(n => n.Mode);
            References(n => n.Contract).Cascade.All();
        }
    }
}
