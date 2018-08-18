using Model.Employees;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class EmployeeRepository : PersistRepository<int, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ISession session) :
            base(session)
        {

        }
    }
}
