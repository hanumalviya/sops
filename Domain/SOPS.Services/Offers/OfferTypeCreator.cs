using Model.Offers;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public class OfferTypeCreator : IOfferTypeCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public OfferTypeCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _unitOfWork = unitOfWork;
            _repositoriesFactory = repositoriesFactory;
        }

        public Model.Offers.OfferType Create(string name)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateOfferTypeRepository(_unitOfWork);

                var offerType = new OfferType() { Name = name };
                repository.Add(offerType);

                _unitOfWork.Commit();

                return offerType;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
