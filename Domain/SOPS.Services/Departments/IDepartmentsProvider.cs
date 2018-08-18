using System;
using System.Collections.Generic;
using System.Linq;
using Model.University;

namespace SOPS.Services.Departments
{
    public interface IDepartmentsProvider
    {
        IList<Department> GetAllDepartments();

        Department GetDepartment(int id);
        Department GetDepartment(string name);
    }
}
