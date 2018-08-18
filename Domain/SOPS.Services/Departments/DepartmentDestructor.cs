using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Departments
{
    public class DepartmentDestructor : IDepartmentDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
                var department = repository.FindBy(id);
                repository.Delete(department);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public bool CanBeDestroyed(int id)
        {            
            var repository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
            var department = repository.FindBy(id);
            return department.Courses.Any() == false;
        }
    }
}
