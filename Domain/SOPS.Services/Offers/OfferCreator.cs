using Model.Companies;
using Model.Offers;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public class OfferCreator : IOfferCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public OfferCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Offer Create(string title, string description, string trade, Company company, OfferType type)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var reposiotry = _repositoriesFactory.CreateOfferRepository(_unitOfWork);
                var offer = new Offer()
                {
                    Approved = true,
                    Date = DateTime.Now,
                    Title = title,
                    Description = description,
                    Trade = trade,
                    Company = company,
                    Type = type
                    
                };
                reposiotry.Add(offer);
                _unitOfWork.Commit();

                company.Offers.Add(offer);

                return offer;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
