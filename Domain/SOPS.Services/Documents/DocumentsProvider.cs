using Model.System;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Documents
{
    public class DocumentsProvider : IDocumentsProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentsProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Document> GetAllDocuments()
        {            
            var repository = _repositoriesFactory.CreateDocumentRepository(_unitOfWork);
            return repository.All().ToList();
        }

        public Document GetDocument(int id)
        {            
            var repository = _repositoriesFactory.CreateDocumentRepository(_unitOfWork);
            return repository.FindBy(id);        
        }
    }
}
