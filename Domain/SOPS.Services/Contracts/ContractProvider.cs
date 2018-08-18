using System;
using System.Linq;
using Model.Students;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;

namespace SOPS.Services.Contracts
{
    public class ContractProvider : IContractProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public ContractProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Contract GetContract(int id)
        {            
            var repository = _repositoriesFactory.CreateContractRepository(_unitOfWork);
            return repository.FindBy(id);
        }
    }
}
