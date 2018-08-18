using Model.University;
using System;
using System.Linq;

namespace SOPS.Services.Departments
{
    public interface IDepartmentCreator
    {
        Department Create(string name);
    }
}
