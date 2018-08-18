using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Courses
{
    public class ModesProvider : IModesProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public ModesProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Mode> GetModes()
        {            
            var repository = _repositoriesFactory.CreateModeRepository(_unitOfWork);
            return repository.All().ToList();
        }
    }
}
