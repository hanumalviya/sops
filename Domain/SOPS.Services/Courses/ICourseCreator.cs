using Model.University;
using System;
using System.Linq;

namespace SOPS.Services.Courses
{
    public interface ICourseCreator
    {
        Course Create(string name, int departmentId);
        Course Create(string name, Department department);
    }
}
