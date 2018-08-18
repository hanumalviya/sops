using Model.University;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.System
{
    public class Template
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string FilePath { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual string ContentType { get; set; }

        public Template()
        {
            this.Courses = new List<Course>();
        }        
    }
}
