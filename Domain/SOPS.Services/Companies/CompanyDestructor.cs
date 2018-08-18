using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Companies
{
    public class CompanyDestructor : ICompanyDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateCompanyRepository(_unitOfWork);
                var company = repository.FindBy(id);
                repository.Delete(company);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }


        public bool CanBeDestroy(int id)
        {
            var repository = _repositoriesFactory.CreateCompanyRepository(_unitOfWork);
            var company = repository.FindBy(id);

            return company.Contracts.Any() == false;
        }
    }
}
