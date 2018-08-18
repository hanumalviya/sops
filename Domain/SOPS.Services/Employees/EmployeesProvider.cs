using System;
using System.Collections.Generic;
using System.Linq;
using Model.Employees;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using SOPS.Services.Utilities;

namespace SOPS.Services.Employees
{
    public class EmployeesProvider : IEmployeesProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Employee> GetEmployees()
        {
            var repository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
            var results = repository.All().ToList();

            return results;
        }

        public IList<Employee> GetEmployees(string filter)
        {
            var repository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);

            var results = repository.All().ToList().Where(n => n.FirstName.InsensitiveContains(filter) ||
                n.LastName.InsensitiveContains(filter) ||
                n.UserName.InsensitiveContains(filter) ||
                n.Course.Name.InsensitiveContains(filter) ||
                n.Email.InsensitiveContains(filter)).ToList();

            return results;
        }

        public IList<Employee> GetEmployees(int departmentId, string filter)
        {
            var repository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);

            var results = repository.FilterBy(x => x.Course.Department.Id == departmentId).ToList()
                .Where(n => n.FirstName.InsensitiveContains(filter) ||
                n.LastName.InsensitiveContains(filter) ||
                n.UserName.InsensitiveContains(filter) ||
                n.Email.InsensitiveContains(filter)).ToList();

            return results;
        }

        public Employee GetEmployee(int id)
        {
            var repository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
            return repository.FindBy(id);
        }

        public Employee GetEmployee(string userName)
        {            
            var repository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
            return repository.FindBy(n => n.UserName == userName);
        }
    }
}
