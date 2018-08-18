using Model.Employees;
using Model.Students;
using Model.System;
using System;
using System.Linq;

namespace SOPS.Services.Contracts
{
    public interface IContractGenerator
    {
        void Generate(string templatePath, string saveFilePath, Student student, University university, Employee employee);
    }
}
