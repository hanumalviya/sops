using System;
using System.Linq;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;

namespace SOPS.Services.System
{
    public class UniversityDetailsUpdater : IUniversityDetailsUpdater
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public UniversityDetailsUpdater(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Update(Model.System.University university)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var reposiotry = _repositoriesFactory.CreateUniversityRepository(_unitOfWork);
                reposiotry.Update(university);
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
