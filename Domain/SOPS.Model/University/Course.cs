using System;
using System.Linq;
using Model.Employees;
using Model.System;
using Model.Students;
using System.Collections.Generic;

namespace Model.University
{
    public class Course
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Department Department { get; set; }
        
        public virtual Template Template { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual IList<Student> Students { get; set; }

        public Course()
        {
            Students = new List<Student>();
        }

        public virtual void SetDepartment(Department department)
        {
            if (this.Department != null)
            {
                Department.Courses.Remove(this);
            }

            this.Department = department;

            if (department != null)
            {
                department.Courses.Add(this);
            }
        }

        public virtual void SetTemplate(Template template)
        {
            if (this.Template != null)
            {
                Template.Courses.Remove(this);
            }

            this.Template = template;

            if (template != null)
            {
                template.Courses.Add(this);
            }
        }
    }
}
