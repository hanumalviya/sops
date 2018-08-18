using Model.University;
using System;
using System.Linq;

namespace SOPS.Services.Departments
{
    public interface IDepartmentUpdater
    {
        void Update(Department department);
    }
}
