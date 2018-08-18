using System.Collections.Generic;

namespace NHMembership.Models
{
    public class Role
    {
        public Role()
        {
            UsersInRole = new List<UserProfile>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual IList<UserProfile> UsersInRole { get; protected set; }
    }
}