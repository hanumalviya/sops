using Model.Students;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Contracts
{
    public class ContractUpdater : IContractUpdater
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public ContractUpdater(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Update(Contract contract)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateContractRepository(_unitOfWork);
                repository.Update(contract);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
