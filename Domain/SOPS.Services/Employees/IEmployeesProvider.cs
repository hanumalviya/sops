using Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Employees
{
    public interface IEmployeesProvider
    {
        IList<Employee> GetEmployees(int departmentId, string search);
        IList<Employee> GetEmployees();

        IList<Employee> GetEmployees(string filter);

        Employee GetEmployee(int id);
        Employee GetEmployee(string userName);
    }
}
