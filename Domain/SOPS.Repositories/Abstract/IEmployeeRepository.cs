using Model.Employees;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface IEmployeeRepository : IPersistRepository<Employee>, IReadOnlyRepository<int, Employee>
    {
    }
}
