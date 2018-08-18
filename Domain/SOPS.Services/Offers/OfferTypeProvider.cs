using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Offers
{
    public class OfferTypeProvider : IOfferTypeProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public OfferTypeProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Model.Offers.OfferType> GetAllOfferTypes()
        {            
            var repository = _repositoriesFactory.CreateOfferTypeRepository(_unitOfWork);
            return repository.All().ToList();
        }

        public Model.Offers.OfferType GetOfferType(int id)
        {
            var repository = _repositoriesFactory.CreateOfferTypeRepository(_unitOfWork);
            return repository.FindBy(id);
        }
    }
}
