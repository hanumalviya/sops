using Model.University;
using System;
using System.Linq;

namespace Model.Students
{
    public class Student
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Album { get; set; }
        public virtual string City { get; set; }
        public virtual string Address { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual Course Course { get; set; }
        public virtual Mode Mode { get; set; }
        public virtual Contract Contract { get; set; }

        public Student()
        {
            Course = new Course();
            Mode = new Mode();
            Contract = new Contract();
        }
    }
}
