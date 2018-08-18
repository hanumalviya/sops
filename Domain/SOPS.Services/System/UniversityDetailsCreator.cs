using Model.System;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.System
{
    public class UniversityDetailsCreator : IUniversityDetailsCreator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoriesFactory _repositoriesFactory;

        public UniversityDetailsCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _unitOfWork = unitOfWork;
            _repositoriesFactory = repositoriesFactory;
        }

        public Model.System.University Create(string name, string address)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateUniversityRepository(_unitOfWork);
                var university = new University() { Name = name, Address = address };
                repository.Add(university);

                _unitOfWork.Commit();

                return university;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
