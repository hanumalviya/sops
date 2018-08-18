using Model.University;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Courses
{
    public interface ICoursesProvider
    {
        IList<Course> GetCourses();
        Course GetCourse(int id);
        Course GetCourse(string courseName);
    }
}
