using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public class OfferDestructor : IOfferDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public OfferDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var reposiotry = _repositoriesFactory.CreateOfferRepository(_unitOfWork);
                var entity = reposiotry.FindBy(id);
                reposiotry.Delete(entity);
                _unitOfWork.Commit();

                entity.Company.Offers.Remove(entity);
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
