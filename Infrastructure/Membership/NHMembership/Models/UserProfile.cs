using System.Collections.Generic;

namespace NHMembership.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            Roles = new List<Role>();
        }

        public virtual int Id { get; protected set; }
        public virtual string UserName { get; set; }
        public virtual UserMembership Membership { get; set; }
        public virtual IList<Role> Roles { get; protected set; }

        public virtual void AddRole(Role role)
        {
            if (Roles.Contains(role) == false)
            {
                role.UsersInRole.Add(this);
                Roles.Add(role);
            }
        }

        public virtual void RemoveRole(Role role)
        {
            if (Roles.Contains(role))
            {
                if (role.UsersInRole.Contains(this))
                {
                    role.UsersInRole.Remove(this);
                }

                Roles.Remove(role);
            }
        }
    }
}