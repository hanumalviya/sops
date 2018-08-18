using System;
using System.Linq;
using Model.System;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;

namespace SOPS.Services.System
{
    public class UniversityDetailsProvider : IUniversityDetailsProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public UniversityDetailsProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public University GetUniversity()
        {                    
            var reposiotry = _repositoriesFactory.CreateUniversityRepository(_unitOfWork);
            return reposiotry.All().FirstOrDefault();
        }
    }
}
