using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Departments
{
    public class DepartmentCreator : IDepartmentCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Department Create(string name)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
                var department = new Department() { Name = name };
                repository.Add(department);
                _unitOfWork.Commit();

                return department;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
