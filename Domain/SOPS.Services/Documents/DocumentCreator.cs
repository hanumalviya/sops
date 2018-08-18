using System;
using System.Linq;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;
using Model.System;

namespace SOPS.Services.Documents
{
    public class DocumentCreator : IDocumentCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Create(string fileName, string filePath, string contentType)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var docRepository = _repositoriesFactory.CreateDocumentRepository(_unitOfWork);
                Document doc = new Document() { Name = fileName, Path = filePath, ContentType = contentType };
                docRepository.Add(doc);
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
