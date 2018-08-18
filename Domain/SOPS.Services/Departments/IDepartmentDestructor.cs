using System;
using System.Linq;

namespace SOPS.Services.Departments
{
    public interface IDepartmentDestructor
    {
        void Destroy(int id);

        bool CanBeDestroyed(int id);
    }
}
