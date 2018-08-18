using FluentNHibernate.Mapping;
using NHMembership.Models;

namespace NHMembership.Mappings
{
    internal class UserProfileMap : ClassMap<UserProfile>
    {
        public UserProfileMap()
        {
            Id(m => m.Id);
            Map(m => m.UserName).Not.Nullable();
            HasOne(m => m.Membership).Cascade.All();
            HasManyToMany(m => m.Roles).Table("UsersInRoles");
        }
    }
}