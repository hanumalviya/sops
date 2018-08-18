using Model.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using SOPS.Services.Utilities;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;

namespace SOPS.Services.Offers
{
    public class OffersProvider : IOffersProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public OffersProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Offer> GetOffers(string filter)
        {
            var repository = _repositoriesFactory.CreateOfferRepository(_unitOfWork);

            return repository.All().ToList().Where(n => n.Description.InsensitiveContains(filter) ||
            n.Company.Name.InsensitiveContains(filter) ||
            n.Type.Name.InsensitiveContains(filter) ||
            n.Title.InsensitiveContains(filter) ||
            n.Trade.InsensitiveContains(filter)).ToList();
        }

        public IList<Offer> GetOffers()
        {
            var repository = _repositoriesFactory.CreateOfferRepository(_unitOfWork);
            return repository.All().ToList();
        }


        public Offer GetOffer(int id)
        {
            var repository = _repositoriesFactory.CreateOfferRepository(_unitOfWork);
            return repository.FindBy(id);
        }
    }
}
