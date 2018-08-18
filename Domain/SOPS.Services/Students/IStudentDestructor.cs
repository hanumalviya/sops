using System;
using System.Linq;

namespace SOPS.Services.Students
{
    public interface IStudentDestructor
    {
        void Destroy(int id);
        void DestroyAllStudents();
    }
}
