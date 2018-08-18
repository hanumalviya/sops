using Model.Students;
using System;
using System.Linq;

namespace SOPS.Services.Students
{
    public interface IStudentUpdater
    {
        void Update(Student student);
    }
}
