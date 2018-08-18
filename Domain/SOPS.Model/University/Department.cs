using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.University
{
    public class Department
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<Course> Courses { get; protected set; }

        public Department()
        {
            Courses = new List<Course>();
        }
    }
}
