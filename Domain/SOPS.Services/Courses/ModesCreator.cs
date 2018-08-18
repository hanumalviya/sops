using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Courses
{
    public class ModesCreator : IModesCreator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoriesFactory _repositoriesFactory;

        public ModesCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _unitOfWork = unitOfWork;
            _repositoriesFactory = repositoriesFactory;
        }

        public Mode Create(string name)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var modesRepository = _repositoriesFactory.CreateModeRepository(_unitOfWork);
                var mode = new Model.University.Mode() { Name = name };
                modesRepository.Add(mode);

                _unitOfWork.Commit();

                return mode;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
