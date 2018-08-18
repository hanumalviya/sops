using Model.Employees;
using Model.University;
using System;
using System.Linq;

namespace SOPS.Services.Courses
{
    public interface ICourseUpdater
    {
        void Update(Course course);
        void SetManager(Course course, Employee emp);
    }
}
