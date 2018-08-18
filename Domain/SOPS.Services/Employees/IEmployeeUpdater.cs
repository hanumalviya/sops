using Model.Employees;
using System;
using System.Linq;

namespace SOPS.Services.Employees
{
    public interface IEmployeeUpdater
    {
        void Update(Employee employee);
    }
}
