using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Departments
{
    public class DepartmentsProvider : IDepartmentsProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Department> GetAllDepartments()
        {
            var repository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
            return repository.All().ToList();
        }

        public Department GetDepartment(int id)
        {
            var repository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
            return repository.FindBy(id);
        }

        public Department GetDepartment(string name)
        {
            var repository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
            return repository.FindBy(n => n.Name == name);
        }
    }
}
