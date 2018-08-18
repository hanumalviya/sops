using Model.University;
using System;
using System.Linq;

namespace Model.Employees
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual Course Course { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }        
        public virtual bool Administrator { get; set; }
        public virtual bool Keeper { get; set; }
        public virtual bool Moderator { get; set; }
        public virtual bool Root { get; set; }
    }

    public class EmployeesRoles
    {
        public const string Administrator = "ADMINISTRATOR";
        public const string Moderator = "MODERATOR";
        public const string Root = "ROOT";
        public const string Keeper = "KEEPER";
    }
}
