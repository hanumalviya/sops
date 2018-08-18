using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Documents
{
    public class DocumentDestructor : IDocumentDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateDocumentRepository(_unitOfWork);
                var doc = repository.FindBy(id);
                repository.Delete(doc);
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
