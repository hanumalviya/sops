using FluentNHibernate.Mapping;
using NHMembership.Models;

namespace NHMembership.Mappings
{
    internal class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(m => m.Id);
            Map(m => m.Name).Not.Nullable();
            Map(m => m.ApplicationName).Not.Nullable();
            HasManyToMany(m => m.UsersInRole).Inverse().Table("UsersInRoles");
        }
    }
}