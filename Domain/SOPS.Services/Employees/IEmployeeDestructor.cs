using System;
using System.Linq;

namespace SOPS.Services.Employees
{
    public interface IEmployeeDestructor
    {
        void Destroy(int id);

        bool CanBeDestroyed(int id, int currentEmployee);
    }
}
