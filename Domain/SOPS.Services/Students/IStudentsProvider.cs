using System;
using System.Collections.Generic;
using System.Linq;
using Model.Students;

namespace SOPS.Services.Students
{
    public interface IStudentsProvider
    {
        IList<Student> GetStudents(int courseId, string filter);
        IList<Student> GetStudents(int courseId);

        Student GetStudent(int id);
    }
}
