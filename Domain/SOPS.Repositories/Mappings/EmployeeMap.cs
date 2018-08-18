using FluentNHibernate.Mapping;
using Model.Employees;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("UserProfile");
            Id(n => n.Id);
            References(n => n.Course);
            Map(n => n.UserName);
            Map(n => n.FirstName);
            Map(n => n.LastName);
            Map(n => n.Email);            
            Map(n => n.Administrator);
            Map(n => n.Keeper);
            Map(n => n.Moderator);
            Map(n => n.Root);
        }
    }
}
